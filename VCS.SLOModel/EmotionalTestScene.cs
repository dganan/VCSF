using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace VCS
{
	[DataContract(IsReference = true)]
	public class EmotionalTestScene : Scene
	{
		[DataMember]
		public EmotionalTestType EmotionalTestType { get; set; }

		//[DataMember]
		//public List<EmotionalJumpCondition> EmotionalJumpConditions { get; set; }

		[DataMember]
		public Scene DefaultNextSceneToJump { get; set; }

		public EmotionalTestScene()
			: base()
		{
//			EmotionalJumpConditions = new List<EmotionalJumpCondition>();
		}

		[DataMember]
		public string MoodQuestion { get; set; }

		[DataMember]
		public string EmotionFeelNowQuestion { get; set; }

		[DataMember]
		public string EmotionFeelDuringQuestion { get; set; }

		[DataMember]
		public string EmotionFeelExperiencedQuestion { get; set; }

		[DataMember]
		public string GenevaWheelQuestion { get; set; }

		public override SceneType SceneType
		{
			get { return SceneType.EmotionalTest; }
		}

		public override Scene Clone()
		{
			EmotionalTestScene clone = new EmotionalTestScene();

			clone.Name = this.Name;

//			clone.EmotionalJumpConditions = this.EmotionalJumpConditions.Select(x => x.Clone()).ToList();

			clone.DefaultNextSceneToJump = this.DefaultNextSceneToJump;

			clone.MoodQuestion = this.MoodQuestion;
			clone.EmotionFeelNowQuestion = this.EmotionFeelNowQuestion;
			clone.EmotionFeelDuringQuestion = this.EmotionFeelDuringQuestion;
			clone.EmotionFeelExperiencedQuestion = this.EmotionFeelExperiencedQuestion;
			clone.GenevaWheelQuestion = this.GenevaWheelQuestion;

			return clone;
		}

		//public Scene NextScene(Emotion emotional)
		//{
		//    foreach (EmotionalJumpCondition mjc in EmotionalJumpConditions)
		//    {
		//        if (mjc.Jump(emotional))
		//        {
		//            return mjc.SceneToJump;
		//        }
		//    }

		//    return DefaultNextSceneToJump;
		//}
		
		//public override bool CanBeEndScene
		//{
		//    get { return false; }
		//}
	}
}
