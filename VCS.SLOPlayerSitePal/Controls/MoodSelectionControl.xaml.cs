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

namespace VCS
{
	public partial class MoodSelectionControl : UserControl
	{
		internal static string NowMoodContext = "MoodNow";

		private Button moodSelectedButton;

		public MoodSelectionControl()
		{
			InitializeComponent();

			if (Config.Language == VCS.Language.Catalan)
			{
				SadText.Text = "Trist";
				UnhappyText.Text = "Infeliç";
				BoredText.Text = "Avorrit";
				SleepyText.Text = "Somnolent";
				NeutralText.Text = "Neutral";
				FineText.Text = "Bé";
				HappyText.Text = "Feliç";
				VeryHappyText.Text = "Molt feliç";
				OtherText.Text = "Altre";
				ContinueButton.Content = "Continua";
			}
		}

		private void MoodButton_Click(object sender, RoutedEventArgs e)
		{
			if (moodSelectedButton != null)
			{
				moodSelectedButton.BorderThickness = new Thickness(0);
			}

			moodSelectedButton = sender as Button;

			moodSelectedButton.BorderThickness = new Thickness(3);

			OtherTextBoxPanel.Visibility = (IsOtherEmotionSelected ? Visibility.Visible : Visibility.Collapsed);
		}

		private bool IsOtherEmotionSelected
		{
			get
			{
				return moodSelectedButton.Tag.ToString() == "Other";
			}
		}
		
		private void ContinueButton_Click(object sender, RoutedEventArgs e)
		{
			if (EmotionSelected != null)
			{
				if (moodSelectedButton == null)
				{
					moodSelectedButton = NeutralButton;
				}

				EmotionSelected(new EmotionalState() { Context = NowMoodContext, Emotion = (Emotion)Enum.Parse(typeof(Emotion), moodSelectedButton.Tag.ToString(), true), OtherEmotion = (IsOtherEmotionSelected ? OtherTextBox.Text : "") });
			}
		}

		public event EmotionSelectedHandler EmotionSelected;

		public string MoodQuestion
		{
			set 
			{
				TitleText.Text = value;
			}
		}
	}
}
