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
	public partial class CharactersEditorPage : Page
	{
		public CharactersEditorPage()
		{
			InitializeComponent();
		}

		// Executes when the user navigates to this page.
		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			LoadData();
		}

		private void LoadData()
		{
			ReloadCharactersList();
		}

		private void ReloadCharactersList(int index = -1)
		{
			StoryBoardEditor.EditingSLO.OrderCharacters();

			this.DataContext = null;
			this.DataContext = StoryBoardEditor.EditingSLO;

			if (index >= 0)
			{
				CharactersList.SelectedIndex = index;

				CharactersList.ScrollIntoView(CharactersList.SelectedItem);
			}
		}

		private void BackButton_Click(object sender, RoutedEventArgs e)
		{
			this.NavigationService.Navigate(Pages.DialogSceneEditorPage);
		}

		private bool CheckIndex()
		{
			bool check = (CharactersList.SelectedIndex >= 0 && CharactersList.SelectedIndex < StoryBoardEditor.EditingSLO.Characters.Count());

			if (!check)
			{
				MessageBox.Show("You must select a character first");
			}

			return check;
		}

		private void AddButton_Click(object sender, RoutedEventArgs e)
		{
			StoryBoardEditor.CreateCharacter(this.NavigationService.Navigate);
		}

		private void EditButton_Click(object sender, RoutedEventArgs e)
		{
			EditCharacter();
		}

		private void EditCharacter()
		{
			if (CheckIndex())
			{
				StoryBoardEditor.EditCharacter(CharactersList.SelectedIndex, this.NavigationService.Navigate);
			}
		}

		private void RemoveButton_Click(object sender, RoutedEventArgs e)
		{
			if (CheckIndex())
			{
				if (System.Windows.Browser.HtmlPage.Window.Confirm("Are you sure to remove this character? (This operation cannot be undone unless you don't save the SLO)"))
				{
					int index = StoryBoardEditor.RemoveCharacter(CharactersList.SelectedIndex);

					if (index < 0)
					{
						MessageBox.Show("This character cannot be removed, provided that it is involved in some dialog scene parts.");
					}
					else
					{
						ReloadCharactersList(index);
					}
				}
			}
		}

		private void DuplicateButton_Click(object sender, RoutedEventArgs e)
		{
			if (CheckIndex())
			{
				ReloadCharactersList(StoryBoardEditor.DuplicateCharacter(CharactersList.SelectedIndex));
			}
		}

		private void CharactersPanel_DoubleClick(object sender, EventArgs e)
		{
			EditCharacter();
		}
	}
}
