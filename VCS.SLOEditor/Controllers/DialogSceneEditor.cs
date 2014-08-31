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
	public class DialogSceneEditor
	{
		internal static DialogScene EditingScene
		{
			get { return SceneEditor.EditingScene as DialogScene; }
		}

		#region DialogSceneParts manipulation methods

		internal static void CreateDialogScenePart(Func<Uri, bool> navigate)
		{
			DialogScenePartEditor.EditingDialogScenePart = new DialogScenePart();
	
			DialogScenePartEditor.Inserting = true;

			navigate(Pages.DialogScenePartEditorPage);
		}

		internal static void EditDialogScenePart(int sourceIndex, Func<Uri, bool> navigate)
		{
			if (sourceIndex >= 0 && sourceIndex < EditingScene.DialogSceneParts.Count)
			{
				DialogScenePartEditor.EditingDialogScenePart = EditingScene.DialogSceneParts[sourceIndex];

				DialogScenePartEditor.Inserting = false;

				navigate(Pages.DialogScenePartEditorPage);
			}
		}
		
		internal static int RemoveDialogScenePart(int index)
		{
			EditingScene.DialogSceneParts.RemoveAt(index);

			return Math.Min(index, EditingScene.DialogSceneParts.Count - 1);
		}

		private static int MoveScenePart(int sourceIndex, int targetIndex)
		{
			if (targetIndex != sourceIndex && targetIndex >= 0 && targetIndex < EditingScene.DialogSceneParts.Count && sourceIndex >= 0 && sourceIndex < EditingScene.DialogSceneParts.Count)
			{
				DialogScenePart scenePart = EditingScene.DialogSceneParts[sourceIndex];

				EditingScene.DialogSceneParts.RemoveAt(sourceIndex);

				EditingScene.DialogSceneParts.Insert(targetIndex, scenePart);

				return targetIndex;
			}

			return sourceIndex;
		}

		internal static int MoveScenePartToLast(int sourceIndex)
		{
			return MoveScenePart(sourceIndex, EditingScene.DialogSceneParts.Count - 1);
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

		internal static void AddDialogScenePart(DialogScenePart scenePart)
		{
			EditingScene.DialogSceneParts.Add(scenePart);
		}

		internal static int DuplicateDialogScenePart(int index)
		{
			DialogScenePart duplicated = EditingScene.DialogSceneParts[index].Clone();

			EditingScene.DialogSceneParts.Insert (index + 1, duplicated);

			return index + 1;
		}

		#endregion

	}
}
