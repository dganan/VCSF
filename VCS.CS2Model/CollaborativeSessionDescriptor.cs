using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace VCS
{
	[DataContract(IsReference = true)]
	[KnownType(typeof(Site))]
	public class CollaborativeSessionDescriptor : CS2Object
	{
		[DataMember]
		public string Name { get; set; }

		[DataMember]
		public string Description { get; set; }

		[DataMember]
		public Site Site { get; set; }
	}
}
