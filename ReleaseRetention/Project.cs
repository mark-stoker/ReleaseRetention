using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using ReleaseRetentionLibrary.Interfaces;

namespace ReleaseRetentionLibrary
{
	public class Project : IProject
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public IList<IEnvironment> Environments { get; set; }
		public IList<IRelease> Releases { get; set; }
		public IList<IDeployment> Deployments { get; set; }

		private readonly ILogger<Project> _logger;
		private int _releasesToKeep = Int32.MaxValue;

		public Project(IProject project, IList<IEnvironment> environments, IList<IRelease> releases, IList<IDeployment> deployments, ILogger<Project> logger = null)
		{
			_logger = logger;
			Id = project.Id;
			Name = project.Name;
			Environments = environments;
			CreateProjectReleases(releases);
			CreateProjectDeployments(deployments);
		}

		public Project()
		{
				
		}

		public void UpdateRetainedDeployedReleases(int numberOfReleases)
		{
			_logger?.LogInformation($"The number of releases to keep has been changed to  {numberOfReleases}." +
			                        $". Only the most recent releases will have been kept.");

			_releasesToKeep = numberOfReleases;
			
			RetainedDeployments();
		}

		public int NumberRetainedDeployedReleases()
		{
			
			return _releasesToKeep;
		}

		private IList<IDeployment> RetainedDeployments()
		{
			var nResults = FilterOutNResults();
			
			ReconstructDeployments(nResults);

			return Deployments;
		}

		private void ReconstructDeployments(List<IEnumerable<IDeployment>> nResults)
		{
			Deployments.Clear();

			foreach (var item in nResults)
			{
				foreach (var deployment in item)
				{
					Deployments.Add(deployment);
				}
			}
		}

		private List<IEnumerable<IDeployment>> FilterOutNResults()
		{
			var nResults = Deployments
				//Group by release id
				.GroupBy(deployment => deployment.ReleaseId)
				//Order by date desc
				.Select(grp =>
					grp.OrderByDescending(deployment => deployment.DeployedAt)
						//Take the n top records from list
						.Take(_releasesToKeep)
				).ToList();
			return nResults;
		}

		private void CreateProjectReleases(IList<IRelease> releases)
		{
			Releases = new List<IRelease>();

			foreach (var release in releases)
			{
				if (release.ProjectId == Id)
					Releases.Add(new Release(release));
			}
		}

		private void CreateProjectDeployments(IList<IDeployment> deployments)
		{
			Deployments = new List<IDeployment>();

			foreach (var deployment in deployments)
			{
				if (Releases.Any(x => x.Id.Contains(deployment.ReleaseId)) && Environments.Any(x => x.Id.Contains(deployment.EnvironmentId)))
					Deployments.Add(new Deployment(deployment));
			}
		}
	}
}