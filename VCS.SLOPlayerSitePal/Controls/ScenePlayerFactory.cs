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

namespace VCS
{
	public static class ScenePlayerFactory
	{
		public static ScenePlayer GetPlayer(Scene s)
		{
			ScenePlayer sp = null;

			if (s is DialogScene)
			{
				//sp = new DialogScenePlayerSimple();
				sp = new DialogScenePlayer();
			}
			else if (s is VideoScene)
			{
				sp = new VideoScenePlayer();
			}
			else if (s is AssessmentScene)
			{
				sp = new AssessmentScenePlayer();
			}
			else if (s is EmotionalTestScene)
			{
				sp = new EmotionalTestScenePlayer();
			}
			else if (s is ReferencesScene)
			{
				sp = new ReferencesScenePlayer();
			}
			
			if (sp != null) { sp.Scene = s; } 

			return sp;
		}
	}
}
