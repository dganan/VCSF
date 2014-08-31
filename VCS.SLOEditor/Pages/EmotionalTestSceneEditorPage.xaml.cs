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
	public partial class EmotionalTestSceneEditorPage : Page
	{
		public EmotionalTestSceneEditorPage()
		{
			InitializeComponent();
		}

		private void LoadData()
		{
			this.DataContext = null;
			this.DataContext = EmotionalTestSceneEditor.EditingScene;

			SetPropertiesPanelsVisibility();

			MoodMeterRadioButton.IsChecked = EmotionalTestSceneEditor.EditingScene.EmotionalTestType == EmotionalTestType.Mood;
			EmotionMeterRadioButton.IsChecked = EmotionalTestSceneEditor.EditingScene.EmotionalTestType == EmotionalTestType.Emotional;
			GenevaWheelRadioButton.IsChecked = EmotionalTestSceneEditor.EditingScene.EmotionalTestType == EmotionalTestType.Geneva_Wheel;

			MoodQuestionTextBox.Text = EmotionalTestSceneEditor.EditingScene.MoodQuestion ?? "";
			EmotionFeelNowQuestionTextBox.Text = EmotionalTestSceneEditor.EditingScene.EmotionFeelNowQuestion ?? "";
			EmotionFeelDuringQuestionTextBox.Text = EmotionalTestSceneEditor.EditingScene.EmotionFeelDuringQuestion ?? "";
			EmotionFeelExperiencedQuestionTextBox.Text = EmotionalTestSceneEditor.EditingScene.EmotionFeelExperiencedQuestion ?? "";
			GenevaWheelQuestionTextBox.Text = EmotionalTestSceneEditor.EditingScene.GenevaWheelQuestion ?? "";
		}

		private void SetPropertiesPanelsVisibility()
		{
			MoodMeterPropertiesPanel.Visibility = EmotionalTestSceneEditor.EditingScene.EmotionalTestType == EmotionalTestType.Mood ? Visibility.Visible : Visibility.Collapsed;
			EmotionMeterPropertiesPanel.Visibility = EmotionalTestSceneEditor.EditingScene.EmotionalTestType == EmotionalTestType.Emotional ? Visibility.Visible : Visibility.Collapsed;
			GenevaWheelPropertiesPanel.Visibility = EmotionalTestSceneEditor.EditingScene.EmotionalTestType == EmotionalTestType.Geneva_Wheel ? Visibility.Visible : Visibility.Collapsed;
		}

		// Executes when the user navigates to this page.
		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			if (SceneEditor.CheckEditingScene(SceneType.EmotionalTest, this.NavigationService.Navigate))
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
			string error = EmotionalTestSceneEditor.SetSceneName(txtName.Text);

			if (error != null)
			{
				MessageBox.Show(error);

				txtName.Focus();
			}

			return error;
		}

		private void RadioButton_Checked(object sender, RoutedEventArgs e)
		{
			RadioButton rb = sender as RadioButton;

			if (rb.Tag.ToString() == "Mood")
			{
				EmotionalTestSceneEditor.EditingScene.EmotionalTestType = EmotionalTestType.Mood;
			}
			else if (rb.Tag.ToString() == "Emotion")
			{
				EmotionalTestSceneEditor.EditingScene.EmotionalTestType = EmotionalTestType.Emotional;
			}
			else if (rb.Tag.ToString() == "Geneva")
			{
				EmotionalTestSceneEditor.EditingScene.EmotionalTestType = EmotionalTestType.Geneva_Wheel;
			}

			SetPropertiesPanelsVisibility();
		}

		private void MoodQuestionTextBox_LostFocus(object sender, RoutedEventArgs e)
		{
			EmotionalTestSceneEditor.EditingScene.MoodQuestion = MoodQuestionTextBox.Text;
		}

		private void EmotionFeelNowQuestionTextBox_LostFocus(object sender, RoutedEventArgs e)
		{
			EmotionalTestSceneEditor.EditingScene.EmotionFeelNowQuestion = EmotionFeelNowQuestionTextBox.Text;
		}

		private void EmotionFeelDuringQuestionTextBox_LostFocus(object sender, RoutedEventArgs e)
		{
			EmotionalTestSceneEditor.EditingScene.EmotionFeelDuringQuestion = EmotionFeelDuringQuestionTextBox.Text;
		}

		private void EmotionFeelExperiencedQuestionTextBox_LostFocus(object sender, RoutedEventArgs e)
		{
			EmotionalTestSceneEditor.EditingScene.EmotionFeelExperiencedQuestion = EmotionFeelExperiencedQuestionTextBox.Text;
		}

		private void GenevaWheelQuestionTextBox_LostFocus(object sender, RoutedEventArgs e)
		{
			EmotionalTestSceneEditor.EditingScene.GenevaWheelQuestion = GenevaWheelQuestionTextBox.Text;
		}

		//private void AcceptButton_Click(object sender, RoutedEventArgs e)
		//{
		//    string error = EmotionalTestSceneEditor.AcceptEdit(QuestionText.Text, this.NavigationService.Navigate);

		//    if (error != null)
		//    {
		//        MessageBox.Show(error);
		//    }
		//}

		//private void CancelButton_Click(object sender, RoutedEventArgs e)
		//{
		//    EmotionalTestSceneEditor.CancelEdit(this.NavigationService.Navigate);
		//}
	}
}
