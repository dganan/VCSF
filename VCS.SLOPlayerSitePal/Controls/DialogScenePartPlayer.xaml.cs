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
using System.Windows.Media.Imaging;
using System.Runtime.InteropServices.Automation;
using System.IO;
using VCS.SpeechService;
using System.Windows.Threading;
using VCS.WavDecoder;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.BZip2;
using System.Threading;

namespace VCS
{
	public partial class DialogScenePartPlayer : ScenePartPlayer
	{
		public DialogScenePartPlayer()
		{
			InitializeComponent();

			VariableWidth = 140;
			VariableImageHeight = 193;
			VariableHeight = VariableImageHeight + 40;

			if (Config.Language == VCS.Language.Catalan)
			{
				ActivityTextBlock.Text = "Activitat:";
				QualityTextBlock.Text = "Qualitat:";
				PassivityTextBlock.Text = "Passivitat:";

				SpeechActsTextBlock.Text = "Categories:";
				KeywordsTextBlock.Text = "Paraules clau:";
			}

			// Call only for immediate learning
			CleanDeferredVisualizations();
		}

		private void CleanDeferredVisualizations()
		{
			BestCharacterPanel.Visibility = Visibility.Collapsed;
			WorstCharacterPanel.Visibility = Visibility.Collapsed;
			MarksPanel.Visibility = Visibility.Collapsed;
			IndicatorsPanel.Visibility = Visibility.Collapsed;
			SpeechActsAndKeywordsPanel.Visibility = Visibility.Collapsed;
		}

		private DispatcherTimer timer;

		public List<Character> Characters
		{
			set
			{
				CharactersList.ItemsSource = value;

				if (value.Count >= 3)
				{
					VariableWidth = 70;
					VariableImageHeight = 96;
					VariableHeight = VariableImageHeight + 40;
				}
				else
				{
					VariableWidth = 140;
					VariableImageHeight = 193;
					VariableHeight = VariableImageHeight + 40;
				}
			}
		}

		private static string latestCharacter;

		public override ScenePart ScenePart
		{
			set
			{
				DialogScenePart dsp = value as DialogScenePart;

				if (dsp != null)
				{
					DialogText.Text = dsp.Speech.RemoveHTMLTags();

					if (!dsp.Character.UseAnimatedAvatar && dsp.Character.PhotoAvatar != null)
					{
						AvatarOpenedClosed.Source = dsp.Character.PhotoAvatarImage;
						AvatarOpenedOpened.Source = dsp.Character.PhotoAvatarImage;
						AvatarClosedClosed.Source = dsp.Character.PhotoAvatarImage;
					}
					else
					{
						AvatarOpenedClosed.Source = dsp.Character.AvatarOpenedClosed;
						AvatarOpenedOpened.Source = dsp.Character.AvatarOpenedOpened;
						AvatarClosedClosed.Source = dsp.Character.AvatarClosedClosed;
					}

					NameText.Text = dsp.Character.Name;

					//if (dsp.DeferredEmotionalState != EmotionDeferred.Neutral)
					{
						Image img = new Image();

						img.Source = new BitmapImage(new Uri("/VCS.SLOToolsLib;component/images/emotions/" + dsp.DeferredEmotionalState + ".png", UriKind.Relative));

						ToolTipService.SetToolTip(img, EmotionsDeferred.Translate(dsp.DeferredEmotionalState, Config.Language));

						img.Width = 50;
						img.Height = 50;

						MarksPanel.Children.Add(img);
					}

					foreach (DialogSpecialMark specialMark in dsp.SpecialMarks)
					{
						Image img = new Image();

						img.Source = new BitmapImage(new Uri("/VCS.SLOToolsLib;component/images/marks/" + specialMark + ".png", UriKind.Relative));

						MarksPanel.Children.Add(img);
					}

					if (dsp.SpeechActs != null)
					{
						SpeechActsList.Text = String.Join(", ", dsp.SpeechActs);

						ToolTipService.SetToolTip(SpeechActsToolTip, String.Join("\n", dsp.SpeechActs));
					}

					if (dsp.Keywords != null)
					{
						KeywordsList.Text = String.Join(", ", dsp.Keywords);

						ToolTipService.SetToolTip(KeywordsToolTip, String.Join("\n", dsp.Keywords));
					}

					if (dsp.Character.Id != latestCharacter)
					{
						Thread.Sleep(500);
					}

					ActivityColor.Fill = new SolidColorBrush(dsp.Character.ActivityColor.ToColor());
					QualityColor.Fill = new SolidColorBrush(dsp.Character.QualityColor.ToColor());
					PassivityColor.Fill = new SolidColorBrush(dsp.Character.PassivityColor.ToColor());
					SocialNetworkColor.Fill = new SolidColorBrush(dsp.Character.SocialNetworkColor.ToColor());
					
					if (dsp.Character.Equals (StoryBoardPlayerPage.SLOToPlay.WorstCharacter))
					{
						WorstCharacter.Visibility = Visibility.Visible;
					}

					if (dsp.Character.Equals (StoryBoardPlayerPage.SLOToPlay.BestCharacter))
					{
						BestCharacter.Visibility = Visibility.Visible;
					}

					latestCharacter = dsp.Character.Id;

					SLOPlayer.SayText(dsp.Character, dsp.Language, dsp.Speech,

						() => { (this.Resources["FaceAnimator"] as Storyboard).Begin(); },

						() => { (this.Resources["FaceAnimator"] as Storyboard).Stop(); OnPlayEnded(); });

					//PlayProcess(dsp);
				}
			}
		}

//		int currentIteration = 0;

