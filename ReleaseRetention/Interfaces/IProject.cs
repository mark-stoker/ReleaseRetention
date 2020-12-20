using System.Collections.Generic;

namespace ReleaseRetentionLibrary.Interfaces
{
	public interface IProject
	{
		string Id { get; set; }
		string Name { get; set; }
		IList<IEnvironment> Environments { get; set; }
		IList<IRelease> Releases { get; set; }
		IList<IDeployment> Deployments { get; set; }

		IList<IRelease> UpdateRetainedDeployedReleases(int numberOfReleases, string environment);
		int NumberRetainedDeployedReleases();
	}
}