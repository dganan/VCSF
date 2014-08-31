using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace VCS
{
	[DataContract(IsReference = true)]
	public class UserAccount : CS2Object
	{
		[DataMember]
		public string Name { get; set; }

		[DataMember]
		public string Email { get; set; }

		[DataMember]
		public Role Role { get; set; }

		[DataMember]
		public Gender Gender { get; set; }

		[DataMember]
		public int Age { get; set; }

		[DataMember]
		public byte[] Avatar { get; set; }
	}
}
