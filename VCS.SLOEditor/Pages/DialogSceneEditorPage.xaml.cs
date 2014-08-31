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

namespace VCS
{
	public partial class DialogSceneEditorPage : Page
	{
		public DialogSceneEditorPage()
		{
			InitializeComponent();
		}

		private void LoadData()
		{
			ReloadScenePartsList();
		}

		private void ReloadScenePartsList(int index = -1)
		{
			this.DataContext = null;
			this.DataContext = DialogSceneEditor.EditingScene;

			if (index >= 0)
			{
				ScenePartsList.SelectedIndex = index;

				ScenePartsList.UpdateLayout();

				ScenePartsList.ScrollIntoView(ScenePartsList.SelectedItem);
			}
		}

		// Executes when the user navigates to this page.
		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			if (SceneEditor.CheckEditingScene(SceneType.Dialog, this.NavigationService.Navigate))
			{
				LoadData();

				txtName.Focus();
			}
		}

		private void BackButton_Click(object sender, RoutedEventArgs e)
		{
			if (ChangeName() == null)
			{
				SceneEditor.GoBack(this.NavigationService.Navigate);
			}
		}

		private void txtName_LostFocus(object sender, RoutedEventArgs e)
		{
			ChangeName();
		}

		private string ChangeName()
		{
			string error = DialogSceneEditor.SetSceneName(txtName.Text);

			if (error != null)
			{
				MessageBox.Show(error);

				txtName.Focus();
			}

			return error;
		}

		private void LastButton_Click(object sender, RoutedEventArgs e)
		{
			if (CheckIndex())
			{
				ReloadScenePartsList(DialogSceneEditor.MoveScenePartToLast(ScenePartsList.SelectedIndex));
			}
		}

		private bool CheckIndex()
		{
			bool check = (ScenePartsList.SelectedIndex >= 0 && ScenePartsList.SelectedIndex < DialogSceneEditor.EditingScene.DialogSceneParts.Count());

			if (!check)
			{
				MessageBox.Show("You must select a dialog scene part first");
			}

			return check;
		}

		private void DownButton_Click(object sender, RoutedEventArgs e)
		{
			if (CheckIndex())
			{
				ReloadScenePartsList(DialogSceneEditor.MoveScenePartDown(ScenePartsList.SelectedIndex));
			}
		}

		private void UpButton_Click(object sender, RoutedEventArgs e)
		{
			if (CheckIndex())
			{
				ReloadScenePartsList(DialogSceneEditor.MoveScenePartUp(ScenePartsList.SelectedIndex));
			}
		}

		private void FirstButton_Click(object sender, RoutedEventArgs e)
		{
			if (CheckIndex())
			{
				ReloadScenePartsList(DialogSceneEditor.MoveScenePartToFirst(ScenePartsList.SelectedIndex));
			}
		}

		private void AddButton_Click(object sender, RoutedEventArgs e)
		{
			DialogSceneEditor.CreateDialogScenePart(this.NavigationService.Navigate);
		}

		private void EditButton_Click(object sender, RoutedEventArgs e)
		{
			EditDialogScenePart();
		}

		private void EditDialogScenePart()
		{
			if (CheckIndex())
			{
				DialogSceneEditor.EditDialogScenePart(ScenePartsList.SelectedIndex, this.NavigationService.Navigate);
			}
		}

		private void RemoveButton_Click(object sender, RoutedEventArgs e)
		{
			if (CheckIndex())
			{
				if (System.Windows.Browser.HtmlPage.Window.Confirm("Are you sure to remove this scene part? (This operation cannot be undone unless you don't save the SLO)"))
				{
					ReloadScenePartsList(DialogSceneEditor.RemoveDialogScenePart(ScenePartsList.SelectedIndex));
				}
			}
		}

		private void DuplicateButton_Click(object sender, RoutedEventArgs e)
		{
			if (CheckIndex())
			{
				ReloadScenePartsList(DialogSceneEditor.DuplicateDialogScenePart(ScenePartsList.SelectedIndex));
			}
		}

		private void DialogPartsPanel_DoubleClick(object sender, EventArgs e)
		{
			EditDialogScenePart();
		}
	}
}
