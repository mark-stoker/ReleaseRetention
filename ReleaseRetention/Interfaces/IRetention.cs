using System;
using System.Collections.Generic;
using System.Text;

namespace ReleaseRetentionLibrary.Interfaces
{
	public interface IRetention
	{
		IList<IProject> Projects { get; set; }
	}
}
