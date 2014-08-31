using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace VCS
{
	[DataContract(IsReference = true)]
	public class AssessmentScene : Scene
	{
		[DataMember]
		public List<AssessmentSceneQuestion> Questions { get; set; }

		[DataMember]
		public List<AssessmentJumpRule> JumpRules { get; set; }

		[DataMember]
		public Scene DefaultSceneToJump { get; set; }

		[DataMember]
		public bool DefaultEnd { get; set; }

		[DataMember]
		public string DefaultFeedbackMessage { get; set; }

		// FURTHERWORK : improve to have multiple scenes to jump depending on the test result (not only failed / success)

		[DataMember]
		public bool EnableRandomization { get; set; }

		public AssessmentScene()
			: base()
		{
			Questions = new List<AssessmentSceneQuestion>();

			JumpRules = new List<AssessmentJumpRule>();
		}

		public override SceneType SceneType
		{
			get { return SceneType.Assessment; }
		}

		public override Scene Clone()
		{
			AssessmentScene clone = new AssessmentScene();

			clone.Name = this.Name;

			clone.JumpRules = this.JumpRules.Select(x => x.Clone()).ToList();

			clone.DefaultSceneToJump = this.DefaultSceneToJump.Clone();
			clone.DefaultEnd = this.DefaultEnd;
			clone.DefaultFeedbackMessage = this.DefaultFeedbackMessage;

			clone.Questions = this.Questions.Select(x => x.Clone()).ToList();
			clone.EnableRandomization = this.EnableRandomization;

			return clone;
		}

		public override bool CanBeManuallySetAsEndScene
		{
			get { return false; }
		}

		public override bool IsEndScene
		{
			get
			{
				return JumpRules.Where(x => x.End).Count() > 0 || DefaultEnd;
			}

			set
			{
			}
		}

		public override List<Scene> ScenesToJump
		{
			get
			{
				List<Scene> js = new List<Scene>();

				foreach (AssessmentJumpRule jr in JumpRules)
				{
					if (jr.SceneToJump != null)
					{
						js.Add(jr.SceneToJump);
					}
				}

				if (DefaultSceneToJump != null)
				{
					js.Add(DefaultSceneToJump);
				}

				return js;
			}
		}
	}
}
