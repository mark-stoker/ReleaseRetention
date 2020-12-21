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
		public void DeployedReleasesToReturn_NoActionTaken_ReturnsIntMaxValue()
		{
			//Arrange and Act
			var projects = CreateTestData(out var environments, out var releases, out var deployments);

			var project = new Project(projects.FirstOrDefault(), environments, releases, deployments);

			//Assert
			Assert.AreEqual(int.MaxValue, project.NumberRetainedDeployedReleases());
		}

		[Test]
		public void StagingDeployedReleasesToReturn_UpdateNumberOfReleasesToReturn_NewValueIsReturned()
		{
			//Arrange
			var projects = CreateTestData(out var environments, out var releases, out var deployments);

			var project = new Project(projects.FirstOrDefault(), environments, releases, deployments);

			//Act
			var numberOfReleasesToKeep = 3;
			var environment = "Environment-1"; //Staging
			project.UpdateRetainedDeployedReleases(numberOfReleasesToKeep, environment);

			//Assert
			Assert.AreEqual(numberOfReleasesToKeep, project.NumberRetainedDeployedReleases());
		}

		[Test]
		public void ProductionDeployedReleasesToReturn_UpdateNumberOfReleasesToReturn_NewValueIsReturned()
		{
			//Arrange
			var projects = CreateTestData(out var environments, out var releases, out var deployments);

			var project = new Project(projects.FirstOrDefault(), environments, releases, deployments);

			//Act
			var numberOfReleasesToKeep = 3;
			var environment = "Environment-2"; //Production
			project.UpdateRetainedDeployedReleases(numberOfReleasesToKeep, environment);

			//Assert
			Assert.AreEqual(numberOfReleasesToKeep, project.NumberRetainedDeployedReleases());
		}

		[Test]
		public void StagingDeployedReleasesToReturn_Project1UpdatesToFirstMostRecentRelease_ReturnsTheTopMostRecentRelease()
		{
			//Arrange
			var projects = CreateTestData(out var environments, out var releases, out var deployments);

			var project1 = new Project(projects.FirstOrDefault(), environments, releases, deployments);

			//Act
			var numberODeploymentsToKeep = 1;
			var environment = "Environment-1"; //Staging
			project1.UpdateRetainedDeployedReleases(numberODeploymentsToKeep, environment);

			var releasesResult = project1.Releases;
			var deploymentsResult = project1.Deployments;

			//Assert
			Assert.AreEqual(false, releasesResult.Any(x => x.Id == "Release-1"));
			Assert.AreEqual(false, deploymentsResult.Any(x => x.Id == "Deployment-1"));
			Assert.AreEqual(false, deploymentsResult.Any(x => x.Id == "Deployment-3"));

			Assert.AreEqual(true, releasesResult.Any(x => x.Id == "Release-2"));
			Assert.AreEqual(true, deploymentsResult.Any(x => x.Id == "Deployment-2"));

			Assert.AreEqual(false, releasesResult.Any(x => x.Id == "Release-3"));

			Assert.LessOrEqual(releasesResult.Count, numberODeploymentsToKeep);
		}

		[Test]
		public void ProductionDeployedReleasesToReturn_Project1UpdatesToFirstMostRecentRelease_ReturnsTheTopMostRecentRelease()
		{
			//Arrange
			var projects = CreateTestData(out var environments, out var releases, out var deployments);

			var project1 = new Project(projects.FirstOrDefault(), environments, releases, deployments);

			//Act
			var numberODeploymentsToKeep = 1;
			var environment = "Environment-2"; //Production
			project1.UpdateRetainedDeployedReleases(numberODeploymentsToKeep, environment);

			var releasesResult = project1.Releases;
			var deploymentsResult = project1.Deployments;

			//Assert
			Assert.AreEqual(true, releasesResult.Any(x => x.Id == "Release-1"));
			Assert.AreEqual(false, deploymentsResult.Any(x => x.Id == "Deployment-1"));
			Assert.AreEqual(true, deploymentsResult.Any(x => x.Id == "Deployment-3"));

			Assert.AreEqual(false, releasesResult.Any(x => x.Id == "Release-2"));
			Assert.AreEqual(false, deploymentsResult.Any(x => x.Id == "Deployment-2"));

			Assert.AreEqual(false, releasesResult.Any(x => x.Id == "Release-3"));

			Assert.LessOrEqual(releasesResult.Count, numberODeploymentsToKeep);
		}

		[Test]
		public void StagingDeployedReleasesToReturn_Project2SetsTop2ReleasesToKeep_Top2ReleasesAreReturned()
		{
			//Arrange
			var projects = CreateTestData(out var environments, out var releases, out var deployments);

			var project = projects.FirstOrDefault(x => x.Id == "Project-2");
			var project2 = new Project(project, environments, releases, deployments);

			//Act
			var numberODeploymentsToKeep = 2;
			var environment = "Environment-1"; //Staging
			project2.UpdateRetainedDeployedReleases(numberODeploymentsToKeep, environment);

			var releasesResult = project2.Releases;
			var deploymentsResult = project2.Deployments;

			//Assert
			Assert.AreEqual(false, releasesResult.Any(x => x.Id == "Release-4"));

			Assert.AreEqual(false, releasesResult.Any(x => x.Id == "Release-5"));
			Assert.AreEqual(false, deploymentsResult.Any(x => x.Id == "Deployment-5"));

			Assert.AreEqual(true, releasesResult.Any(x => x.Id == "Release-6")); //Has most recent deployment
			Assert.AreEqual(true, deploymentsResult.Any(x => x.Id == "Deployment-6"));
			Assert.AreEqual(false, deploymentsResult.Any(x => x.Id == "Deployment-7"));
			Assert.AreEqual(true, deploymentsResult.Any(x => x.Id == "Deployment-9"));

			Assert.AreEqual(true, releasesResult.Any(x => x.Id == "Release-7")); //Second most recent deployment
			Assert.AreEqual(true, deploymentsResult.Any(x => x.Id == "Deployment-8"));

			Assert.LessOrEqual(releasesResult.Count, numberODeploymentsToKeep);
		}

		[Test]
		public void ProductionDeployedReleasesToReturn_Project2SetsTop2ReleasesToKeep_Top2ReleasesAreReturned()
		{
			//Arrange
			var projects = CreateTestData(out var environments, out var releases, out var deployments);

			var project = projects.FirstOrDefault(x => x.Id == "Project-2");
			var project2 = new Project(project, environments, releases, deployments);

			//Act
			var numberODeploymentsToKeep = 2;
			var environment = "Environment-2"; //Production
			project2.UpdateRetainedDeployedReleases(numberODeploymentsToKeep, environment);

			var releasesResult = project2.Releases;
			var deploymentsResult = project2.Deployments;

			//Assert
			//Included because left outer join
			Assert.AreEqual(true, releasesResult.Any(x => x.Id == "Release-4"));

			Assert.AreEqual(false, releasesResult.Any(x => x.Id == "Release-5"));
			Assert.AreEqual(false, deploymentsResult.Any(x => x.Id == "Deployment-5"));

			Assert.AreEqual(true, releasesResult.Any(x => x.Id == "Release-6")); //Has most recent deployment
			Assert.AreEqual(false, deploymentsResult.Any(x => x.Id == "Deployment-6"));
			Assert.AreEqual(true, deploymentsResult.Any(x => x.Id == "Deployment-7"));
			Assert.AreEqual(false, deploymentsResult.Any(x => x.Id == "Deployment-9"));

			Assert.AreEqual(false, releasesResult.Any(x => x.Id == "Release-7")); //Second most recent deployment
			Assert.AreEqual(false, deploymentsResult.Any(x => x.Id == "Deployment-8"));

			Assert.LessOrEqual(releasesResult.Count, numberODeploymentsToKeep);
		}

		[Test]
		public void StagingDeployedReleasesToReturn_NumberOfReleasesToKeepZero_ReturnsNoReleases()
		{
			//Arrange
			var projects = CreateTestData(out var environments, out var releases, out var deployments);

			var project1 = new Project(projects.FirstOrDefault(), environments, releases, deployments);

			//Act
			var numberODeploymentsToKeep = 0;
			var environment = "Environment-1"; //Staging
			project1.UpdateRetainedDeployedReleases(numberODeploymentsToKeep, environment);

			var releasesResult = project1.Releases;

			//Assert
			Assert.LessOrEqual(releasesResult.Count, numberODeploymentsToKeep);
		}


		[Test]
		public void ProductionDeployedReleasesToReturn_NumberOfReleasesToKeepZero_ReturnsNoReleases()
		{
			//Arrange
			var projects = CreateTestData(out var environments, out var releases, out var deployments);

			var project1 = new Project(projects.FirstOrDefault(), environments, releases, deployments);

			//Act
			var numberODeploymentsToKeep = 0;
			var environment = "Environment-2"; //Production
			project1.UpdateRetainedDeployedReleases(numberODeploymentsToKeep, environment);

			var releasesResult = project1.Releases;

			//Assert
			Assert.LessOrEqual(releasesResult.Count, numberODeploymentsToKeep);
		}

		[Test]
		public void StagingDeployedReleasesToReturn_UpdateNumberOfReleasesToRandomHighNumber_AllReleasesForProjectReturned()
		{
			//Arrange
			var projects = CreateTestData(out var environments, out var releases, out var deployments);

			var project1 = new Project(projects.FirstOrDefault(), environments, releases, deployments);

			//Act
			var numberODeploymentsToKeep = 1000;
			var environment = "Environment-1"; //Staging
			project1.UpdateRetainedDeployedReleases(numberODeploymentsToKeep, environment);

			var releasesResult = project1.Releases;
			var deploymentsResult = project1.Deployments;

			//Assert
			Assert.AreEqual(true, releasesResult.Any(x => x.Id == "Release-1"));
			Assert.AreEqual(true, deploymentsResult.Any(x => x.Id == "Deployment-1"));
			Assert.AreEqual(false, deploymentsResult.Any(x => x.Id == "Deployment-3"));

			Assert.AreEqual(true, releasesResult.Any(x => x.Id == "Release-2"));
			Assert.AreEqual(true, deploymentsResult.Any(x => x.Id == "Deployment-2"));

			Assert.AreEqual(true, releasesResult.Any(x => x.Id == "Release-3"));

			Assert.LessOrEqual(releasesResult.Count, numberODeploymentsToKeep);
		}

		[Test]
		public void ProductionDeployedReleasesToReturn_UpdateNumberOfReleasesToRandomHighNumber_AllReleasesForProjectReturned()
		{
			//Arrange
			var projects = CreateTestData(out var environments, out var releases, out var deployments);

			var project1 = new Project(projects.FirstOrDefault(), environments, releases, deployments);

			//Act
			var numberODeploymentsToKeep = 1000;
			var environment = "Environment-2"; //Production
			project1.UpdateRetainedDeployedReleases(numberODeploymentsToKeep, environment);

			var releasesResult = project1.Releases;
			var deploymentsResult = project1.Deployments;

			//Assert
			Assert.AreEqual(true, releasesResult.Any(x => x.Id == "Release-1"));
			Assert.AreEqual(false, deploymentsResult.Any(x => x.Id == "Deployment-1"));
			Assert.AreEqual(true, deploymentsResult.Any(x => x.Id == "Deployment-3"));

			Assert.AreEqual(true, releasesResult.Any(x => x.Id == "Release-2"));
			Assert.AreEqual(false, deploymentsResult?.Any(x => x.Id == "Deployment-2"));

			Assert.AreEqual(true, releasesResult.Any(x => x.Id == "Release-3"));

			Assert.LessOrEqual(releasesResult.Count, numberODeploymentsToKeep);
		}

		[Test]
		public void StagingDeployedReleasessToReturn_UpdateNumberOfReleasesToNegativeNumber_NoDeploymentsForProjectReturned()
		{
			//Arrange
			var projects = CreateTestData(out var environments, out var releases, out var deployments);

			var project1 = new Project(projects.FirstOrDefault(), environments, releases, deployments);

			//Act
			var numberODeploymentsToKeep = -50;
			var environment = "Environment-1"; //Staging
			project1.UpdateRetainedDeployedReleases(numberODeploymentsToKeep, environment);

			var releasesResult = project1.Deployments;
			var deploymentsResult = project1.Deployments;

			//Assert
			Assert.AreEqual(false, releasesResult.Any(x => x.Id == "Release-1"));
			Assert.AreEqual(false, deploymentsResult.Any(x => x.Id == "Deployment-1"));
			Assert.AreEqual(false, deploymentsResult.Any(x => x.Id == "Deployment-3"));

			Assert.AreEqual(false, releasesResult.Any(x => x.Id == "Release-2"));
			Assert.AreEqual(false, deploymentsResult.Any(x => x.Id == "Deployment-2"));

			Assert.AreEqual(false, releasesResult.Any(x => x.Id == "Release-3"));

			Assert.AreEqual(releasesResult.Count, 0);
		}

		[Test]
		public void ProductionDeployedReleasessToReturn_UpdateNumberOfReleasesToNegativeNumber_NoDeploymentsForProjectReturned()
		{
			//Arrange
			var projects = CreateTestData(out var environments, out var releases, out var deployments);

			var project1 = new Project(projects.FirstOrDefault(), environments, releases, deployments);

			//Act
			var numberODeploymentsToKeep = -50;
			var environment = "Environment-2"; //Production
			project1.UpdateRetainedDeployedReleases(numberODeploymentsToKeep, environment);

			var releasesResult = project1.Deployments;
			var deploymentsResult = project1.Deployments;

			//Assert
			Assert.AreEqual(false, releasesResult.Any(x => x.Id == "Release-1"));
			Assert.AreEqual(false, deploymentsResult.Any(x => x.Id == "Deployment-1"));
			Assert.AreEqual(false, deploymentsResult.Any(x => x.Id == "Deployment-3"));

			Assert.AreEqual(false, releasesResult.Any(x => x.Id == "Release-2"));
			Assert.AreEqual(false, deploymentsResult.Any(x => x.Id == "Deployment-2"));

			Assert.AreEqual(false, releasesResult.Any(x => x.Id == "Release-3"));

			Assert.AreEqual(releasesResult.Count, 0);
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
