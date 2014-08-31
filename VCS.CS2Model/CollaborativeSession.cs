using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace VCS
{
	[DataContract(IsReference = true)]
	public class CollaborativeSession : CS2Object
	{
		[DataMember]
		public string Name { get; set; }

		[DataMember]
		public string Description { get; set; }

		[DataMember]
		public List<Post> Posts { get; set; }

		[DataMember]
		public List<UserAccount> UserAccounts { get; set; }

		[DataMember]
		public Site Site { get; set; }

		[DataMember]
		public string Url { get; set; }

		public CollaborativeSession()
		{
			Posts = new List<Post>();

			UserAccounts = new List<UserAccount>();
		}
	}
}
