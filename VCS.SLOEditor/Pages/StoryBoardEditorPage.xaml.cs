using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Navigation;
using VCS.SLORepositoryService;
using System.Windows.Browser;

namespace VCS
{
	public partial class StoryBoardEditorPage : Page
	{
		public StoryBoardEditorPage()
		{
			InitializeComponent();

			if (SLOEditor.Embedded)
			{
				BackButton.Visibility = System.Windows.Visibility.Collapsed;
				SaveButton.Visibility = System.Windows.Visibility.Collapsed;
				SaveAsButton.Visibility = System.Windows.Visibility.Collapsed;
			}

			HtmlPage.RegisterScriptableObject("Page", this); 
		}

		private void LoadData()
		{
			ReloadScenesList();
		}

		private void ReloadScenesList(int index = -1)
		{
			// Update scene order
			for (int i = 0; i < StoryBoardEditor.EditingSLO.Scenes.Count; i++)
			{
				StoryBoardEditor.EditingSLO.Scenes[i].Order = i;
			}

			this.DataContext = null;
			this.DataContext = StoryBoardEditor.EditingSLO;

			if (index >= 0)
			{
				ScenesList.SelectedIndex = index;

				ScenesList.UpdateLayout();

				ScenesList.ScrollIntoView(ScenesList.SelectedItem);
			}
		}

		// Executes when the user navigates to this page.
		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			if (StoryBoardEditor.CheckEditingSLO(this.NavigationService.Navigate))
			{
				LoadData();
			}
		}

		private void BackButton_Click(object sender, RoutedEventArgs e)
		{
			this.NavigationService.Navigate(Pages.AvailableSLOsListPage);
		}

		private void SaveAsButton_Click(object sender, RoutedEventArgs rea)
		{
			string error = StoryBoardEditor.SaveCopy(txtName.Text);

			if (error != null)
			{
				MessageBox.Show(error);
			}
		}

		private void SaveButton_Click(object sender, RoutedEventArgs rea)
		{
			SaveSLO();
		}

		[ScriptableMember]
		public void SaveSLO()
		{
			string error = StoryBoardEditor.Save(txtName.Text);

			if (error != null)
			{
				MessageBox.Show(error);
			}
		}

		private bool CheckIndex()
		{
			bool check = (ScenesList.SelectedIndex >= 0 && ScenesList.SelectedIndex < StoryBoardEditor.EditingSLO.Scenes.Count());

			if (!check)
			{
				MessageBox.Show("You must select a scene first");
			}

			return check;
		}

		private void LastButton_Click(object sender, RoutedEventArgs e)
		{
			if (CheckIndex())
			{
				ReloadScenesList(StoryBoardEditor.MoveSceneToLast(ScenesList.SelectedIndex));
			}
		}

		private void DownButton_Click(object sender, RoutedEventArgs e)
		{
			if (CheckIndex())
			{
				ReloadScenesList(StoryBoardEditor.MoveSceneDown(ScenesList.SelectedIndex));
			}
		}

		private void UpButton_Click(object sender, RoutedEventArgs e)
		{
			if (CheckIndex())
			{
				ReloadScenesList(StoryBoardEditor.MoveSceneUp(ScenesList.SelectedIndex));
			}
		}

		private void FirstButton_Click(object sender, RoutedEventArgs e)
		{
			if (CheckIndex())
			{
				ReloadScenesList(StoryBoardEditor.MoveSceneToFirst(ScenesList.SelectedIndex));
			}
		}

		private void EditButton_Click(object sender, RoutedEventArgs e)
		{
			EditScene();
		}

		private void EditScene()
		{
			if (CheckIndex())
			{
				StoryBoardEditor.EditScene(ScenesList.SelectedIndex, NavigationService.Navigate);
			}
		}

		private void RemoveButton_Click(object sender, RoutedEventArgs e)
		{
			if (CheckIndex())
			{
				if (System.Windows.Browser.HtmlPage.Window.Confirm("Are you sure to remove this scene? (This operation cannot be undone unless you don't save the SLO)"))
				{
					int index = StoryBoardEditor.RemoveScene(ScenesList.SelectedIndex);

					if (index < 0)
					{
						MessageBox.Show("This scene cannot be removed, provided that it is referenced by other scenes.");
					}
					else
					{
						ReloadScenesList(index);
					}
				}
			}
		}

		private void AddButton_Click(object sender, RoutedEventArgs e)
		{
			NavigationService.Navigate(Pages.CreateNewScenePage);
		}

		private void DuplicateButton_Click(object sender, RoutedEventArgs e)
		{
			if (CheckIndex())
			{
				ReloadScenesList(StoryBoardEditor.DuplicateScene(ScenesList.SelectedIndex));
			}
		}

		private void EndButton_Click(object sender, RoutedEventArgs e)
		{
			if (CheckIndex())
			{
				StoryBoardEditor.ToggleEndScene(ScenesList.SelectedIndex);

				ReloadScenesList(ScenesList.SelectedIndex);
			}
		}

		private void txtName_LostFocus(object sender, RoutedEventArgs e)
		{
			ChangeName();
		}

		private string ChangeName()
		{
			string error = StoryBoardEditor.SetSLOName(txtName.Text);

			if (error != null)
			{
				MessageBox.Show(error);

				txtName.Focus();
			}

			return error;
		}

		private void EditCharactersButton_Click(object sender, RoutedEventArgs e)
		{
			this.NavigationService.Navigate(Pages.CharactersEditorPage);
		}

		private void ScenePanel_DoubleClick(object sender, EventArgs e)
		{
			EditScene();
		}              
 	}
}
