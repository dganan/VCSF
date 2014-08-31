using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VCS
{
	public interface IConverter
	{
		List<CollaborativeSessionDescriptor> AvailableCollaborativeSessions { get; }

		CollaborativeSession ReadCollaborativeSession(string csId, params string [] parameters);
	}
}
