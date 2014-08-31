using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VCS
{
	public abstract class Converter : IConverter
	{
		public abstract List<CollaborativeSessionDescriptor> AvailableCollaborativeSessions { get; }

		protected abstract CollaborativeSession ReadCollaborativeSessionData(string csId);

		protected abstract List<UserAccount> ReadUserAccounts(string csId);

		protected abstract List<Post> ReadPosts(string csId);

		public virtual CollaborativeSession ReadCollaborativeSession(string csId, params string [] parameters)
		{
			CollaborativeSession cs2 = ReadCollaborativeSessionData(csId);

			cs2.UserAccounts = ReadUserAccounts(csId);

			cs2.Posts = ReadPosts(csId);

			return cs2;
		}
	}
}
