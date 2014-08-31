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
	public partial class AssessmentSceneQuestionEditorPage : Page
	{
		public AssessmentSceneQuestionEditorPage()
		{
			InitializeComponent();
		}

		private void LoadData()
		{
			QuestionSpeech.Text = AssessmentSceneQuestionEditor.EditingAssessmentSceneQuestion.QuestionSpeech ?? "";
			QuestionText.Text = AssessmentSceneQuestionEditor.EditingAssessmentSceneQuestion.QuestionText ?? "";

			ScoreText.Text = AssessmentSceneQuestionEditor.EditingAssessmentSceneQuestion.Score.ToString();

			ReloadAnswersList();

			cbGender.ItemsSource = Genders.GetValues();

			// ReloadAvatars(); -- not needed, it is called when the gender is set

			if (AssessmentSceneQuestionEditor.EditingAssessmentSceneQuestion.Evaluator != null)
			{
				txtName.Text = AssessmentSceneQuestionEditor.EditingAssessmentSceneQuestion.Evaluator.Name ?? "";

				cbGender.SelectedValue = AssessmentSceneQuestionEditor.EditingAssessmentSceneQuestion.Evaluator.Gender.ToString();

				cbAnimationAvatar.SelectedValue = AssessmentSceneQuestionEditor.EditingAssessmentSceneQuestion.Evaluator.AnimationAvatar.ToString();
			}
			else 
			{
				cbGender.SelectedIndex = 0;
			}
		}

		private void ReloadAnswersList(int index = -1)
		{
			AnswersList.ItemsSource = null;
			AnswersList.ItemsSource = AssessmentSceneQuestionEditor.EditingAnswersList;

			if (index >= 0)
			{
				AnswersList.SelectedIndex = index;

				AnswersList.UpdateLayout();

				AnswersList.ScrollIntoView(AnswersList.SelectedItem);
			}
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

		// Executes when the user navigates to this page.
		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			if (AssessmentSceneQuestionEditor.CheckEditingAssessmentSceneQuestion(this.NavigationService.Navigate))
			{
				LoadData();

				QuestionText.Focus();
			}
		}

		private void LastButton_Click(object sender, RoutedEventArgs e)
		{
			if (CheckIndex())
			{
				ReloadAnswersList(AssessmentSceneQuestionEditor.MoveScenePartToLast(AnswersList.SelectedIndex));
			}
		}

		private bool CheckIndex()
		{
			bool check = (AnswersList.SelectedIndex >= 0 && AnswersList.SelectedIndex < AssessmentSceneQuestionEditor.EditingAnswersList.Count());

			if (!check)
			{
				MessageBox.Show("You must select an answer first");
			}

			return check;
		}

		private void DownButton_Click(object sender, RoutedEventArgs e)
		{
			if (CheckIndex())
			{
				ReloadAnswersList(AssessmentSceneQuestionEditor.MoveScenePartDown(AnswersList.SelectedIndex));
			}
		}

		private void UpButton_Click(object sender, RoutedEventArgs e)
		{
			if (CheckIndex())
			{
				ReloadAnswersList(AssessmentSceneQuestionEditor.MoveScenePartUp(AnswersList.SelectedIndex));
			}
		}

		private void FirstButton_Click(object sender, RoutedEventArgs e)
		{
			if (CheckIndex())
			{
				ReloadAnswersList(AssessmentSceneQuestionEditor.MoveScenePartToFirst(AnswersList.SelectedIndex));
			}
		}

		private void AddButton_Click(object sender, RoutedEventArgs e)
		{
			editingAnswer = -1;

			AnswerEditPanel.Visibility = Visibility.Visible;

			AnswerText.Focus();
		}

		private int editingAnswer;

		private void EditButton_Click(object sender, RoutedEventArgs e)
		{
			EditAnswer();
		}

		private void EditAnswer()
		{
			if (CheckIndex())
			{
				editingAnswer = AnswersList.SelectedIndex;

				AnswerEditPanel.Visibility = Visibility.Visible;

				AnswerText.Text = AssessmentSceneQuestionEditor.EditingAnswersList[editingAnswer].AnswerText;

				AnswerText.Focus();
			}
		}

		private void RemoveButton_Click(object sender, RoutedEventArgs e)
		{
			if (CheckIndex())
			{
				if (System.Windows.Browser.HtmlPage.Window.Confirm("Are you sure to remove this scene part? (This operation cannot be undone unless you don't save the SLO)"))
				{
					ReloadAnswersList(AssessmentSceneQuestionEditor.RemoveAnswer(AnswersList.SelectedIndex));
				}
			}
		}

		private void DuplicateButton_Click(object sender, RoutedEventArgs e)
		{
			if (CheckIndex())
			{
				ReloadAnswersList(AssessmentSceneQuestionEditor.DuplicateAnswer(AnswersList.SelectedIndex));
			}
		}

		private void AcceptButton_Click(object sender, RoutedEventArgs e)
		{
			string error = AssessmentSceneQuestionEditor.AcceptEdit(QuestionSpeech.Text, QuestionText.Text, ScoreText.Text, txtName.Text, cbAnimationAvatar.SelectedValue.ToString(), cbGender.SelectedValue.ToString (), this.NavigationService.Navigate);

			if (error != null)
			{
				MessageBox.Show(error);
			}
		}

		private void CancelButton_Click(object sender, RoutedEventArgs e)
		{
			AssessmentSceneQuestionEditor.CancelEdit(this.NavigationService.Navigate);
		}

		private void AnswerEditAcceptButton_Click(object sender, RoutedEventArgs e)
		{
			if (editingAnswer >= 0)
			{
				AssessmentSceneQuestionEditor.EditAnswer(editingAnswer, AnswerText.Text);
			}
			else
			{
				AssessmentSceneQuestionEditor.CreateAnswer(AnswerText.Text);
			}

			AnswerText.Text = "";

			AnswerEditPanel.Visibility = Visibility.Collapsed;

			ReloadAnswersList();
		}

		private void AnswerEditCancelButton_Click(object sender, RoutedEventArgs e)
		{
			AnswerText.Text = "";

			AnswerEditPanel.Visibility = Visibility.Collapsed;
		}

		private void CheckButton_Click(object sender, RoutedEventArgs e)
		{
			if (CheckIndex())
			{
				AssessmentSceneQuestionEditor.CheckAnswer(AnswersList.SelectedIndex);

				ReloadAnswersList();
			}
		}

		private void cbGender_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			ReloadAvatars();
		}

		private void AnswersPanel_DoubleClick(object sender, EventArgs e)
		{
			EditAnswer();
		}
	}
}
