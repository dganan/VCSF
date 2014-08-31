using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace VCS
{
	[DataContract(IsReference = true)]
	public class ReferencesSceneReference : ScenePart
	{
		[DataMember]
		public string Description { get; set; }

		[DataMember]
		public string Url { get; set; }

		public ReferencesSceneReference()
		{
		}

		public ReferencesSceneReference Clone()
		{
			ReferencesSceneReference clone = new ReferencesSceneReference();

			clone.Description = this.Description;
			clone.Url = this.Url;

			return clone;
		}
	}
}
