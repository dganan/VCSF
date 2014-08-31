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
	public partial class AssessmentSceneEditorPage : Page
	{
		public AssessmentSceneEditorPage()
		{
			InitializeComponent();
		}

		private void LoadData()
		{
			cbDefaultSceneToJump.ItemsSource = StoryBoardEditor.EditingSLO.Scenes.Where(x => !x.Equals(AssessmentSceneEditor.EditingScene));

			cbSceneToJump.ItemsSource = StoryBoardEditor.EditingSLO.Scenes.Where(x => !x.Equals(AssessmentSceneEditor.EditingScene));

			ckDefaultEnd.IsChecked = AssessmentSceneEditor.EditingScene.DefaultEnd;

			tbDefaultFeedbackMessage.Text = AssessmentSceneEditor.EditingScene.DefaultFeedbackMessage ?? "";

			cbDefaultSceneToJump.SelectedValue = AssessmentSceneEditor.EditingScene.DefaultSceneToJump;

			ReloadQuestionsList();
			ReloadJumpsList();
		}

		private void ReloadQuestionsList(int index = -1)
		{
			this.DataContext = null;
			this.DataContext = AssessmentSceneEditor.EditingScene;

			if (index >= 0)
			{
				QuestionsList.SelectedIndex = index;

				QuestionsList.UpdateLayout();

				QuestionsList.ScrollIntoView(QuestionsList.SelectedItem);
			}
		}

		// Executes when the user navigates to this page.
		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			if (SceneEditor.CheckEditingScene(SceneType.Assessment, this.NavigationService.Navigate))
			{
				LoadData();

				txtName.Focus();
			}
		}

		private void BackButton_Click(object sender, RoutedEventArgs e)
		{
			if (ChangeName() == null && ChangeDefaultSceneToJump() == null)
			{
				SceneEditor.GoBack(this.NavigationService.Navigate);
			}
		}

		private void txtName_LostFocus(object sender, RoutedEventArgs e)
		{
			ChangeName();
		}

		private void LastButton_Click(object sender, RoutedEventArgs e)
		{
			if (CheckIndex())
			{
				ReloadQuestionsList(AssessmentSceneEditor.MoveScenePartToLast(QuestionsList.SelectedIndex));
			}
		}

		private bool CheckIndex()
		{
			bool check = (QuestionsList.SelectedIndex >= 0 && QuestionsList.SelectedIndex < AssessmentSceneEditor.EditingScene.Questions.Count());

			if (!check)
			{
				MessageBox.Show("You must select a Assessment scene part first");
			}

			return check;
		}

		private void DownButton_Click(object sender, RoutedEventArgs e)
		{
			if (CheckIndex())
			{
				ReloadQuestionsList(AssessmentSceneEditor.MoveScenePartDown(QuestionsList.SelectedIndex));
			}
		}

		private void UpButton_Click(object sender, RoutedEventArgs e)
		{
			if (CheckIndex())
			{
				ReloadQuestionsList(AssessmentSceneEditor.MoveScenePartUp(QuestionsList.SelectedIndex));
			}
		}

		private void FirstButton_Click(object sender, RoutedEventArgs e)
		{
			if (CheckIndex())
			{
				ReloadQuestionsList(AssessmentSceneEditor.MoveScenePartToFirst(QuestionsList.SelectedIndex));
			}
		}

		private void AddButton_Click(object sender, RoutedEventArgs e)
		{
			AssessmentSceneEditor.CreateAssessmentSceneQuestion(this.NavigationService.Navigate);
		}

		private void EditButton_Click(object sender, RoutedEventArgs e)
		{
			EditQuestion();
		}

		private void EditQuestion()
		{
			if (CheckIndex())
			{
				AssessmentSceneEditor.EditAssessmentSceneQuestion(QuestionsList.SelectedIndex, this.NavigationService.Navigate);
			}
		}

		private void RemoveButton_Click(object sender, RoutedEventArgs e)
		{
			if (CheckIndex())
			{
				if (System.Windows.Browser.HtmlPage.Window.Confirm("Are you sure to remove this scene part? (This operation cannot be undone unless you don't save the SLO)"))
				{
					ReloadQuestionsList(AssessmentSceneEditor.RemoveAssessmentSceneQuestion(QuestionsList.SelectedIndex));
				}
			}
		}

		private void DuplicateButton_Click(object sender, RoutedEventArgs e)
		{
			if (CheckIndex())
			{
				ReloadQuestionsList(AssessmentSceneEditor.DuplicateAssessmentSceneQuestion(QuestionsList.SelectedIndex));
			}
		}

		private string ChangeName()
		{
			string error = AssessmentSceneEditor.SetSceneName(txtName.Text);

			if (error != null)
			{
				MessageBox.Show(error);

				txtName.Focus();
			}

			return error;
		}

		#region Default scene to jump

		private string ChangeDefaultSceneToJump()
		{
			string error = AssessmentSceneEditor.SetDefaultSceneToJump(cbDefaultSceneToJump.SelectedValue, ckDefaultEnd.IsChecked.Value, tbDefaultFeedbackMessage.Text);

			if (error != null)
			{
				MessageBox.Show(error);

				cbDefaultSceneToJump.Focus();
			}

			return error;
		}


		private void cbDefaultSceneToJump_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			ChangeDefaultSceneToJump();
		}

		private void ckDefaultEnd_Checked(object sender, RoutedEventArgs e)
		{
			cbDefaultSceneToJump.IsEnabled = !ckDefaultEnd.IsChecked.Value;
			ChangeDefaultSceneToJump();
		}
		
		private void ckDefaultEnd_Unchecked(object sender, RoutedEventArgs e)
		{
			cbDefaultSceneToJump.IsEnabled = !ckDefaultEnd.IsChecked.Value;
			ChangeDefaultSceneToJump();
		}

		private void tbDefaultFeedbackMessage_TextChanged(object sender, TextChangedEventArgs e)
		{
			ChangeDefaultSceneToJump();
		}

		#endregion

		#region JumpsList

		private void ReloadJumpsList(int index = -1)
		{
			this.DataContext = null;
			this.DataContext = AssessmentSceneEditor.EditingScene;

			if (index >= 0)
			{
				JumpsList.SelectedIndex = index;

				JumpsList.UpdateLayout();

				JumpsList.ScrollIntoView(QuestionsList.SelectedItem);
			}
		}

		private void JumpsListLastButton_Click(object sender, RoutedEventArgs e)
		{
			if (JumpsListCheckIndex())
			{
				ReloadJumpsList(AssessmentSceneEditor.MoveJumpRuleToLast(JumpsList.SelectedIndex));
			}
		}

		private bool JumpsListCheckIndex()
		{
			bool check = (JumpsList.SelectedIndex >= 0 && JumpsList.SelectedIndex < AssessmentSceneEditor.EditingScene.JumpRules.Count());

			if (!check)
			{
				MessageBox.Show("You must select a jump rule first");
			}

			return check;
		}

		private void JumpsListDownButton_Click(object sender, RoutedEventArgs e)
		{
			if (JumpsListCheckIndex())
			{
				ReloadJumpsList(AssessmentSceneEditor.MoveJumpRuleDown(JumpsList.SelectedIndex));
			}
		}

		private void JumpsListUpButton_Click(object sender, RoutedEventArgs e)
		{
			if (JumpsListCheckIndex())
			{
				ReloadJumpsList(AssessmentSceneEditor.MoveJumpRuleUp(JumpsList.SelectedIndex));
			}
		}

		private void JumpsListFirstButton_Click(object sender, RoutedEventArgs e)
		{
			if (JumpsListCheckIndex())
			{
				ReloadJumpsList(AssessmentSceneEditor.MoveJumpRuleToFirst(JumpsList.SelectedIndex));
			}
		}

		private void JumpsListAddButton_Click(object sender, RoutedEventArgs e)
		{
			editingJumpRule = -1;

			JumpRuleEditPanel.Visibility = Visibility.Visible;
		}

		private int editingJumpRule;

		private void JumpsListEditButton_Click(object sender, RoutedEventArgs e)
		{
			EditJump();
		}

		private void EditJump()
		{
			if (JumpsListCheckIndex())
			{
				editingJumpRule = JumpsList.SelectedIndex;

				AssessmentJumpRule jr = AssessmentSceneEditor.EditingScene.JumpRules[editingJumpRule];

				JumpRuleEditPanel.Visibility = Visibility.Visible;

				cbActivateTestResult.IsChecked = jr.CheckTestResult;

				if (jr.CheckTestResult)
				{
					TestResultMinText.Text = jr.MinResult.ToString();
					TestResultMinIncluded.IsChecked = jr.MinResultIncluded;
					TestResultMaxText.Text = jr.MaxResult.ToString();
					TestResultMaxIncluded.IsChecked = jr.MaxResultIncluded;
				}

				cbActivateTestAttempts.IsChecked = jr.CheckNumberOfIterations;

				if (jr.CheckNumberOfIterations)
				{
					TestAttemptsMinText.Text = jr.MinNumberOfIterations.ToString();

					if (jr.MaxNumberOfIterations < Int32.MaxValue)
					{
						TestAttemptsMaxText.Text = jr.MaxNumberOfIterations.ToString();
					}
				}

				cbActivatePrerequisites.IsChecked = jr.CheckPrerequisites;

				if (jr.CheckPrerequisites)
				{
					Prerequisites.IsChecked = jr.MeetsPrerequisites;
				}

				ckEnd.IsChecked = jr.End;

				if (!jr.End)
				{
					cbSceneToJump.SelectedValue = jr.SceneToJump;
				}

				if (jr.FeedbackMessage != null)
				{
					tbFeedbackMessage.Text = jr.FeedbackMessage;
				}
			}
		}

		private void JumpsListRemoveButton_Click(object sender, RoutedEventArgs e)
		{
			if (JumpsListCheckIndex())
			{
				if (System.Windows.Browser.HtmlPage.Window.Confirm("Are you sure to remove this jump rule? (This operation cannot be undone unless you don't save the SLO)"))
				{
					ReloadJumpsList(AssessmentSceneEditor.RemoveJumpRule(JumpsList.SelectedIndex));
				}
			}
		}

		private void JumpsListDuplicateButton_Click(object sender, RoutedEventArgs e)
		{
			if (JumpsListCheckIndex())
			{
				ReloadJumpsList(AssessmentSceneEditor.DuplicateJumpRule(JumpsList.SelectedIndex));
			}
		}

		private void ckEnd_Checked(object sender, RoutedEventArgs e)
		{
			cbSceneToJump.IsEnabled = !ckEnd.IsChecked.Value;
		}

		private void ckEnd_Unchecked(object sender, RoutedEventArgs e)
		{
			cbSceneToJump.IsEnabled = !ckEnd.IsChecked.Value;
		}

		private void JumpRuleEditAcceptButton_Click(object sender, RoutedEventArgs e)
		{
			string error = null;

			if (editingJumpRule >= 0)
			{
				error = AssessmentSceneEditor.EditJumpRule(editingJumpRule, cbActivateTestResult.IsChecked.Value, TestResultMinText.Text, TestResultMinIncluded.IsChecked.Value, TestResultMaxText.Text, TestResultMaxIncluded.IsChecked.Value, cbActivateTestAttempts.IsChecked.Value, TestAttemptsMinText.Text, TestAttemptsMaxText.Text, cbActivatePrerequisites.IsChecked.Value, Prerequisites.IsChecked.Value, cbSceneToJump.SelectedValue, ckEnd.IsChecked.Value, tbFeedbackMessage.Text);
			}
			else
			{
				error = AssessmentSceneEditor.CreateJumpRule(cbActivateTestResult.IsChecked.Value, TestResultMinText.Text, TestResultMinIncluded.IsChecked.Value, TestResultMaxText.Text, TestResultMaxIncluded.IsChecked.Value, cbActivateTestAttempts.IsChecked.Value, TestAttemptsMinText.Text, TestAttemptsMaxText.Text, cbActivatePrerequisites.IsChecked.Value, Prerequisites.IsChecked.Value, cbSceneToJump.SelectedValue, ckEnd.IsChecked.Value, tbFeedbackMessage.Text);
			}

			if (error != null)
			{
				MessageBox.Show(error);
			}
			else
			{
				CleanJumpRuleEditPanel();

				JumpRuleEditPanel.Visibility = Visibility.Collapsed;

				ReloadJumpsList();
			}
		}

		private void CleanJumpRuleEditPanel()
		{
			cbActivateTestResult.IsChecked = true;
			TestResultMinText.Text = "";
			TestResultMinIncluded.IsChecked = true;
			TestResultMaxText.Text = "";
			TestResultMaxIncluded.IsChecked = true;

			cbActivateTestAttempts.IsChecked = true;
			TestAttemptsMinText.Text = "";
			TestAttemptsMaxText.Text = "";

			cbActivatePrerequisites.IsChecked = true;
			Prerequisites.IsChecked = false;
			
			cbSceneToJump.SelectedValue = null;
			ckEnd.IsChecked = false;
			tbFeedbackMessage.Text = "";
		}

		private void JumpRuleEditCancelButton_Click(object sender, RoutedEventArgs e)
		{
			CleanJumpRuleEditPanel();

			JumpRuleEditPanel.Visibility = Visibility.Collapsed;
		}

		private void cbActivateTestResult_Checked(object sender, RoutedEventArgs e)
		{
			if (cbActivateTestResult != null)
			{
				TestResultMinText.IsEnabled = cbActivateTestResult.IsChecked.Value;
				TestResultMinIncluded.IsEnabled = cbActivateTestResult.IsChecked.Value;
				TestResultMaxText.IsEnabled = cbActivateTestResult.IsChecked.Value;
				TestResultMinIncluded.IsEnabled = cbActivateTestResult.IsChecked.Value;
			}
		}

		private void cbActivateTestResult_Unchecked(object sender, RoutedEventArgs e)
		{
			if (cbActivateTestResult != null)
			{
				TestResultMinText.IsEnabled = cbActivateTestResult.IsChecked.Value;
				TestResultMinIncluded.IsEnabled = cbActivateTestResult.IsChecked.Value;
				TestResultMaxText.IsEnabled = cbActivateTestResult.IsChecked.Value;
				TestResultMaxIncluded.IsEnabled = cbActivateTestResult.IsChecked.Value;
			}
		}

		private void cbActivateTestAttempts_Checked(object sender, RoutedEventArgs e)
		{
			if (cbActivateTestAttempts != null)
			{
				TestAttemptsMinText.IsEnabled = cbActivateTestAttempts.IsChecked.Value;
				TestAttemptsMaxText.IsEnabled = cbActivateTestAttempts.IsChecked.Value;
			}
		}

		private void cbActivateTestAttempts_Unchecked(object sender, RoutedEventArgs e)
		{
			if (cbActivateTestAttempts != null)
			{
				TestAttemptsMinText.IsEnabled = cbActivateTestAttempts.IsChecked.Value;
				TestAttemptsMaxText.IsEnabled = cbActivateTestAttempts.IsChecked.Value;
			}
		}

		private void cbActivatePrerequisites_Checked(object sender, RoutedEventArgs e)
		{
			if (cbActivatePrerequisites != null)
			{
				Prerequisites.IsEnabled = cbActivatePrerequisites.IsChecked.Value;
			}
		}

		private void cbActivatePrerequisites_Unchecked(object sender, RoutedEventArgs e)
		{
			if (cbActivatePrerequisites != null)
			{
				Prerequisites.IsEnabled = cbActivatePrerequisites.IsChecked.Value;
			}
		}

		#endregion

		private void QuestionsPanel_DoubleClick(object sender, EventArgs e)
		{
			EditQuestion();
		}

		private void JumpsPanel_DoubleClick(object sender, EventArgs e)
		{
			EditJump();
		}
	}
}
