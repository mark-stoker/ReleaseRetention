using ReleaseRetentionLibrary.Interfaces;

namespace ReleaseRetentionLibrary
{
	public class Environment : IEnvironment
	{
		public string Id { get; set; }
		public string Name { get; set; }
	}
}