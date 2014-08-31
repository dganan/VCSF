using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace VCS
{
	[DataContract(IsReference = true)]
	public class Activity
	{
		[DataMember]
		public string IP { get; set; }

		[DataMember]
		public string UserName { get; set; }

		[DataMember]
		public string Action { get; set; }

		[DataMember]
		public List<Argument> Arguments { get; set; }

		[DataMember]
		public DateTime Date { get; set; }
	}
}