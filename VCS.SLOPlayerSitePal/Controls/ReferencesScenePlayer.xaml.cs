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
using System.Threading;
using System.Windows.Threading;
using VCS.SpeechService;
using System.IO;
using VCS.WavDecoder;
using System.Windows.Media.Imaging;

namespace VCS
{
	public partial class ReferencesScenePlayer : ScenePlayer
	{
		private ReferencesScene ReferencesScene
		{
			get
			{
				return Scene as ReferencesScene;
			}
		}

		private DispatcherTimer timer;

		public ReferencesScenePlayer()
		{
			InitializeComponent();

			VariableWidth = 140;
			VariableImageHeight = 193;
			VariableHeight = VariableImageHeight + 40;

			if (Config.Language == VCS.Language.Catalan)
			{
				ContinueButton.Content = "Continuar";
			}
		}

		public override void Play()
		{
			SetupCurrentQuestion();
		}

		//public override void Stop()
		//{
		//    if (currentReferencesSceneReferencePlayer != null)
		//    {
		//        playing = false;

		//        currentReferencesSceneReferencePlayer.Stop();

		//        currentQuestion = -1;
		//    }
		//}

		//public override void Pause()
		//{
		//    if (currentReferencesSceneReferencePlayer != null)
		//    {
		//        playing = false;

		//        // It stops the current scenepart. If play is clicked again, the scenepart will be rendered from the begining.
		//        currentReferencesSceneReferencePlayer.Stop();
		//    }
		//}

		public override string Title
		{
			set
			{
				TitleText.Text = value;
			}
		}

		public override string SubTitle
		{
			set
			{
				SubTitleText.Text = value;
			}
		}

		public override Scene Scene
		{
			get
			{
				return base.Scene;
			}

			set
			{
				base.Scene = value;

				new Thread(new ThreadStart(SpeechProcess)).Start();
			}
		}

		void SetupCurrentQuestion()
		{
			ActivityLogger.LogActivity(SLOPlayer.UserInfo.Ip, SLOPlayer.UserInfo.Name, String.Format("References Scene. Previes: {0}, scene order: {1}", SLOPlayer.UserInfo.MeetsPrerequisites, ReferencesScene.Order));

			if (!this.Dispatcher.CheckAccess())
			{
				this.Dispatcher.BeginInvoke(new Action(
					() =>
					{
						SetupCurrentQuestion();
					}));
			}
			else
			{
				LayoutRoot.Children.Clear();

				if (ReferencesScene.SpeechAudio == null)
				{
					timer = new DispatcherTimer();
					//timer.Interval = new TimeSpan(0, (int)(duration / 60), (int)(duration % 60));
					timer.Interval = new TimeSpan(0, 0, 0, 0, 1000);

					timer.Tick += new EventHandler(timer_Tick);

					timer.Start();
				}
				else
				{
					// SETUP SCENE

					SetupScene();
				}
			}
		}

		void timer_Tick(object sender, EventArgs e)
		{
			new Thread(new ThreadStart(SetupCurrentQuestion)).Start();

			timer.Stop();
		}

		public struct SpeechParam
		{
			public int scene;
			public bool loop;
		}

		private object lockObj = new object();

		private void SpeechProcess()
		{
			if (ReferencesScene.SpeechAudio == null)
			{
				string textToSpeech = ReferencesScene.Speech;

				textToSpeech = textToSpeech.RemoveHTMLTags();

				SpeechServiceClient client = SLOPlayer.SpeechServiceClient;

				client.SpeakCompleted += (o, ea) =>
				{
					ReferencesScene.SpeechAudio = Utils.DecompressBytes(ea.Result);
				};

				client.SpeakAsync(textToSpeech, ReferencesScene.Character.Gender, ReferencesScene.Character.Age, VCS.Language.Unknown);
			}
		}

		public override bool IsFastNextEnabled { get { return false; } }

		public override bool IsNextEnabled { get { return false; } }

		public override bool IsPreviousEnabled { get { return false; } }

		private void ContinueButton_Click(object sender, RoutedEventArgs e)
		{
			(this.Resources["FaceAnimator"] as Storyboard).Stop();

			OnPlayNext();
		}

