using System;
using ReleaseRetentionLibrary.Interfaces;

namespace ReleaseRetentionLibrary
{
	public class Deployment : IDeployment
	{
		public string Id { get; set; }
		public string ReleaseId { get; set; }
		public string EnvironmentId { get; set; }
		public DateTime DeployedAt { get; set; }

		public Deployment(IDeployment deployment)
		{
			Id = deployment.Id;
			ReleaseId = deployment.ReleaseId;
			EnvironmentId = deployment.EnvironmentId;
			DeployedAt = deployment.DeployedAt;
		}

		public Deployment()
		{
			
		}
	}
}