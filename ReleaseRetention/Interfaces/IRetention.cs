using System.Collections.Generic;

namespace ReleaseRetentionLibrary.Interfaces
{
	public interface IRetention
	{
		IList<IProject> Projects { get; set; }
	}
}
