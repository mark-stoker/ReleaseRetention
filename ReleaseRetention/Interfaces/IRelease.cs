namespace ReleaseRetentionLibrary.Interfaces
{
	public interface IRelease
	{
		string Id { get; set; }
		string ProjectId { get; set; }
		string Version { get; set; }
		string Created { get; set; }
	}
}