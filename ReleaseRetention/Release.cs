using ReleaseRetentionLibrary.Interfaces;

namespace ReleaseRetentionLibrary
{
	public class Release : IRelease
	{
		public string Id { get; set; }
		public string ProjectId { get; set; }
		public string Version { get; set; }
		public string Created { get; set; }

		public Release(IRelease release)
		{
			Id = release.Id;
			ProjectId = release.ProjectId;
			Version = release.Version;
			Created = release.Created;
		}

		public Release()
		{
			
		}
	}
}