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
	public class EmotionalTestSceneEditor : SceneEditor
	{
		internal static EmotionalTestScene EditingScene
		{
			get { return SceneEditor.EditingScene as EmotionalTestScene; }
		}

		#region EmotionalTestScene manipulation methods

		//public static string AcceptEdit(string text, Func<Uri, bool> navigate)
		//{
		//    if (String.IsNullOrWhiteSpace(text))
		//    {
		//        return "The scene name is required";
		//    }

		//    //// check if only one correct question

		//    //int correct = EditingAnswersList.Where(x => x.IsCorrectAnswer).Count();

		//    //if (correct != 1)
		//    //{
		//    //    return "Please select one correct answer";
		//    //}

		//    //EditingAssessmentSceneQuestion.QuestionText = text;

		//    //EditingAssessmentSceneQuestion.Answers = EditingAnswersList;

		//    //if (Inserting)
		//    //{
		//    //    AssessmentSceneEditor.AddAssessmentSceneQuestion(EditingAssessmentSceneQuestion);

		//    //    Inserting = false;
		//    //}

		//    //GoBack(navigate);

		//    return null;
		//}

		public static void CancelEdit(Func<Uri, bool> navigate)
		{
			GoBack(navigate);
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

		//internal static void AddEmotionalTestSceneQuestion(EmotionalTestSceneQuestion scenePart)
		//{
		//    EditingScene.Questions.Add(scenePart);
		//}

		//internal static int DuplicateEmotionalTestSceneQuestion(int index)
		//{
		//    EmotionalTestSceneQuestion duplicated = EditingScene.Questions[index].Clone();

		//    EditingScene.Questions.Insert(index + 1, duplicated);

		//    return index + 1;
		//}

		#endregion
	}
}
