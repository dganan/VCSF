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
	public partial class DialogScenePartEditorPage : Page
	{
		public DialogScenePartEditorPage()
		{
			InitializeComponent();
		}

		// Executes when the user navigates to this page.
		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			if (DialogScenePartEditor.CheckEditingDialogScenePart(this.NavigationService.Navigate))
			{
				LoadData();
			}
		}

		private void AcceptButton_Click(object sender, RoutedEventArgs e)
		{
			string error = DialogScenePartEditor.AcceptEdit(txtName.Text, txtText.Text, cbLanguage.SelectedValue.ToString(), cbCharacter.SelectedItem as Character, cbEmotionalState.SelectedValue.ToString(), SelectedKeywords, SelectedSpeechActs, SelectedDialogSpecialMarks, this.NavigationService.Navigate);

			if (error != null)
			{
				MessageBox.Show(error);
			}
		}

		private void CancelButton_Click(object sender, RoutedEventArgs e)
		{
			DialogScenePartEditor.CancelEdit(this.NavigationService.Navigate);
		}

		private void LoadData()
		{
			DialogScenePartEditor.GetKeywordsList(this);

			DialogScenePartEditor.GetSpeechActsList(this);

			txtName.Text = DialogScenePartEditor.EditingDialogScenePart.Name ?? "";
			txtText.Text = DialogScenePartEditor.EditingDialogScenePart.Speech ?? "";

			cbLanguage.ItemsSource = Languages.GetValues();
			cbLanguage.SelectedValue = DialogScenePartEditor.EditingDialogScenePart.Language.ToString();

			cbCharacter.ItemsSource = StoryBoardEditor.EditingSLO.Characters;
			cbCharacter.SelectedValue = DialogScenePartEditor.EditingDialogScenePart.Character;

			cbEmotionalState.ItemsSource = EmotionsDeferred.GetValues();
			cbEmotionalState.SelectedValue = DialogScenePartEditor.EditingDialogScenePart.DeferredEmotionalState.ToString();

			smcSpecialMarks.SelectedValues = DialogScenePartEditor.EditingDialogScenePart.SpecialMarks;
		}

		public List<string> SourceKeywords
		{
			set { KeywordsList.SourceItems = value; }
		}

		public List<string> SourceSpeechActs
		{
			set { SpeechActsList.SourceItems = value; }
		}

		public List<string> SelectedKeywords
		{
			get { return KeywordsList.TargetItems; }
			set { KeywordsList.TargetItems = value; }
		}

		public List<string> SelectedSpeechActs
		{
			get { return SpeechActsList.TargetItems; }
			set { SpeechActsList.TargetItems = value; }
		}

		public List<DialogSpecialMark> SelectedDialogSpecialMarks
		{
			get { return smcSpecialMarks.SelectedValues; }
			set { smcSpecialMarks.SelectedValues = value; }
		}

		private void AddKeyword_Click(object sender, RoutedEventArgs e)
		{
			if (String.IsNullOrWhiteSpace(txtKeyword.Text))
			{
				MessageBox.Show("Please write a keyword!");
				txtKeyword.Text = "";
				txtKeyword.Focus();
			}
			else 
			{
				KeywordsList.AddItemToTarget(txtKeyword.Text);
				txtKeyword.Text = "";
			}
		}

		private void SuggestClassification_Click(object sender, RoutedEventArgs e)
		{
			LoadingPanel.IsBusy = true;

			DialogScenePartEditor.ClassifySpeechAct(this, LoadingPanel);
		}
	}
}
