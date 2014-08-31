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
	public partial class EmotionSelectionControl : UserControl
	{
		private Button feelNowSelectedButton;
		private Button feelDuringSelectedButton;
		private Button feelExperiencedSelectedButton;

		public EmotionSelectionControl()
		{
			InitializeComponent();

			if (Config.Language == VCS.Language.Catalan)
			{
				FeelNowHappyText.Text = "Feliç";
				FeelNowNeutralText.Text = "Neutral";
				FeelNowSadText.Text = "Trist";
				FeelNowUnhappyText.Text = "Infeliç";
				FeelNowVeryHappyText.Text = "Molt feliç";

				FeelDuringBoredText.Text = "Avorrit";
				FeelDuringEnthusiasticText.Text = "Entusiasta";
				FeelDuringFineText.Text = "Bé";
				FeelDuringNeutralText.Text = "Neutral";
				FeelDuringRelaxedText.Text = "Relaxat";
				FeelDuringSleepyText.Text = "Somnolent";
				FeelDuringTiredText.Text = "Cansat";

				FeelExperiencedAngryText.Text = "Enutjat";
				FeelExperiencedAnxiousText.Text = "Ansiós";
				FeelExperiencedConfusedText.Text = "Confós";
				FeelExperiencedExcitedText.Text = "Excitat";
				FeelExperiencedInspiredText.Text = "Inspirat";
				FeelExperiencedInterestedText.Text = "Interessat";
				FeelExperiencedNeutralText.Text = "Neutral";
				FeelExperiencedPuzzledText.Text = "Perplex";
				
				ContinueButton.Content = "Continua";
			}
		}

		private void FeelNowButton_Click(object sender, RoutedEventArgs e)
		{
			if (feelNowSelectedButton != null)
			{
				feelNowSelectedButton.BorderThickness = new Thickness(0);
			}

			feelNowSelectedButton = sender as Button;

			feelNowSelectedButton.BorderThickness = new Thickness(3);

			FeelNowOtherTextBoxPanel.Visibility = (IsFeelNowOtherEmotionSelected ? Visibility.Visible : Visibility.Collapsed);
		}

		private void FeelDuringButton_Click(object sender, RoutedEventArgs e)
		{
			if (feelDuringSelectedButton != null)
			{
				feelDuringSelectedButton.BorderThickness = new Thickness(0);
			}

			feelDuringSelectedButton = sender as Button;

			feelDuringSelectedButton.BorderThickness = new Thickness(3);

			FeelDuringOtherTextBoxPanel.Visibility = (IsFeelDuringOtherEmotionSelected ? Visibility.Visible : Visibility.Collapsed);
		}

		private void FeelExperiencedButton_Click(object sender, RoutedEventArgs e)
		{
			if (feelExperiencedSelectedButton != null)
			{
				feelExperiencedSelectedButton.BorderThickness = new Thickness(0);
			}

			feelExperiencedSelectedButton = sender as Button;

			feelExperiencedSelectedButton.BorderThickness = new Thickness(3);

			FeelExperiencedOtherTextBoxPanel.Visibility = (IsFeelExperiencedOtherEmotionSelected ? Visibility.Visible : Visibility.Collapsed);
		}

		public event MultipleEmotionSelectedHandler EmotionsSelected;

		internal static string NowEmotionalContext = "EmotionNow";
		internal static string DuringSLOEmotionalContext = "DuringSLO";
		internal static string OtherExperiencedEmotionalContext = "OtherExperienced";

		private void ContinueButton_Click(object sender, RoutedEventArgs e)
		{
			if (EmotionsSelected != null)
			{
				List<EmotionalState> emotions = new List<EmotionalState>();

				if (feelNowSelectedButton == null)
				{
					feelNowSelectedButton = FeelNowNeutralButton;
				}

				emotions.Add(new EmotionalState()
									{
										Context = NowEmotionalContext,
										Emotion = (Emotion)Enum.Parse(typeof(Emotion), feelNowSelectedButton.Tag.ToString(), true),
										OtherEmotion = (IsFeelNowOtherEmotionSelected ? FeelNowOtherTextBox.Text : "")
									});

				if (feelDuringSelectedButton == null)
				{
					feelDuringSelectedButton = FeelDuringNeutralButton;
				}

				emotions.Add(new EmotionalState()
					{
						Context = DuringSLOEmotionalContext,
						Emotion = (Emotion)Enum.Parse(typeof(Emotion), feelDuringSelectedButton.Tag.ToString(), true),
						OtherEmotion = (IsFeelDuringOtherEmotionSelected ? FeelDuringOtherTextBox.Text : "")
					});

				if (feelExperiencedSelectedButton != null)
				{
					emotions.Add(new EmotionalState()
						{
							Context = OtherExperiencedEmotionalContext,
							Emotion = (Emotion)Enum.Parse(typeof(Emotion), feelExperiencedSelectedButton.Tag.ToString(), true),
							OtherEmotion = (IsFeelExperiencedOtherEmotionSelected ? FeelExperiencedOtherTextBox.Text : "")
						});
				}

				EmotionsSelected(emotions);


				//    EmotionSelected(new EmotionEventArgs () { Emotion = (Emotion)Enum.Parse(typeof(Emotion), (sender as Button).Tag.ToString(), true)});
			}
		}

		private bool IsFeelNowOtherEmotionSelected
		{
			get
			{
				return feelNowSelectedButton.Tag.ToString() == "Other";
			}
		}

		private bool IsFeelDuringOtherEmotionSelected
		{
			get
			{
				return feelDuringSelectedButton.Tag.ToString() == "Other";
			}
		}

		private bool IsFeelExperiencedOtherEmotionSelected
		{
			get
			{
				return feelExperiencedSelectedButton.Tag.ToString() == "Other";
			}
		}

		public string FeelNowQuestion
		{
			set
			{
				FeelNowTitleText.Text = value;
			}
		}

		public string FeelDuringQuestion
		{
			set
			{
				FeelDuringTitleText.Text = value;
			}
		}

		public string FeelExperiencedQuestion
		{
			set
			{
				FeelExperiencedTitleText.Text = value;
			}
		}
	}
}
