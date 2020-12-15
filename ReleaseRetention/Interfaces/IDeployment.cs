using System;

namespace ReleaseRetentionLibrary.Interfaces
{
	public interface IDeployment
	{
		string Id { get; set; }
		string ReleaseId { get; set; }
		string EnvironmentId { get; set; }
		DateTime DeployedAt { get; set; }
	}
}