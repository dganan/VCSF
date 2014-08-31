using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Linq;

namespace VCS
{
	public class AssessmentSceneEditor
	{
		internal static AssessmentScene EditingScene
		{
			get { return SceneEditor.EditingScene as AssessmentScene; }
		}

		#region AssessmentSceneQuestions manipulation methods

		internal static void CreateAssessmentSceneQuestion(Func<Uri, bool> navigate)
		{
			AssessmentSceneQuestionEditor.EditingAssessmentSceneQuestion = new AssessmentSceneQuestion();

			AssessmentSceneQuestionEditor.Inserting = true;

			navigate(Pages.AssessmentSceneQuestionEditorPage);
		}

		internal static void EditAssessmentSceneQuestion(int sourceIndex, Func<Uri, bool> navigate)
		{
			if (sourceIndex >= 0 && sourceIndex < EditingScene.Questions.Count)
			{
				AssessmentSceneQuestionEditor.EditingAssessmentSceneQuestion = EditingScene.Questions[sourceIndex];

				AssessmentSceneQuestionEditor.Inserting = false;

				navigate(Pages.AssessmentSceneQuestionEditorPage);
			}
		}

		internal static int RemoveAssessmentSceneQuestion(int index)
		{
			EditingScene.Questions.RemoveAt(index);

			return Math.Min(index, EditingScene.Questions.Count - 1);
		}

		private static int MoveScenePart(int sourceIndex, int targetIndex)
		{
			if (targetIndex != sourceIndex && targetIndex >= 0 && targetIndex < EditingScene.Questions.Count && sourceIndex >= 0 && sourceIndex < EditingScene.Questions.Count)
			{
				AssessmentSceneQuestion scenePart = EditingScene.Questions[sourceIndex];

				EditingScene.Questions.RemoveAt(sourceIndex);

				EditingScene.Questions.Insert(targetIndex, scenePart);

				return targetIndex;
			}

			return sourceIndex;
		}

		internal static int MoveScenePartToLast(int sourceIndex)
		{
			return MoveScenePart(sourceIndex, EditingScene.Questions.Count - 1);
		}

		internal static int MoveScenePartDown(int sourceIndex)
		{
			return MoveScenePart(sourceIndex, sourceIndex + 1);
		}

		internal static int MoveScenePartUp(int sourceIndex)
		{
			return MoveScenePart(sourceIndex, sourceIndex - 1);
		}

		internal static int MoveScenePartToFirst(int sourceIndex)
		{
			return MoveScenePart(sourceIndex, 0);
		}

		internal static string SetSceneName(string name)
		{
			string error = null;

			if (String.IsNullOrWhiteSpace(name))
			{
				error = "The name of the scene is required";
			}
			else
			{
				EditingScene.Name = name;
			}

			return error;
		}

		internal static void AddAssessmentSceneQuestion(AssessmentSceneQuestion scenePart)
		{
			EditingScene.Questions.Add(scenePart);
		}

		internal static int DuplicateAssessmentSceneQuestion(int index)
		{
			AssessmentSceneQuestion duplicated = EditingScene.Questions[index].Clone();

			EditingScene.Questions.Insert(index + 1, duplicated);

			return index + 1;
		}

		#endregion

		internal static string SetDefaultSceneToJump(object sceneToJump, bool end, string feedback)
		{
			string error = null;

			EditingScene.DefaultEnd = end;

			if (String.IsNullOrWhiteSpace(feedback))
			{
				EditingScene.DefaultFeedbackMessage = null;
			}
			else
			{
				EditingScene.DefaultFeedbackMessage = feedback;
			}

			if (end)
			{
				EditingScene.DefaultSceneToJump = null;
			}
			else
			{
				Scene scene = sceneToJump as Scene;

				if (scene == null)
				{
					error = "The default scene to jump is required";
				}
				else
				{
					EditingScene.DefaultSceneToJump = scene;
				}
			}

			return error;
		}

		#region Jump rules manipulation methods

		internal static int RemoveJumpRule(int index)
		{
			EditingScene.JumpRules.RemoveAt(index);

			return Math.Min(index, EditingScene.JumpRules.Count - 1);
		}

		private static int MoveJumpRule(int sourceIndex, int targetIndex)
		{
			if (targetIndex != sourceIndex && targetIndex >= 0 && targetIndex < EditingScene.JumpRules.Count && sourceIndex >= 0 && sourceIndex < EditingScene.JumpRules.Count)
			{
				AssessmentJumpRule jumpRule = EditingScene.JumpRules[sourceIndex];

				EditingScene.JumpRules.RemoveAt(sourceIndex);

				EditingScene.JumpRules.Insert(targetIndex, jumpRule);

				return targetIndex;
			}

			return sourceIndex;
		}

		internal static int MoveJumpRuleToLast(int sourceIndex)
		{
			return MoveJumpRule(sourceIndex, EditingScene.JumpRules.Count - 1);
		}

		internal static int MoveJumpRuleDown(int sourceIndex)
		{
			return MoveJumpRule(sourceIndex, sourceIndex + 1);
		}

		internal static int MoveJumpRuleUp(int sourceIndex)
		{
			return MoveJumpRule(sourceIndex, sourceIndex - 1);
		}

		internal static int MoveJumpRuleToFirst(int sourceIndex)
		{
			return MoveJumpRule(sourceIndex, 0);
		}

		internal static int DuplicateJumpRule(int index)
		{
			AssessmentJumpRule duplicated = EditingScene.JumpRules[index].Clone();

			EditingScene.JumpRules.Insert(index + 1, duplicated);

			return index + 1;
		}

