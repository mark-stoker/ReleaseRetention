namespace ReleaseRetentionLibrary.Interfaces
{
	public interface IReleaseToKeep
	{
		string Environment { get; set; }
		int NReleasesToKeep { get; set; }
	}
}