		//AutoResetEvent autoEvent = new AutoResetEvent(false);

		//private void PlayProcess(DialogScenePart dsp)
		//{
		//    byte[] audio = dsp.SpeechAudio;

		//    MemoryStream ms = new MemoryStream(audio);
			
		//    //MessageBox.Show(ms.Length / 1024 + " kilobytes");

		//    WavMediaStreamSource audioStream = new WavMediaStreamSource(ms);

		//    WavRiffParser parser = new WavRiffParser(ms);

		//    parser.ParseWAVEHeader();

		//    // It seems it depends on the voice used
		//    // With System.Speech, take 500
		//    // int sleep = (int)(parser.Duration / 10000d) - 500;
		//    // int sleep = (int)(parser.Duration / 10000d) - 800;
			
		//    int sleep = (int)(parser.Duration / 10000d);

		//    //if (sleep > 0)
		//    //{
		//    //    Thread.Sleep(sleep);
		//    //}

		//    //int duration = (int)Math.Round(parser.Duration / 10000000d, 0);

		//    //if (timer == null)
		//    {
		//        timer = new DispatcherTimer();
		//        //timer.Interval = new TimeSpan(0, (int)(duration / 60), (int)(duration % 60));
		//        timer.Interval = new TimeSpan(0, 0, 0, 0, sleep);

		//        timer.Tick += new EventHandler(timer_Tick);
		//    }

		//    AudioPlayer.AutoPlay = false;

		//    AudioPlayer.MediaOpened += new RoutedEventHandler((o, e) =>
		//    {
		//        // Quitamos el chasquido inicial
		//        AudioPlayer.Position = new TimeSpan(500000); 
		//        AudioPlayer.Play();
		//    });
			
		//    AudioPlayer.SetSource(audioStream);

		//    (this.Resources["FaceAnimator"] as Storyboard).Begin();

		//    timer.Start();
		//}

		//void AudioPlayer_MediaOpened(object sender, RoutedEventArgs e)
		//{
		//    throw new NotImplementedException();
		//}

		//DateTime dt;

		//void timer_Tick(object sender, EventArgs e)
		//{
		//    // end DO CLEANUP
		//    Stop();

		//    OnPlayEnded();
		//}

		public override void Stop()
		{
			SLOPlayer.StopSayText(() => { (this.Resources["FaceAnimator"] as Storyboard).Stop(); });

			//if (timer != null)
			//{
			//    (this.Resources["FaceAnimator"] as Storyboard).Stop();

			//    AudioPlayer.Stop();

			//    timer.Stop();
			//    timer = null;
			//}
		}

		public int VariableWidth { get; set; }
		public int VariableHeight { get; set; }
		public int VariableImageHeight { get; set; }
	}
}