		#endregion

		internal static string EditJumpRule(int editingJumpRule, bool activateTestResult, string resultMin, bool resultMinIncluded, string resultMax, bool resultMaxIncluded, bool activateTestAttempts, string attemptsMin, string attemptsMax, bool activatePrerequisites, bool prerequisites, object sceneToJump, bool end, string feedback)
		{
			string error = ValidateData(activateTestResult, resultMin, resultMinIncluded, resultMax, resultMaxIncluded, activateTestAttempts, attemptsMin, attemptsMax, activatePrerequisites, prerequisites, sceneToJump, end, feedback);

			if (error == null)
			{
				FillData(EditingScene.JumpRules[editingJumpRule], activateTestResult, resultMin, resultMinIncluded, resultMax, resultMaxIncluded, activateTestAttempts, attemptsMin, attemptsMax, activatePrerequisites, prerequisites, sceneToJump, end, feedback);
			}

			return error;
		}

		internal static string CreateJumpRule(bool activateTestResult, string resultMin, bool resultMinIncluded, string resultMax, bool resultMaxIncluded, bool activateTestAttempts, string attemptsMin, string attemptsMax, bool activatePrerequisites, bool prerequisites, object sceneToJump, bool end, string feedback)
		{
			AssessmentJumpRule jr = new AssessmentJumpRule();

			string error = ValidateData(activateTestResult, resultMin, resultMinIncluded, resultMax, resultMaxIncluded, activateTestAttempts, attemptsMin, attemptsMax, activatePrerequisites, prerequisites, sceneToJump, end, feedback);

			if (error == null)
			{
				FillData(jr, activateTestResult, resultMin, resultMinIncluded, resultMax, resultMaxIncluded, activateTestAttempts, attemptsMin, attemptsMax, activatePrerequisites, prerequisites, sceneToJump, end, feedback);

				EditingScene.JumpRules.Add(jr);
			}

			return error;
		}

		private static string ValidateData(bool activateTestResult, string resultMin, bool resultMinIncluded, string resultMax, bool resultMaxIncluded, bool activateTestAttempts, string attemptsMin, string attemptsMax, bool activatePrerequisites, bool prerequisites, object sceneToJump, bool end, string feedback)
		{
			if (!activateTestResult && !activateTestAttempts && !activatePrerequisites)
			{
				return "At least one condition must be applied!";
			}

			if (activateTestResult)
			{
				double minValue = 0;
				double maxValue = 0;

				if (!Double.TryParse(resultMin, out minValue))
				{
					return "Minimum test result must be a valid double value";
				}

				if (!Double.TryParse(resultMax, out maxValue))
				{
					return "Maximum test result must be a valid double value";
				}

				if (minValue < 0)
				{
					return "Minimum test result must be greater than or equal to 0";
				}

				if (maxValue > 1)
				{
					return "Maximum test result must be less than or to 1";
				}

				if (minValue > maxValue)
				{
					return "Minimum test result must be less than or equal to maximum test result";
				}

				if (minValue == maxValue && !resultMinIncluded && !resultMaxIncluded)
				{
					return "Test result range is empty!";
				}
			}

			if (activateTestAttempts)
			{
				int minValue = 0;
				int maxValue = 0;

				if (!Int32.TryParse(attemptsMin, out minValue))
				{
					return "Minimum test attempts must be a valid integer value";
				}

				if (String.IsNullOrWhiteSpace(attemptsMax))
				{
					maxValue = Int32.MaxValue;
				}
				else if (!Int32.TryParse(attemptsMax, out maxValue))
				{
					return "Maximum test attempts must be a valid integer  value";
				}

				if (minValue < 1)
				{
					return "Minimum test attempts must be greater than or equal to 1";
				}

				if (minValue > maxValue)
				{
					return "Minimum test attempts must be less than or equal to maximum test attempts";
				}
			}

			Scene scene = sceneToJump as Scene;

			if (!end && scene == null)
			{
				return "The scene to jump is required";
			}

			return null;
		}

		private static void FillData(AssessmentJumpRule jr, bool activateTestResult, string resultMin, bool resultMinIncluded, string resultMax, bool resultMaxIncluded, bool activateTestAttempts, string attemptsMin, string attemptsMax, bool activatePrerequisites, bool prerequisites, object sceneToJump, bool end, string feedback)
		{
			jr.CheckTestResult = activateTestResult;

			if (activateTestResult)
			{
				jr.MinResult = Double.Parse(resultMin);
				jr.MaxResult = Double.Parse(resultMax);

				jr.MinResultIncluded = resultMinIncluded;
				jr.MaxResultIncluded = resultMaxIncluded;
			}

			jr.CheckNumberOfIterations = activateTestAttempts;

			if (activateTestAttempts)
			{
				jr.MinNumberOfIterations = Int32.Parse(attemptsMin);

				if (String.IsNullOrWhiteSpace(attemptsMax))
				{
					jr.MaxNumberOfIterations = Int32.MaxValue;
				}
				else
				{
					jr.MaxNumberOfIterations = Int32.Parse(attemptsMax);
				}
			}

			jr.CheckPrerequisites = activatePrerequisites;

			if (activatePrerequisites)
			{
				jr.MeetsPrerequisites = prerequisites;
			}

			jr.End = end;

			if (end)
			{
				jr.SceneToJump = null;
			}
			else
			{
				jr.SceneToJump = sceneToJump as Scene;
			}

			if (String.IsNullOrWhiteSpace(feedback))
			{
				jr.FeedbackMessage = null;
			}
			else
			{
				jr.FeedbackMessage = feedback;
			}
		}
	}
}
