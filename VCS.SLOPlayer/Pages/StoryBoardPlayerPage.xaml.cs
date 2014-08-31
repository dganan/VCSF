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
	public partial class StoryBoardPlayerPage : Page
	{
		public static SLO SLOToPlay { set; get; }
		public static bool ForceStart { set; private get; }

		public StoryBoardPlayerPage()
		{
			InitializeComponent();

			StoryBoardPlayer.SLO = SLOToPlay;
			StoryBoardPlayer.PlayEnded += new PlayEventHandler(StoryBoardPlayer_PlayEnded);

			StoryBoardPlayer.ScenePlayerChanged += new ScenePlayerChangedHandler(StoryBoardPlayer_ScenePlayerChanged);

			ProgressBar.Minimum = 0;
			ProgressBar.Maximum = SLOToPlay.Scenes.Count;

			if (ForceStart)
			{
				Play();
			}
		}

		void StoryBoardPlayer_PlayEnded()
		{
			PlayButton.Visibility = System.Windows.Visibility.Visible;
			NextButton.Visibility = System.Windows.Visibility.Collapsed;
			FastNextButton.Visibility = System.Windows.Visibility.Collapsed;
			PreviousButton.Visibility = System.Windows.Visibility.Collapsed;
			PauseButton.Visibility = System.Windows.Visibility.Collapsed;
			StopButton.Visibility = System.Windows.Visibility.Collapsed;
		}

		// Executes when the user navigates to this page.
		protected override void OnNavigatedTo(NavigationEventArgs e)
		{

		}

		private void PlayButton_Click(object sender, RoutedEventArgs e)
		{
			Play();
		}

		private void Play()
		{
			ActivityLogger.LogActivity(SLOPlayer.UserInfo.Ip, SLOPlayer.UserInfo.Name, "SLOPlayer_PlayButton");

			PlayButton.Visibility = System.Windows.Visibility.Collapsed;
			NextButton.Visibility = System.Windows.Visibility.Visible;
			FastNextButton.Visibility = System.Windows.Visibility.Visible;
			PreviousButton.Visibility = System.Windows.Visibility.Visible;
			PauseButton.Visibility = System.Windows.Visibility.Visible;
			StopButton.Visibility = System.Windows.Visibility.Visible;

			StoryBoardPlayer.Play();
		}

		private void StopButton_Click(object sender, RoutedEventArgs e)
		{
			ActivityLogger.LogActivity(SLOPlayer.UserInfo.Ip, SLOPlayer.UserInfo.Name, "SLOPlayer_StopButton");
			
			PlayButton.Visibility = System.Windows.Visibility.Visible;
			NextButton.Visibility = System.Windows.Visibility.Collapsed;
			FastNextButton.Visibility = System.Windows.Visibility.Collapsed;
			PreviousButton.Visibility = System.Windows.Visibility.Collapsed;
			PauseButton.Visibility = System.Windows.Visibility.Collapsed;
			StopButton.Visibility = System.Windows.Visibility.Collapsed;

			StoryBoardPlayer.Stop();
		}

		private void PauseButton_Click(object sender, RoutedEventArgs e)
		{
			ActivityLogger.LogActivity(SLOPlayer.UserInfo.Ip, SLOPlayer.UserInfo.Name, "SLOPlayer_PauseButton");

			PlayButton.Visibility = System.Windows.Visibility.Visible;
			NextButton.Visibility = System.Windows.Visibility.Collapsed;
			FastNextButton.Visibility = System.Windows.Visibility.Collapsed;
			PreviousButton.Visibility = System.Windows.Visibility.Collapsed;
			PauseButton.Visibility = System.Windows.Visibility.Collapsed;
			StopButton.Visibility = System.Windows.Visibility.Visible;

			StoryBoardPlayer.Pause();
		}

		private void NextButton_Click(object sender, RoutedEventArgs e)
		{
			ActivityLogger.LogActivity(SLOPlayer.UserInfo.Ip, SLOPlayer.UserInfo.Name, "SLOPlayer_NextButton");

			PlayButton.Visibility = System.Windows.Visibility.Collapsed;
			PauseButton.Visibility = System.Windows.Visibility.Visible;
			StopButton.Visibility = System.Windows.Visibility.Visible;

			StoryBoardPlayer.Next();
		}

		private void FastNextButton_Click(object sender, RoutedEventArgs e)
		{
			ActivityLogger.LogActivity(SLOPlayer.UserInfo.Ip, SLOPlayer.UserInfo.Name, "SLOPlayer_FastNextButton");

			PlayButton.Visibility = System.Windows.Visibility.Collapsed;
			PauseButton.Visibility = System.Windows.Visibility.Visible;
			StopButton.Visibility = System.Windows.Visibility.Visible;

			StoryBoardPlayer.FastNext();
		}

		private void PreviousButton_Click(object sender, RoutedEventArgs e)
		{
			ActivityLogger.LogActivity(SLOPlayer.UserInfo.Ip, SLOPlayer.UserInfo.Name, "SLOPlayer_PreviousButton");

			PlayButton.Visibility = System.Windows.Visibility.Collapsed;
			PauseButton.Visibility = System.Windows.Visibility.Visible;
			StopButton.Visibility = System.Windows.Visibility.Visible;
			
			StoryBoardPlayer.Previous();
		}

		void StoryBoardPlayer_ScenePlayerChanged(ScenePlayer sp)
		{
			if (Config.Language == VCS.Language.Catalan)
			{
				ProgressText.Text = "Escena " + sp.Scene.Order + " de " + SLOToPlay.Scenes.Count;
			}
			else 
			{
				ProgressText.Text = "Scene " + sp.Scene.Order + " of " + SLOToPlay.Scenes.Count;
			}

			ProgressBar.Value = sp.Scene.Order;

			PlayButton.IsEnabled = sp.IsPlayEnabled;
			NextButton.IsEnabled = sp.IsNextEnabled;
			FastNextButton.IsEnabled = sp.IsFastNextEnabled;
			PreviousButton.IsEnabled = sp.IsPreviousEnabled;
			PauseButton.IsEnabled = sp.IsPauseEnabled;
			StopButton.IsEnabled = sp.IsStopEnabled;
		}
	}
}
