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
using System.Collections.Generic;
using VCS.KeywordsRepositoryService;
using System.Linq;
using VCS.SpeechActClassificationService;

namespace VCS
{
	public class AssessmentSceneQuestionEditor
	{
		private static AssessmentSceneQuestion editingAssessmentSceneQuestion;

		internal static AssessmentSceneQuestion EditingAssessmentSceneQuestion
		{
			get { return editingAssessmentSceneQuestion; }

			set
			{
				editingAssessmentSceneQuestion = value;

				if (value != null)
				{
					EditingAnswersList = value.Answers.Select(x => x.Clone()).ToList();
				}
				else
				{
					EditingAnswersList = null;
				}
			}
		}

		internal static List<Answer> EditingAnswersList { get; set; }

		internal static bool CheckEditingAssessmentSceneQuestion(Func<Uri, bool> navigate)
		{
			if (EditingAssessmentSceneQuestion == null)
			{
				if (SceneEditor.CheckEditingScene(SceneType.Assessment, navigate))
				{
					GoBack(navigate);

					return false;
				}
			}

			return true;
		}

		public static bool Inserting { get; set; }

		private static void GoBack(Func<Uri, bool> navigate)
		{
			EditingAssessmentSceneQuestion = null;

			navigate(Pages.SceneEditorPage(SceneEditor.EditingScene.SceneType));
		}

		private static int MoveScenePart(int sourceIndex, int targetIndex)
		{
			if (targetIndex != sourceIndex && targetIndex >= 0 && targetIndex < EditingAnswersList.Count && sourceIndex >= 0 && sourceIndex < EditingAnswersList.Count)
			{
				Answer answer = EditingAnswersList[sourceIndex];

				EditingAnswersList.RemoveAt(sourceIndex);

				EditingAnswersList.Insert(targetIndex, answer);

				return targetIndex;
			}

			return sourceIndex;
		}

		internal static int MoveScenePartToLast(int sourceIndex)
		{
			return MoveScenePart(sourceIndex, EditingAnswersList.Count - 1);
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

		internal static string SetAssessmentSceneQuestionText(string text)
		{
			string error = null;

			if (String.IsNullOrWhiteSpace(text))
			{
				error = "The text of the question is required";
			}
			else
			{
				EditingAssessmentSceneQuestion.QuestionText = text;
			}

			return error;
		}

		internal static void AddAnswer(Answer answer)
		{
			EditingAnswersList.Add(answer);
		}

		internal static int DuplicateAnswer(int index)
		{
			Answer duplicated = EditingAnswersList[index].Clone();

			EditingAnswersList.Insert(index + 1, duplicated);

			return index + 1;
		}

		internal static void CreateAnswer(string text)
		{
			EditingAnswersList.Add(new Answer() { AnswerText = text });
		}

		internal static void EditAnswer(int sourceIndex, string text)
		{
			if (sourceIndex >= 0 && sourceIndex < EditingAnswersList.Count)
			{
				EditingAnswersList[sourceIndex].AnswerText = text;
			}
		}

		internal static int RemoveAnswer(int index)
		{
			EditingAnswersList.RemoveAt(index);

			return Math.Min(index, EditingAnswersList.Count - 1);
		}

		public static string AcceptEdit(string questionspeech, string questiontext, string scorestr, string characterName, string characterAvatar, string characterGender, Func<Uri, bool> navigate)
		{
			if (String.IsNullOrWhiteSpace(questionspeech))
			{
				return "The question speech is required";
			}

			if (String.IsNullOrWhiteSpace(questiontext))
			{
				return "The question text is required";
			}

			double score = 0;

			if (String.IsNullOrWhiteSpace(scorestr))
			{
				return "The score of the question is required";
			}
			else
			{
				if (!Double.TryParse(scorestr, out score))
				{
					return "The score must be a valid real number";
				}

				if (score <= 0)
				{
					return "The score must be greater than zero";
				}
			}

			AnimationAvatar animationAvatar;

			if (!Enum.TryParse<AnimationAvatar>(characterAvatar, true, out animationAvatar))
			{
				return "The selected evaluator animation avatar type is not valid";
			}

			Gender gender;

			if (!Enum.TryParse<Gender>(characterGender, true, out gender))
			{
				return "The selected evaluator gender type is not valid";
			}

			// check if only one correct question

			int correct = EditingAnswersList.Where(x => x.IsCorrectAnswer).Count();

			if (correct != 1)
			{
				return "Please select one correct answer";
			}

			EditingAssessmentSceneQuestion.QuestionSpeech = questionspeech;
			EditingAssessmentSceneQuestion.QuestionText = questiontext;

			EditingAssessmentSceneQuestion.Score = score;

			EditingAssessmentSceneQuestion.Answers = EditingAnswersList;

			EditingAssessmentSceneQuestion.Evaluator = new Character() 
			{ 
				Name = (String.IsNullOrWhiteSpace(characterName) ? "Evaluator" : characterName), 
				AnimationAvatar = animationAvatar, 
				UseAnimatedAvatar = true, 
				Gender = gender 
			};

			if (Inserting)
			{
				AssessmentSceneEditor.AddAssessmentSceneQuestion(EditingAssessmentSceneQuestion);

				Inserting = false;
			}

			GoBack(navigate);

			return null;
		}

		public static void CancelEdit(Func<Uri, bool> navigate)
		{
			Inserting = false;

			GoBack(navigate);
		}

		internal static void CheckAnswer(int sourceIndex)
		{
			// depends on if the question allows multiple correct questions or not (by now only one correct answer)

			foreach (Answer a in EditingAnswersList)
			{
				a.IsCorrectAnswer = false;
			}

			EditingAnswersList[sourceIndex].IsCorrectAnswer = true;
		}
	}
}
