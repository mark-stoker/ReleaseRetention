using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using ReleaseRetentionLibrary;
using ReleaseRetentionLibrary.Interfaces;
using ReleaseRetentionTesting.SampleData;

namespace ReleaseRetentionTesting
{
	public class ReleaseRetentionTests
	{
		[Test]
		public void ProjectSetup_PopulateWithTestData_TheCorrectNumberOfProjectsIsReturned()
		{
			//Arrange
			var projects = CreateTestData(out var environments, out var releases, out var deployments);

			//Act
			var results = new Retention(projects, environments, releases, deployments);

			//Assert
			Assert.AreEqual(projects.Count, results.Projects.Count);
		}

		[Test]
		public void ProjectSetup_PopulateWithTestData_TheCorrectNumberOfEnvrionmentsIsReturned()
		{
			//Arrange
			var projects = CreateTestData(out var environments, out var releases, out var deployments);

			var releaseRetention = new Retention(projects, environments, releases, deployments);

			//Act
			var expected = environments.Count;
			var project1Environments =
				releaseRetention["Project-1"].Environments.Count();

			var project2Environments = 
				releaseRetention["Project-2"].Environments.Count;

			//Assert
			Assert.AreEqual(expected, project1Environments);
			Assert.AreEqual(expected, project2Environments);
		}

		[Test]
		public void ProjectSetup_PopulateWithTestData_TheCorrectNumberOfReleasesIsReturned()
		{
			//Arrange
			var projects = CreateTestData(out var environments, out var releases, out var deployments);

			var releaseRetention = new Retention(projects, environments, releases, deployments);

			//Act
			var project1ExpectedReleases = 3;
			var project1Releases =
				releaseRetention["Project-1"].Releases.Count();

			var project2ExpectedReleases = 4;
			var project2Releases =
				releaseRetention["Project-2"].Releases.Count();

			//Assert, One release was discounted because it was for 'Project-3'
			Assert.AreEqual(project1ExpectedReleases, project1Releases);
			Assert.AreEqual(project2ExpectedReleases, project2Releases);
		}

		[Test]
		public void ProjectSetup_PopulateWithTestData_TheCorrectNumberOfDeploymentsIsReturned()
		{
			//Arrange
			var projects = CreateTestData(out var environments, out var releases, out var deployments);

			var releaseRetention = new Retention(projects, environments, releases, deployments);

			//Act
			var project1ExpectedDeployments = 3; //Deployment-4 is for an incorrect EnvironmentId
			var project1Deployments =
				releaseRetention["Project-1"].Deployments.Count();

			var project2ExpectedDeployments = 5; //Deployment-8 is for release with invalid ProjectId
			var project2Deployments =
				releaseRetention["Project-2"].Deployments.Count();

			Assert.AreEqual(project1ExpectedDeployments, project1Deployments);
			Assert.AreEqual(project2ExpectedDeployments, project2Deployments);
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