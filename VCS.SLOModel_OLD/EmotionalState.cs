using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VCS.SLOModel_OLD
{
	public class EmotionalState
	{
		public string Context { get; set; }

		public Emotion Emotion { get; set; }

		public string OtherEmotion { get; set; }

		public int Intensity { get; set; }
	}
}
