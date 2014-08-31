using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace VCS
{
	[DataContract(IsReference = true)]
	public class AssessmentSceneQuestion : ScenePart
	{
		[DataMember]
		public string QuestionSpeech { get; set; }

		[DataMember]
		public Character Evaluator { get; set; }

		[DataMember]
		public string QuestionText { get; set; }

		public string Speech
		{
			get
			{
				return QuestionSpeech.Trim().RemoveHTMLTags() + ". " + QuestionText;
			}
		}

		[DataMember]
		public List<Answer> Answers { get; set; }

		[DataMember]
		public bool EnableRandomization { get; set; }

		[DataMember]
		public bool EnableMultipleAnswers { get; set; }

		[DataMember]
		public double Score { get; set; }

		[DataMember]
		public byte[] SpeechAudio { get; set; }

		public AssessmentSceneQuestion()
		{
			Answers = new List<Answer>();
		}

		public AssessmentSceneQuestion Clone()
		{
			AssessmentSceneQuestion clone = new AssessmentSceneQuestion();

			clone.QuestionSpeech = this.QuestionSpeech;
			clone.QuestionText = this.QuestionText;
			clone.SpeechAudio = (this.SpeechAudio == null ? null : (byte[])this.SpeechAudio.Clone());
			clone.Evaluator = this.Evaluator.Clone();
			clone.Answers = this.Answers;
			clone.EnableMultipleAnswers = this.EnableMultipleAnswers;
			clone.EnableRandomization = this.EnableRandomization;
			clone.Score = this.Score;

			return clone;
		}
	}
}
