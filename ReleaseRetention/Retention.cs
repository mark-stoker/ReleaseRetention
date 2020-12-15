using System.Collections.Generic;
using ReleaseRetentionLibrary.Interfaces;

namespace ReleaseRetentionLibrary
{
	public class Retention : IRetention
	{
		public IList<IProject> Projects { get; set; }

		public Retention(IList<IProject> projects, IList<IEnvironment> environments, 
			IList<IRelease> releases, IList<IDeployment> deployments)
		{
			CreateProjectList(projects, environments, releases, deployments);
		}

		private void CreateProjectList(IList<IProject> projects, IList<IEnvironment> environments,
			IList<IRelease> releases, IList<IDeployment> deployments)
		{
			Projects = new List<IProject>();

			foreach (var project in projects)
			{
				var projectList = new Project(project, environments, releases, deployments);
				Projects.Add(projectList);
			}
		}
	}
}
