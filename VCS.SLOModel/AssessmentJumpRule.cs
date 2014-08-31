using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace VCS
{
	[DataContract(IsReference = true)]
	public class AssessmentJumpRule
	{
		[DataMember]
		public bool CheckTestResult { get; set; }

		[DataMember]
		public double MinResult { get; set; }

		[DataMember]
		public bool MinResultIncluded { get; set; }

		[DataMember]
		public double MaxResult { get; set; }

		[DataMember]
		public bool MaxResultIncluded { get; set; }

		[DataMember]
		public bool CheckNumberOfIterations { get; set; }

		[DataMember]
		public int MinNumberOfIterations { get; set; }

		[DataMember]
		public int MaxNumberOfIterations { get; set; }

		[DataMember]
		public bool CheckPrerequisites { get; set; }

		[DataMember]
		public bool MeetsPrerequisites { get; set; }

		[DataMember]
		public Scene SceneToJump { get; set; }

		[DataMember]
		public bool End { get; set; }

		[DataMember]
		public string FeedbackMessage { get; set; }

		public AssessmentJumpRule Clone()
		{
			AssessmentJumpRule clone = new AssessmentJumpRule();

			clone.SceneToJump = this.SceneToJump;

			clone.MinResult = this.MinResult;
			clone.MinResultIncluded = this.MinResultIncluded;

			clone.MaxResult = this.MaxResult;
			clone.MaxResultIncluded = this.MaxResultIncluded;

			clone.MinNumberOfIterations = this.MinNumberOfIterations;

			clone.MaxNumberOfIterations = this.MaxNumberOfIterations;

			clone.MeetsPrerequisites = this.MeetsPrerequisites;

			clone.End = this.End;

			clone.FeedbackMessage = this.FeedbackMessage;

			clone.CheckTestResult = this.CheckTestResult;
			clone.CheckNumberOfIterations = this.CheckNumberOfIterations;
			clone.CheckPrerequisites = this.CheckPrerequisites;

			return clone;
		}

		public bool IsSatisfied(double testResult, int testAttempt, bool meetsPrerequisities)
		{
			bool satisfied = true;

			if (CheckTestResult)
			{
				if (MinResultIncluded)
				{
					satisfied = satisfied && testResult >= MinResult;
				}
				else
				{
					satisfied = satisfied && testResult > MinResult;
				}

				if (MaxResultIncluded)
				{
					satisfied = satisfied && testResult <= MaxResult;
				}
				else
				{
					satisfied = satisfied && testResult < MaxResult;
				}
			}

			if (satisfied && CheckNumberOfIterations)
			{
				satisfied = satisfied && testAttempt >= MinNumberOfIterations && testAttempt <= MaxNumberOfIterations;
			}

			if (satisfied && CheckPrerequisites)
			{
				satisfied = satisfied && meetsPrerequisities == MeetsPrerequisites;
			}

			return satisfied;
		}

		public string Description
		{
			get
			{
				List<string> texts = new List<string>();

				if (CheckTestResult)
				{
					texts.Add("Result " + (MinResultIncluded ? "[" : "(") + MinResult + ", " + MaxResult + (MaxResultIncluded ? "]" : ")"));
				}

				if (CheckNumberOfIterations)
				{
					texts.Add("Nº iter. " + "[" + MinNumberOfIterations + ", " + (MaxNumberOfIterations == Int32.MaxValue ? "N" : MaxNumberOfIterations.ToString()) + "]");
				}

				if (CheckPrerequisites)
				{
					texts.Add("Prereq. " + (MeetsPrerequisites ? "Y" : "N"));
				}

				if (SceneToJump != null)
				{
					texts.Add("Jump to " + SceneToJump.Order);
				}
				else if (End)
				{
					texts.Add("End");
				}

				return String.Join(" - ", texts);
			}
		}
	}
}
