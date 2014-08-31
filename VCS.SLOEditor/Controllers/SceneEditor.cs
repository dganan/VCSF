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
	public class SceneEditor
	{
		internal static Scene EditingScene { get; set; }
		public static bool CreatingScene { get; set; }

		internal static bool CheckEditingScene(SceneType type, Func<Uri, bool> navigate)
		{
			if (EditingScene == null)
			{
				if (StoryBoardEditor.CheckEditingSLO(navigate))
				{
					//MessageBox.Show("No SLO is being edited");

					navigate(Pages.StoryBoardEditorPage);

					return false;
				}
			}

			return EditingScene.SceneType == type;
		}

		internal static void GoBack(Func<Uri, bool> navigate)
		{
			if (CreatingScene)
			{
				StoryBoardEditor.AddScene(SceneEditor.EditingScene);
				SceneEditor.CreatingScene = false;
			}

			EditingScene = null;

			navigate(Pages.StoryBoardEditorPage);
		}
	}
}