		private void SetupScene ()
		{
			DialogText.Text = ReferencesScene.Speech;

			foreach (ReferencesSceneReference r in ReferencesScene.References)
			{
				DockPanel dp = new DockPanel();

				dp.Margin = new Thickness(0, 0, 0, 5);
				dp.LastChildFill = true;
				
				TextBlock tb = new TextBlock ();

				tb.Text = r.Description;
				tb.TextWrapping = TextWrapping.Wrap;
				tb.Height = 30;

				Button b = new Button();

				b.Click += new RoutedEventHandler(GoButton_Click);
				b.Tag = r.Url;

				Image img = new Image();
				img.Source = new BitmapImage(new Uri("/VCS.SLOPlayerSitePal;component/images/go.png", UriKind.Relative));

				img.Width = 32;
				img.Height = 32;

				b.Content = img;
				
				DockPanel.SetDock(b, Dock.Right);

				dp.Children.Add(b);
				dp.Children.Add(tb);

				ReferencesPanel.Children.Add(dp);
			}

			ReferencesPanel.InvalidateMeasure();
			ReferencesPanel.UpdateLayout();

			if (!ReferencesScene.Character.UseAnimatedAvatar && ReferencesScene.Character.PhotoAvatar != null)
			{
				AvatarOpenedClosed.Source = ReferencesScene.Character.PhotoAvatarImage;
				AvatarOpenedOpened.Source = ReferencesScene.Character.PhotoAvatarImage;
				AvatarClosedClosed.Source = ReferencesScene.Character.PhotoAvatarImage;
			}
			else
			{
				AvatarOpenedClosed.Source = ReferencesScene.Character.AvatarOpenedClosed;
				AvatarOpenedOpened.Source = ReferencesScene.Character.AvatarOpenedOpened;
				AvatarClosedClosed.Source = ReferencesScene.Character.AvatarClosedClosed;
			}

			NameText.Text = ReferencesScene.Character.Name;

			PlayProcess(ReferencesScene);
		}

		void GoButton_Click(object sender, RoutedEventArgs e)
		{
			Button button = sender as Button;

			if (button != null)
			{
				string url = (string)button.Tag;

				System.Windows.Browser.HtmlPage.Window.Navigate(new Uri(url, UriKind.Absolute), "_blank");
			}
		}

		//		int currentIteration = 0;

		AutoResetEvent autoEvent = new AutoResetEvent(false);

		private void PlayProcess(ReferencesScene r)
		{
			byte[] audio = r.SpeechAudio;

			MemoryStream ms = new MemoryStream(audio);

			//MessageBox.Show(ms.Length / 1024 + " kilobytes");

			WavMediaStreamSource audioStream = new WavMediaStreamSource(ms);

			WavRiffParser parser = new WavRiffParser(ms);

			parser.ParseWAVEHeader();

			// It seems it depends on the voice used
			// With System.Speech, take 500
			// int sleep = (int)(parser.Duration / 10000d) - 500;
			//int sleep = (int)(parser.Duration / 10000d) - 800;
			int sleep = (int)(parser.Duration / 10000d);

			//if (sleep > 0)
			//{
			//    Thread.Sleep(sleep);
			//}

			//int duration = (int)Math.Round(parser.Duration / 10000000d, 0);

			//if (timer == null)
			{
				timer = new DispatcherTimer();
				//timer.Interval = new TimeSpan(0, (int)(duration / 60), (int)(duration % 60));
				timer.Interval = new TimeSpan(0, 0, 0, 0, sleep);

				timer.Tick += new EventHandler((o, ea) => { Stop(); });
			}

			AudioPlayer.AutoPlay = false;

			AudioPlayer.MediaOpened += new RoutedEventHandler((o, e) =>
			{
				// Quitamos el chasquido inicial
				AudioPlayer.Position = new TimeSpan(500000);
				AudioPlayer.Play();
			});

			AudioPlayer.SetSource(audioStream);

			(this.Resources["FaceAnimator"] as Storyboard).Begin();

			timer.Start();
		}

		DateTime dt;

		public override void Stop()
		{
			if (timer != null)
			{
				// This is for the avatar to continue opening / closing eyes, but not the mouth
				AvatarOpenedOpened.Source = ReferencesScene.Character.AvatarOpenedClosed;

				AudioPlayer.Stop();

				timer.Stop();
				timer = null;
			}
		}

		public int VariableWidth { get; set; }
		public int VariableHeight { get; set; }
		public int VariableImageHeight { get; set; }
	}
}
