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

		public IList<IRelease> UpdateRetainedDeployedReleases(int numberOfReleases)
		{
			_logger?.LogInformation($"The number of releases to keep has been changed to  {numberOfReleases}." +
			                        $". Only the most recent deployed releases will have been kept.");

			_releasesToKeep = numberOfReleases;
			
			return RetainedReleases();
		}

		public int NumberRetainedDeployedReleases()
		{
			return _releasesToKeep;
		}

		private IList<IRelease> RetainedReleases()
		{
			//Ordered list of releases based on deployment date
			//Filters out to the n top releases
			var filteredReleases = Releases.GroupJoin(
					Deployments,
					release => release.Id,
					deployment => deployment.ReleaseId,
					(x, y) => new {Release = x, Deployments = y})
				.SelectMany(
					x => x.Deployments.DefaultIfEmpty(),
					(x, y) => new {Releases = x.Release, Deployment = y})
				.OrderByDescending(x => x.Deployment?.DeployedAt)
				.GroupBy(x => x.Releases.Id)
				.Take(_releasesToKeep).ToList();


			var releaseId = new List<string>();
			var deploymentId = new List<string>();

			foreach (var releaseItem in filteredReleases)
			{
				releaseId.Add(releaseItem.Select(x => x.Releases.Id).FirstOrDefault());

				foreach (var deploymentItem in releaseItem.Select(x => x.Deployment))
				{
					if (deploymentItem != null)
						deploymentId.Add(deploymentItem?.Id);
				}
			}

			Releases = Releases.Where(x => releaseId.Contains(x.Id)).ToList();
			Deployments = Deployments.Where(x => deploymentId.Contains(x.Id)).ToList();

			return Releases;
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