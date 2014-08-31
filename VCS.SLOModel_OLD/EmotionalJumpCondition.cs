using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VCS.SLOModel_OLD
{
	public class EmotionalJumpCondition : JumpCondition<Emotion>
	{
		public Emotion Emotion { get; set; }

		public override bool Jump(Emotion emotionalCondition)
		{
			return this.Emotion == emotionalCondition;
		}

		public EmotionalJumpCondition Clone()
		{
			EmotionalJumpCondition clone = new EmotionalJumpCondition();

			clone.SceneToJump = this.SceneToJump;

			clone.Emotion = this.Emotion;

			return clone;
		}
	}
}
