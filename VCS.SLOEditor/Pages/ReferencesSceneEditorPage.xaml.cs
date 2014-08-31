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
	public partial class ReferencesSceneEditorPage : Page
	{
		public ReferencesSceneEditorPage()
		{
			InitializeComponent();
		}

		private void LoadData()
		{
			ReloadReferencesList();

			txtName.Text = ReferencesSceneEditor.EditingSceneName ?? "";

			Speech.Text = ReferencesSceneEditor.EditingSpeechText ?? "";

			cbGender.ItemsSource = Genders.GetValues();

			// ReloadAvatars(); -- not needed, it is called when the gender is set

			if (ReferencesSceneEditor.EditingTeacherCharacter != null)
			{
				txtTeacherName.Text = ReferencesSceneEditor.EditingTeacherCharacter.Name ?? "";

				cbGender.SelectedValue = ReferencesSceneEditor.EditingTeacherCharacter.Gender.ToString();

				cbAnimationAvatar.SelectedValue = ReferencesSceneEditor.EditingTeacherCharacter.AnimationAvatar.ToString();
			}
			else
			{
				cbGender.SelectedIndex = 0;
			}
		}

		private void ReloadReferencesList(int index = -1)
		{
			//this.DataContext = null;
			//this.DataContext = ReferencesSceneEditor.EditingScene;

			ReferencesList.ItemsSource = null;
			ReferencesList.ItemsSource = ReferencesSceneEditor.EditingReferencesList;
			
			if (index >= 0)
			{
				ReferencesList.SelectedIndex = index;

				ReferencesList.UpdateLayout();

				ReferencesList.ScrollIntoView(ReferencesList.SelectedItem);
			}
		}

		// Executes when the user navigates to this page.
		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			if (SceneEditor.CheckEditingScene(SceneType.References, this.NavigationService.Navigate))
			{
				LoadData();

				txtName.Focus();
			}
		}
		
		#region References management

		private void LastButton_Click(object sender, RoutedEventArgs e)
		{
			if (CheckIndex())
			{
				ReloadReferencesList(ReferencesSceneEditor.MoveScenePartToLast(ReferencesList.SelectedIndex));
			}
		}

		private bool CheckIndex()
		{
			bool check = (ReferencesList.SelectedIndex >= 0 && ReferencesList.SelectedIndex < ReferencesSceneEditor.EditingReferencesList.Count());

			if (!check)
			{
				MessageBox.Show("You must select a reference first");
			}

			return check;
		}

		private void DownButton_Click(object sender, RoutedEventArgs e)
		{
			if (CheckIndex())
			{
				ReloadReferencesList(ReferencesSceneEditor.MoveScenePartDown(ReferencesList.SelectedIndex));
			}
		}

		private void UpButton_Click(object sender, RoutedEventArgs e)
		{
			if (CheckIndex())
			{
				ReloadReferencesList(ReferencesSceneEditor.MoveScenePartUp(ReferencesList.SelectedIndex));
			}
		}

		private void FirstButton_Click(object sender, RoutedEventArgs e)
		{
			if (CheckIndex())
			{
				ReloadReferencesList(ReferencesSceneEditor.MoveScenePartToFirst(ReferencesList.SelectedIndex));
			}
		}

		private int editingReference;

		private void AddButton_Click(object sender, RoutedEventArgs e)
		{
			editingReference = -1;

			ReferenceEditPanel.Visibility = Visibility.Visible;
		}

		private void EditButton_Click(object sender, RoutedEventArgs e)
		{
			EditReference();
		}

		private void EditReference()
		{
			if (CheckIndex())
			{
				editingReference = ReferencesList.SelectedIndex;

				ReferencesSceneReference r = ReferencesSceneEditor.EditingReferencesList[editingReference];

				ReferenceEditPanel.Visibility = Visibility.Visible;

				ReferenceDescription.Text = r.Description;
				ReferenceUrl.Text = r.Url;
			}
		}

		private void RemoveButton_Click(object sender, RoutedEventArgs e)
		{
			if (CheckIndex())
			{
				if (System.Windows.Browser.HtmlPage.Window.Confirm("Are you sure to remove this reference? (This operation cannot be undone unless you don't save the SLO)"))
				{
					ReloadReferencesList(ReferencesSceneEditor.RemoveReferencesSceneReference(ReferencesList.SelectedIndex));
				}
			}
		}

		private void DuplicateButton_Click(object sender, RoutedEventArgs e)
		{
			if (CheckIndex())
			{
				ReloadReferencesList(ReferencesSceneEditor.DuplicateReferencesSceneReference(ReferencesList.SelectedIndex));
			}
		}

		#endregion

		#region Reference Edit Panel

		private void ReferenceEditAcceptButton_Click(object sender, RoutedEventArgs e)
		{
			string error = null;

			if (editingReference >= 0)
			{
				error = ReferencesSceneEditor.EditReference(editingReference, ReferenceDescription.Text, ReferenceUrl.Text);
			}
			else
			{
				error = ReferencesSceneEditor.CreateReference(ReferenceDescription.Text, ReferenceUrl.Text);
			}

			if (error != null)
			{
				MessageBox.Show(error);
			}
			else
			{
				CleanReferenceEditPanel();

				ReferenceEditPanel.Visibility = Visibility.Collapsed;

				ReloadReferencesList();
			}
		}

		private void CleanReferenceEditPanel()
		{
			ReferenceDescription.Text = "";
			ReferenceUrl.Text = "";
		}

		private void ReferenceEditCancelButton_Click(object sender, RoutedEventArgs e)
		{
			CleanReferenceEditPanel();

			ReferenceEditPanel.Visibility = Visibility.Collapsed;
		}

		#endregion

		#region avatar edit

		private void cbGender_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			ReloadAvatars();
		}

		private void ReloadAvatars()
		{
			string current = (cbAnimationAvatar.SelectedValue != null ? cbAnimationAvatar.SelectedValue.ToString() : "");

			string[] avatars = (cbGender.SelectedValue != null ? AnimationAvatars.GetValues(cbGender.SelectedValue.ToString()) : AnimationAvatars.GetValues());

			cbAnimationAvatar.ItemsSource = avatars;

			if (avatars.Contains(current))
			{
				cbAnimationAvatar.SelectedValue = current;
			}
			else
			{
				cbAnimationAvatar.SelectedIndex = 0;
			}
		}

		#endregion
		
		private void AcceptButton_Click(object sender, RoutedEventArgs e)
		{
			string error = ReferencesSceneEditor.AcceptEdit(txtName.Text, Speech.Text, txtTeacherName.Text, cbAnimationAvatar.SelectedValue.ToString(), cbGender.SelectedValue.ToString(), this.NavigationService.Navigate);

			if (error != null)
			{
				MessageBox.Show(error);
			}
		}

		private void CancelButton_Click(object sender, RoutedEventArgs e)
		{
			SceneEditor.CreatingScene = false;

			ReferencesSceneEditor.CancelEdit (this.NavigationService.Navigate);
		}

		private void ReferencesPanel_DoubleClick(object sender, EventArgs e)
		{
			EditReference();
		}
	}
}
