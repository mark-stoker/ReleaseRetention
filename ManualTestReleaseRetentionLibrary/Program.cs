using System;
using System.Collections.Generic;
using System.Linq;
using ReleaseRetentionLibrary;
using ReleaseRetentionLibrary.Interfaces;
using ReleaseRetentionTesting.SampleData;

namespace ManualTestReleaseRetentionLibrary
{
	class Program
	{
		private static IProject _selectedProject;
		private static IList<IProject> _projects;
		private static IList<IEnvironment> _environments;
		private static IList<IRelease> _releases;
		private static IList<IDeployment> _deployments;

		static void Main(string[] args)
		{
			_projects = ConstructTestData.GetProjectsData();
			_environments = ConstructTestData.GetEnvironmentsData();
			_releases = ConstructTestData.GetReleasesData();
			_deployments = ConstructTestData.GetDeploymentsData();

			var releaseRetention = new Retention(_projects, _environments, _releases, _deployments);

			MainMenu();

			bool quitNow = false;

			while (!quitNow)
			{
				switch (Console.ReadLine())
				{
					case "1":
						SelectProject(releaseRetention);
						MainMenu();
						break;
					case "2":
						DisplayProjectReleaseRetention();
						MainMenu();
						break;
					case "3":
						UpdateReleaseRetention();
						MainMenu();
						break;
					case "4":
						DisplayRetainedReleases();
						MainMenu();
						break;
					case "5":
						ResetData();
						MainMenu();
						break;
					case "6":
						Console.WriteLine($"Shutting down releas retention");
						quitNow = true;
						break;
				}
			}
		}

		private static void MainMenu()
		{
			Console.WriteLine();
			Console.WriteLine();
			Console.WriteLine("Choose an option from the following list:");
			Console.WriteLine("\t1 - Select Project.");
			Console.WriteLine("\t2 - Display Project's Release Retention.");
			Console.WriteLine("\t3 - Update Project's Release Retention.");
			Console.WriteLine("\t4 - Display the kept Releases.");
			Console.WriteLine("\t5 - Reset sample data.");
			Console.WriteLine("\t6 - Quit ReleaseRetention.");
			Console.WriteLine();
			Console.WriteLine();
		}

		private static void SelectProject(Retention releaseRetention)
		{
			Console.WriteLine("Enter project name e.g. 'Project-1' or 'Project-2'.");
			var projectName = Convert.ToString(Console.ReadLine());
			_selectedProject = releaseRetention.Projects.FirstOrDefault(x => x.Id == projectName);
			Console.WriteLine();
			Console.WriteLine();
		}

		private static void DisplayProjectReleaseRetention()
		{
			if (_selectedProject == null)
			{
				Console.WriteLine();
				Console.WriteLine();
				Console.WriteLine($"Please first select a project.");
			}
			else
			{
				Console.WriteLine();
				Console.WriteLine();
				Console.WriteLine($"The current Release Retention is set to: "
				                  + _selectedProject?.NumberRetainedDeployedReleases());
			}
		}

		private static void UpdateReleaseRetention()
		{
			if (_selectedProject == null)
			{
				Console.WriteLine();
				Console.WriteLine();
				Console.WriteLine($"Please first select a project.");
			}
			else
			{
				Console.WriteLine("Enter the new Release Retention for the selected project.");
				var releaseRetentionNumber = Convert.ToInt32(Console.ReadLine());
				Console.WriteLine("Enter the Environment you wish this to apply for.");
				var environment = Convert.ToString(Console.ReadLine());
				_selectedProject.UpdateRetainedDeployedReleases(releaseRetentionNumber, environment);
				Console.WriteLine();
				Console.WriteLine();
				Console.WriteLine($"The new release retention number is: "
				                  + _selectedProject?.NumberRetainedDeployedReleases());
			}
		}

		private static void DisplayRetainedReleases()
		{
			if (_selectedProject == null)
			{
				Console.WriteLine();
				Console.WriteLine();
				Console.WriteLine($"Please first select a project.");
			}
			else
			{
				Console.WriteLine("Here are the current Retained Releases: ");
				foreach (var releaseItem in _selectedProject.Releases.ToList())
				{
					Console.WriteLine();
					Console.WriteLine(releaseItem.Id);
					Console.WriteLine(releaseItem.Created);
					Console.WriteLine(releaseItem.ProjectId);
					Console.WriteLine(releaseItem.Version);
					Console.WriteLine();
					
					foreach (var deploymentItem in _selectedProject.Deployments.ToList())
					{
						if (deploymentItem.ReleaseId == releaseItem.Id)
						{
							Console.WriteLine("			" + deploymentItem.Id);
							Console.WriteLine("			" + deploymentItem.DeployedAt);
							Console.WriteLine("			" + deploymentItem.EnvironmentId);
							Console.WriteLine("			" + deploymentItem.ReleaseId);
							Console.WriteLine();
						}
					}
				}
			}
		}

		private static void ResetData()
		{
			_selectedProject = null;
			_projects = ConstructTestData.GetProjectsData();
			_environments = ConstructTestData.GetEnvironmentsData();
			_releases = ConstructTestData.GetReleasesData();
			_deployments = ConstructTestData.GetDeploymentsData();
			Console.WriteLine();
			Console.WriteLine();
			Console.WriteLine($"The sample data has been reset.");
		}
	}
}
