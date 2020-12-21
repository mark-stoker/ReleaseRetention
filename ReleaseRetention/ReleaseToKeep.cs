using System;
using ReleaseRetentionLibrary.Interfaces;

namespace ReleaseRetentionLibrary
{
	public class ReleaseToKeep : IReleaseToKeep
	{
		public string Environment { get; set; }
		public int NReleasesToKeep { get; set; } = Int32.MaxValue;
	}
}