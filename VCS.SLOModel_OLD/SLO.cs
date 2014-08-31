using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace VCS.SLOModel_OLD
{
	[DataContract(IsReference = true)]
	public class SLO : SLOElement
	{
		[DataMember]
		public string Name { get; set; }

		[DataMember]
		public string SourceUrl { get; set; }

		[DataMember]
		public List<Scene> Scenes { get; set; }

		public SLO()
		{
			Scenes = new List<Scene>();
		}
	}
}
