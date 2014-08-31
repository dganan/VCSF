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
using ICSharpCode.SharpZipLib.BZip2;
using System.Text.RegularExpressions;

namespace VCS
{
	public partial class DialogScenePlayer : ScenePlayer
	{
		private bool playing;

		public DialogScenePlayer()
		{
			InitializeComponent();

			NavigationList.NavigateToScene += new NavigateToSceneHandler(NavigationList_NavigateToScene);

			currentScene = -1;
		}

		void NavigationList_NavigateToScene(Scene scene)
		{
			if (playing && currentDialogScenePartPlayer != null)
			{
				playing = false;

				currentDialogScenePartPlayer.Stop();
			
				OnJumpToScene(scene);
			}
		}

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

				currentScene = 0;

				nextSceneToLoad = 0;

				new Thread(new ThreadStart(SpeechProcess)).Start();
			}
		}

		public List<DialogScene> Contributions
		{
			set 
			{
				SetupNavigationList(value);
			}
		}

		private DialogScene DialogScene
		{
			get
			{
				return Scene as DialogScene;
			}
		}

		private int currentScene;

		//private DispatcherTimer timer;

		public override void Play()
		{
			//if (timer == null)
			//{
			//    timer = new DispatcherTimer();
			//    timer.Interval = new TimeSpan(0, 0, 6);

			//    timer.Tick += new EventHandler(timer_Tick);
			//}

			SetupCurrentScenePart();

			//timer.Start();
		}

		DialogScenePartPlayer currentDialogScenePartPlayer;

		//void timer_Tick(object sender, EventArgs e)

		void currentDialogScenePartPlayer_PlayEnded()
		{
			playing = false;

			currentScene++;

			SetupCurrentScenePart();
		}

		public override void Stop()
		{
			if (currentDialogScenePartPlayer != null)
			{
				playing = false;

				currentDialogScenePartPlayer.Stop();

				currentScene = -1;
				//OnPlayEnded();
			}
		}

		public override void Pause()
		{
			if (currentDialogScenePartPlayer != null)
			{
				playing = false;

				// It stops the current scenepart. If play is clicked again, the scenepart will be rendered from the begining.
				currentDialogScenePartPlayer.Stop();
			}
		}

		public override void Next()
		{
			if (playing && currentDialogScenePartPlayer != null)
			{
				playing = false;

				currentDialogScenePartPlayer.Stop();

				currentScene++;

				SetupCurrentScenePart();
			}
		}

		public override void FastNext()
		{
			if (playing && currentDialogScenePartPlayer != null)
			{
				playing = false;

				int count = (Scene as DialogScene).DialogSceneParts.Count;

				currentDialogScenePartPlayer.Stop();

				// Force to end the current scene
				currentScene = count;
				
				SetupCurrentScenePart();
				

				//string currentCharacter = (Scene as DialogScene).DialogSceneParts[currentScene].Character.Id;

				//int count = (Scene as DialogScene).DialogSceneParts.Count;

				//bool same = true;

				//int nextScene = currentScene;

				//while (nextScene < count - 1 && same)
				//{
				//    nextScene++;

				//    same = currentCharacter == (Scene as DialogScene).DialogSceneParts[nextScene].Character.Id;
				//}

				////if (nextScene < count - 1)
				//{
				//    currentDialogScenePartPlayer.Stop();

				//    currentScene = nextScene;

				//    SetupCurrentScenePart();
				//}
				////else
				////{
				////    playing = true;
				////}
			}
		}

		public override void Previous()
		{
			if (playing && currentDialogScenePartPlayer != null)
			{
				playing = false;

				currentDialogScenePartPlayer.Stop();

				currentScene--;

				SetupCurrentScenePart();
			}
		}

		private void GotoScene(int scene)
		{
			if (playing && currentDialogScenePartPlayer != null)
			{
				playing = false;

				currentDialogScenePartPlayer.Stop();

				currentScene = scene;

				SetupCurrentScenePart();
			}
		}

		void SetupCurrentScenePart()
		{
			if (!this.Dispatcher.CheckAccess())
			{
				this.Dispatcher.BeginInvoke(new Action(
					() =>
					{
						SetupCurrentScenePart();
					}));
			}
			else
			{
				LayoutRoot.Children.Clear();

				if (currentScene >= 0 && currentScene < DialogScene.DialogSceneParts.Count)
				{
					if (DialogScene.DialogSceneParts[currentScene].SpeechAudio == null)
					{
						lock (lockObj)
						{
							nextSceneToLoad = currentScene;
						}

						timer = new DispatcherTimer();
						//timer.Interval = new TimeSpan(0, (int)(duration / 60), (int)(duration % 60));
						timer.Interval = new TimeSpan(0, 0, 0, 0, 1000);

						timer.Tick += new EventHandler(timer_Tick);

						timer.Start();
					}
					else
					{
						currentDialogScenePartPlayer = new DialogScenePartPlayer();
						
						LayoutRoot.Children.Add(currentDialogScenePartPlayer);

						currentDialogScenePartPlayer.ScenePart = DialogScene.DialogSceneParts[currentScene];

						currentDialogScenePartPlayer.PlayEnded += new PlayEventHandler(currentDialogScenePartPlayer_PlayEnded);

						currentDialogScenePartPlayer.Characters = Characters;

						playing = true;
					}
				}
				else
				{
					// timer.Stop();

					if (currentScene < 0)
					{
						OnPlayPrevious();
					}
					else
					{
						OnPlayNext();
					}

					currentScene = -1;
				}
			}
		}

		public List<Character> Characters
		{
			private get;
			set;
		}

		//private void RequestCurrentScene()
		//{
		//    if (DialogScene.DialogSceneParts[currentScene].SpeechAudio == null)
		//    {
		//        SpeechParam sp = new SpeechParam();
		//        sp.loop = false;
		//        sp.scene = currentScene;

		//        new Thread(new ParameterizedThreadStart(SpeechProcess)).Start(sp);

		//        if (speeches.Count == 0)
		//        {
		//            //if (timer == null)
		//            {
		//                timer = new DispatcherTimer();
		//                //timer.Interval = new TimeSpan(0, (int)(duration / 60), (int)(duration % 60));
		//                timer.Interval = new TimeSpan(0, 0, 0, 0, 1000);

		//                timer.Tick += new EventHandler(timer_Tick);
		//            }

		//            timer.Start();

		//            //autoEvent.WaitOne();

		//            return;
		//        }
		//    }
		//}

		private DispatcherTimer timer;

		void timer_Tick(object sender, EventArgs e)
		{
			new Thread(new ThreadStart(SetupCurrentScenePart)).Start();

			timer.Stop();
		}

		public struct SpeechParam
		{
			public int scene;
			public bool loop;
		}

		private int nextSceneToLoad;
		private object lockObj = new object();

		private void SpeechProcess()
		{
			int sceneToLoad;

			lock (lockObj)
			{
				sceneToLoad = nextSceneToLoad;
				nextSceneToLoad++;

				if (nextSceneToLoad >= DialogScene.DialogSceneParts.Count)
				{
					// Si arribem al final, tornem al principi per si queda alguna escena per baixar
					nextSceneToLoad = 0;
				}
			}

			if (sceneToLoad < 0 || sceneToLoad >= DialogScene.DialogSceneParts.Count)
			{

			}
			else
			{
				DialogScenePart dsp = DialogScene.DialogSceneParts[sceneToLoad];

				if (dsp.SpeechAudio == null)
				{
					string textToSpeech = dsp.Speech;

					textToSpeech = textToSpeech.RemoveHTMLTags();

					SpeechServiceClient client = SLOPlayer.SpeechServiceClient;

					client.SpeakCompleted += (o, ea) =>
					{
						DialogScene.DialogSceneParts[sceneToLoad].SpeechAudio = Utils.DecompressBytes(ea.Result);

						new Thread(new ThreadStart(SpeechProcess)).Start();
					};

					client.SpeakAsync(textToSpeech, dsp.Character.Gender, dsp.Character.Age, dsp.Language);
				}
				else
				{
					// Si l'escena actual per carregar ja te audio, es programa la seguent iteració del SpeechProcess a menys que totes les 
					// escenes ja tinguin audio carregat. En aquest ultim cas, s'aborta el thread de càrrega (no es torna a cridar)

					if (DialogScene.DialogSceneParts.Any(x => x.SpeechAudio == null))
					{
						new Thread(new ThreadStart(SpeechProcess)).Start();
					}
				}
			}
		}

		private void SetupNavigationList(List<DialogScene> value)
		{
			List<Contribution> contributions = new List<Contribution>();

			int sceneId = 0;

			foreach (DialogScene ds in value)
			{
				if (ds.DialogSceneParts.Count > 0)
				{
					DialogScenePart dsp = ds.DialogSceneParts[0];

					Contribution c = new Contribution()
					{
						Id = ds.Order,
						SceneId = sceneId,
						Author = dsp.Character.Name,
						Description = dsp.Speech.Length > 20 ? dsp.Speech.Substring(0, 17) + "..." : dsp.Speech,
						Scene = ds
					};

					contributions.Add(c);
				}

				sceneId++;
			}
			
			NavigationList.Contributions = contributions;
		}
	}
}
