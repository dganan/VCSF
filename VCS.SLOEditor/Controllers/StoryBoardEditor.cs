using System;
using System.Net;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using VCS.SLORepositoryService;

namespace VCS
{
	public class StoryBoardEditor
	{
		internal static SLO EditingSLO { get; set; }

		internal static bool CheckEditingSLO(Func<Uri, bool> navigate)
		{
			if (EditingSLO == null)
			{
				navigate(Pages.AvailableSLOsListPage);

				return false;
			}

			return true;
		}

		internal static string CreateScene(string name, string typestr, Func<Uri, bool> navigate)
		{
			string error = null;

			if (String.IsNullOrWhiteSpace(name))
			{
				error = "The name of the scene is required";
			}
			else
			{
				SceneType type;

				if (Enum.TryParse<SceneType>(typestr, true, out type))
				{
					SceneEditor.EditingScene = SceneFactory.CreateScene(type, name);
					SceneEditor.CreatingScene = true;

					Uri u = Pages.SceneEditorPage(SceneEditor.EditingScene.SceneType);

					if (u != null)
					{
						navigate(u);
					}
				}
				else
				{
					error = "The selected scene type is not valid";
				}
			}

			return error;
		}

		internal static void EditScene(int index, Func<Uri, bool> navigate)
		{
			SceneEditor.EditingScene = EditingSLO.Scenes[index];

			Uri u = Pages.SceneEditorPage(SceneEditor.EditingScene.SceneType);

			if (u != null)
			{
				navigate(u);
			}
		}

		internal static string Save(string name)
		{
			string error = null;

			try
			{
				if (String.IsNullOrWhiteSpace(name))
				{
					error = "The name of the SLO is required";
				}
				else
				{
					EditingSLO.Name = name;

					SLORepositoryServiceClient sloRepository = SLOEditor.SLORepositoryServiceClient;

					sloRepository.UpdateSLOCompleted += (o, ea) =>
					{
						if (ea.Error != null)
						{
							ExceptionHandler.HandleException(ea.Error);
						}
						else
						{
							if (!SLOEditor.Embedded)
							{
								MessageBox.Show("SLO was updated successfully");
							}
						}
					};

					sloRepository.UpdateSLOAsync(EditingSLO, SLOEditor.UserInfo.Id);
				}
			}
			catch (Exception e)
			{
				ExceptionHandler.HandleException(e);
			}

			return error;
		}

		internal static string SaveCopy(string name)
		{
			string error = null;

			try
			{
				if (String.IsNullOrWhiteSpace(name))
				{
					error = "The name of the SLO is required";
				}
				else
				{
					EditingSLO.Name = name;

					SLORepositoryServiceClient sloRepository = SLOEditor.SLORepositoryServiceClient;

					sloRepository.InsertSLOCompleted += (o, ea) =>
					{
						if (ea.Error != null)
						{
							ExceptionHandler.HandleException(ea.Error);
						}
						else
						{
							MessageBox.Show("SLO was saved successfully");
						}
					};

					sloRepository.InsertSLOAsync(EditingSLO, SLOEditor.UserInfo.Id);
				}
			}
			catch (Exception e)
			{
				ExceptionHandler.HandleException(e);
			}

			return error;
		}

		private static int MoveScene(int sourceIndex, int targetIndex)
		{
			if (targetIndex != sourceIndex && targetIndex >= 0 && targetIndex < EditingSLO.Scenes.Count && sourceIndex >= 0 && sourceIndex < EditingSLO.Scenes.Count)
			{
				Scene scene = EditingSLO.Scenes[sourceIndex];

				EditingSLO.Scenes.RemoveAt(sourceIndex);

				EditingSLO.Scenes.Insert(targetIndex, scene);

				return targetIndex;
			}

			return sourceIndex;
		}

		internal static int MoveSceneToLast(int sourceIndex)
		{
			return MoveScene(sourceIndex, EditingSLO.Scenes.Count - 1);
		}

		internal static int MoveSceneDown(int sourceIndex)
		{
			return MoveScene(sourceIndex, sourceIndex + 1);
		}

		internal static int MoveSceneUp(int sourceIndex)
		{
			return MoveScene(sourceIndex, sourceIndex - 1);
		}

		internal static int MoveSceneToFirst(int sourceIndex)
		{
			return MoveScene(sourceIndex, 0);
		}

		internal static int RemoveScene(int sourceIndex)
		{
			if (EditingSLO.Scenes.Where(x => x.ScenesToJump.Contains(EditingSLO.Scenes[sourceIndex])).ToList().Count() > 0)
			{
				return -1;
			} 
			
			EditingSLO.Scenes.RemoveAt(sourceIndex);

			return Math.Min(sourceIndex, EditingSLO.Scenes.Count - 1);
		}

		internal static int DuplicateScene(int index)
		{
			Scene duplicated = EditingSLO.Scenes[index].Clone();

			EditingSLO.Scenes.Insert (index + 1, duplicated);

			return index + 1;
		}

		internal static void ToggleEndScene(int index)
		{
			if (!EditingSLO.Scenes[index].CanBeManuallySetAsEndScene)
			{
				MessageBox.Show("This type of scene cannot be manually set as end scene.");
			}
			else 
			{
				EditingSLO.Scenes[index].IsEndScene = !EditingSLO.Scenes[index].IsEndScene;
			}
		}

		internal static string SetSLOName(string name)
		{
			string error = null;

			if (String.IsNullOrWhiteSpace(name))
			{
				error = "The name of the SLO  is required";
			}
			else
			{
				EditingSLO.Name = name;
			}

			return error;
		}

	
		#region Characters manipulation methods

		internal static void CreateCharacter(Func<Uri, bool> navigate)
		{
			CharacterEditor.EditingCharacter = new Character();

			CharacterEditor.Inserting = true;

			navigate(Pages.CharacterEditorPage);
		}

		internal static void EditCharacter(int sourceIndex, Func<Uri, bool> navigate)
		{
			if (sourceIndex >= 0 && sourceIndex < EditingSLO.Characters.Count)
			{
				CharacterEditor.EditingCharacter = EditingSLO.Characters[sourceIndex];

				CharacterEditor.Inserting = false;

				navigate(Pages.CharacterEditorPage);
			}
		}

		internal static int RemoveCharacter(int index)
		{
			// This must be extended if the list of characters is also used for other kind of scenes
			if (EditingSLO.Scenes.SelectMany (x=>x.UsedCharacters).Where(x => x.Equals(EditingSLO.Characters[index])).Count() > 0)
			{
				return -1;
			}

			EditingSLO.Characters.RemoveAt(index);

			return Math.Min(index, EditingSLO.Characters.Count - 1);
		}

		internal static void AddCharacter(Character character)
		{
			EditingSLO.Characters.Add(character);
		}

		internal static int DuplicateCharacter(int index)
		{
			Character duplicated = EditingSLO.Characters[index].Clone();

			EditingSLO.Characters.Insert(index + 1, duplicated);

			return index + 1;
		}

		#endregion

		internal static void AddScene(Scene scene)
		{
			EditingSLO.Scenes.Add(scene);
		}
	}
}
