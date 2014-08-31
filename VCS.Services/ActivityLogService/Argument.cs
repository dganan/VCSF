using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace VCS
{
	[DataContract(IsReference = true)]
	public class Argument
	{
		[DataMember]
		public string Key { get; set; }

		[DataMember]
		public string Value { get; set; }
	}
}
