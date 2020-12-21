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
		private IList<IReleaseToKeep> ReleasesToKeep { get; set; }

		private readonly ILogger<Project> _logger;
		
		//private int _releasesToKeep = Int32.MaxValue;

		public Project(IProject project, IList<IEnvironment> environments, IList<IRelease> releases, IList<IDeployment> deployments, ILogger<Project> logger = null)
		{
			
			Id = project.Id;
			Name = project.Name;
			Environments = environments;
			CreateProjectReleases(releases);
			CreateProjectDeployments(deployments);
			_logger = logger;
			ReleasesToKeep = new List<IReleaseToKeep>() { new ReleaseToKeep() };
		}

		public Project()
		{
				
		}

		public IList<IRelease> UpdateRetainedDeployedReleases(int numberOfReleases, string environment)
		{
			_logger?.LogInformation($"The number of releases to keep has been changed to  {numberOfReleases}." +
			                        $". Only the most recent deployed releases will have been kept.");

			var releaseToKeep = new ReleaseToKeep
			{
				NReleasesToKeep = numberOfReleases,
				Environment = environment
			};

			ReleasesToKeep.Add(releaseToKeep);
			
			return RetainedReleases();
		}

		public int NumberRetainedDeployedReleases()
		{
			return ReleasesToKeep.LastOrDefault().NReleasesToKeep;
		}

		private IList<IRelease> RetainedReleases()
		{
			var releaseIds = new List<string>();
			var deploymentIds = new List<string>();

			FilterReleasesByN(releaseIds, deploymentIds);

			Releases = Releases.Where(x => releaseIds.Contains(x.Id)).ToList();
			Deployments = Deployments.Where(x => deploymentIds.Contains(x.Id)).ToList();

			return Releases;
		}

		//Update this functionality to include EnvironmentId
		private void FilterReleasesByN(List<string> releaseIds, List<string> deploymentIds)
		{
			foreach (var releaseItem in Releases.GroupJoin(
					Deployments.Where(x => x.EnvironmentId == ReleasesToKeep.LastOrDefault()?.Environment),
					release => release.Id,
					deployment => deployment.ReleaseId,
					(x, y) => new {Release = x, Deployments = y})
				.SelectMany(
					x => x.Deployments.DefaultIfEmpty(),
					(x, y) => new {Releases = x.Release, Deployment = y})
				.OrderByDescending(x => x.Deployment?.DeployedAt)
				.GroupBy(x => x.Releases.Id)
				.Take(ReleasesToKeep.LastOrDefault().NReleasesToKeep).ToList())
			{
				releaseIds.Add(releaseItem.Select(x => x.Releases.Id).FirstOrDefault());

				foreach (var deploymentItem in releaseItem.Select(x => x.Deployment))
				{
					if (deploymentItem != null)
						deploymentIds.Add(deploymentItem?.Id);
				}
			}
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