using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace VCS
{
	[DataContract(IsReference = true)]
	public class EmotionalState
	{
		[DataMember]
		public string Context { get; set; }

		[DataMember]
		public Emotion Emotion { get; set; }

		[DataMember]
		public string OtherEmotion { get; set; }

		[DataMember]
		public int Intensity { get; set; }
	}
}
