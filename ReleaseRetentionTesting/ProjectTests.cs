using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using ReleaseRetentionLibrary;
using ReleaseRetentionLibrary.Interfaces;
using ReleaseRetentionTesting.SampleData;

namespace ReleaseRetentionTesting
{
	public class ProjectTests
	{
		[Test]
		public void NumberOfDeployedReleasesToReturn_NoActionTaken_ReturnsIntMaxValue()
		{
			//Arrange and Act
			var project = new Project();

			//Assert
			Assert.AreEqual(int.MaxValue, project.NumberRetainedDeployedReleases());
		}

		[Test]
		public void NumberOfDeployedReleasesToReturn_UpdateValue_NewValueIsReturned()
		{
			//Arrange
			var projects = CreateTestData(out var environments, out var releases, out var deployments);

			var project = new Project(projects.FirstOrDefault(), environments, releases, deployments);

			//Act
			var numberOfReleasesToKeep = 3;
			project.UpdateRetainedDeployedReleases(numberOfReleasesToKeep);

			//Assert
			Assert.AreEqual(numberOfReleasesToKeep, project.NumberRetainedDeployedReleases());
		}

		[Test]
		public void NumberOfDeploymentsToReturn_Project1UpdateNumberOfReleasesToKeep_ProjectHasNMostRecentDeployments()
		{
			//Arrange
			var projects = CreateTestData(out var environments, out var releases, out var deployments);

			var project1 = new Project(projects.FirstOrDefault(), environments, releases, deployments);

			//Act
			var numberODeploymentsToKeep = 1;
			project1.UpdateRetainedDeployedReleases(numberODeploymentsToKeep);

			var result = project1.Deployments;

			//Assert
			Assert.AreEqual(false, result.Any(x => x.ReleaseId == "Release-1" && x.Id == "Deployment-1"));
			Assert.AreEqual(true, result.Any(x => x.ReleaseId == "Release-1" && x.Id == "Deployment-3"));
			Assert.AreEqual(true, result.Any(x => x.ReleaseId == "Release-2" && x.Id == "Deployment-2"));

			Assert.LessOrEqual(result.Count(x => x.ReleaseId == "Release-1"), numberODeploymentsToKeep);
			Assert.LessOrEqual(result.Count(x => x.ReleaseId == "Release-2"), numberODeploymentsToKeep);
		}

		[Test]
		public void NumberOfDeploymentsToReturn_Project2UpdateNumberOfReleasesToKeep_ProjectHasNMostRecentDeployments()
		{
			//Arrange
			var projects = CreateTestData(out var environments, out var releases, out var deployments);

			var project = projects.FirstOrDefault(x => x.Id == "Project-2");
			var project2 = new Project(project, environments, releases, deployments);

			//Act
			var numberODeploymentsToKeep = 2;
			project2.UpdateRetainedDeployedReleases(numberODeploymentsToKeep);

			var result = project2.Deployments;

			//Assert
			Assert.AreEqual(true, result.Any(x => x.ReleaseId == "Release-5" && x.Id == "Deployment-5"));
			//Deployment-2 is oldest in Release-6
			Assert.AreEqual(false, result.Any(x => x.ReleaseId == "Release-6" && x.Id == "Deployment-6"));
			Assert.AreEqual(true, result.Any(x => x.ReleaseId == "Release-6" && x.Id == "Deployment-7"));
			Assert.AreEqual(true, result.Any(x => x.ReleaseId == "Release-6" && x.Id == "Deployment-9"));
			Assert.AreEqual(true, result.Any(x => x.ReleaseId == "Release-7" && x.Id == "Deployment-8"));

			Assert.LessOrEqual(result.Count(x => x.ReleaseId == "Release-5"), numberODeploymentsToKeep);
			Assert.LessOrEqual(result.Count(x => x.ReleaseId == "Release-6"), numberODeploymentsToKeep);
			Assert.LessOrEqual(result.Count(x => x.ReleaseId == "Release-7"), numberODeploymentsToKeep);
		}

		[Test]
		public void NumberOfDeploymentsToReturn_NumberOfReleasesToKeepZero_ReturnsNoDeployments()
		{
			//Arrange
			var projects = CreateTestData(out var environments, out var releases, out var deployments);

			var project1 = new Project(projects.FirstOrDefault(), environments, releases, deployments);

			//Act
			var numberODeploymentsToKeep = 0;
			project1.UpdateRetainedDeployedReleases(numberODeploymentsToKeep);

			var result = project1.Deployments;

			//Assert
			Assert.LessOrEqual(result.Count, numberODeploymentsToKeep);
		}

		[Test]
		public void NumberOfDeploymentsToReturn_UpdateNumberOfReleasesToRandomHighNumber_AllDeploymentsForProjectReturned()
		{
			//Arrange
			var projects = CreateTestData(out var environments, out var releases, out var deployments);

			var project1 = new Project(projects.FirstOrDefault(), environments, releases, deployments);

			//Act
			var numberODeploymentsToKeep = 1000;
			project1.UpdateRetainedDeployedReleases(numberODeploymentsToKeep);

			var result = project1.Deployments;

			//Assert
			Assert.AreEqual(true, result.Any(x => x.ReleaseId == "Release-1" && x.Id == "Deployment-1"));
			Assert.AreEqual(true, result.Any(x => x.ReleaseId == "Release-1" && x.Id == "Deployment-3"));
			Assert.AreEqual(true, result.Any(x => x.ReleaseId == "Release-2" && x.Id == "Deployment-2"));

			Assert.LessOrEqual(result.Count(x => x.ReleaseId == "Release-1"), numberODeploymentsToKeep);
			Assert.LessOrEqual(result.Count(x => x.ReleaseId == "Release-2"), numberODeploymentsToKeep);
		}

		[Test]
		public void NumberOfDeploymentsToReturn_UpdateNumberOfReleasesToNegativeNumber_NoDeploymentsForProjectReturned()
		{
			//Arrange
			var projects = CreateTestData(out var environments, out var releases, out var deployments);

			var project1 = new Project(projects.FirstOrDefault(), environments, releases, deployments);

			//Act
			var numberODeploymentsToKeep = -50;
			project1.UpdateRetainedDeployedReleases(numberODeploymentsToKeep);

			var result = project1.Deployments;

			//Assert
			Assert.AreEqual(false, result.Any(x => x.ReleaseId == "Release-1" && x.Id == "Deployment-1"));
			Assert.AreEqual(false, result.Any(x => x.ReleaseId == "Release-1" && x.Id == "Deployment-3"));
			Assert.AreEqual(false, result.Any(x => x.ReleaseId == "Release-2" && x.Id == "Deployment-2"));

			Assert.LessOrEqual(result.Count(x => x.ReleaseId == "Release-1"), 0);
			Assert.LessOrEqual(result.Count(x => x.ReleaseId == "Release-2"), 0);
		}

		private static List<IProject> CreateTestData(out List<IEnvironment> environments, out List<IRelease> releases, out List<IDeployment> deployments)
		{
			var projects = ConstructTestData.GetProjectsData();
			environments = ConstructTestData.GetEnvironmentsData();
			releases = ConstructTestData.GetReleasesData();
			deployments = ConstructTestData.GetDeploymentsData();
			return projects;
		}
	}
}
