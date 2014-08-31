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

namespace VCS
{
	public delegate void PlayEventHandler();
	public delegate void JumpEventHandler(int sceneToJump);

	public delegate void ScenePlayerChangedHandler(ScenePlayer sp);

	public partial class StoryBoardPlayer : UserControl
	{
		public event PlayEventHandler PlayEnded;

		public event ScenePlayerChangedHandler ScenePlayerChanged;

		ScenePlayer currentScenePlayer;

		protected SceneNavigator sceneNavigator;

		public StoryBoardPlayer()
		{
			InitializeComponent();
		}

		protected static SLO slo;

		public static List<Character> Characters
		{
			get { return slo.Characters; }
		}

		public SLO SLO
		{
			private get { return slo; }
			
			set 
			{
				slo = value;

				InitializePlay();
			} 
		}

		private void InitializePlay()
		{
			currentScenePlayer = null;

			LayoutRoot.Children.Clear();

			sceneNavigator = new SceneNavigator(slo);

			if (slo.Scenes.Count() == 0)
			{
				MessageBox.Show("SLO does not contain scenes");
			}
		}

		public void Play()
		{
			if (sceneNavigator.CurrentScene == null)
			{
				OnPlayEnded();
			}
			else 
			{
				if (currentScenePlayer == null)
				{
					// Get suitable player for current scene
						
					currentScenePlayer = ScenePlayerFactory.GetPlayer(sceneNavigator.CurrentScene);

					if (ScenePlayerChanged != null)
					{
						ScenePlayerChanged(currentScenePlayer);
					}

					if (slo.Name != null)
					{
						string[] titles = slo.Name.Split(new string [] { "#;#" }, StringSplitOptions.RemoveEmptyEntries);

						if (titles.Length > 0)
						{
							currentScenePlayer.Title = titles[0];
						}

						if (titles.Length > 1)
						{
							currentScenePlayer.SubTitle = titles[1];
						}
					}

					DialogScenePlayer dsp = currentScenePlayer as DialogScenePlayer;

					if (dsp!=null)
					{
						List<DialogScene> contributions = ContributionsList(sceneNavigator.CurrentScene);
						
						dsp.Contributions = contributions;

						dsp.Characters = OrderCharacters(contributions, sceneNavigator.CurrentScene as DialogScene);
					}

					LayoutRoot.Children.Clear();
					LayoutRoot.Children.Add(currentScenePlayer);

					currentScenePlayer.PlayNext += () =>
					{
						currentScenePlayer = null;

						sceneNavigator.NextScene();

						Thread.Sleep(500);

						Play();
					};

					currentScenePlayer.PlayPrevious += () =>
					{
						currentScenePlayer = null;

						sceneNavigator.PreviousScene();

						Thread.Sleep(500);

						Play();
					};

					currentScenePlayer.StopPlay += () =>
					{
						Stop();
					};

					currentScenePlayer.PlayEnded += () =>
					{
						OnPlayEnded();
					};

					currentScenePlayer.JumpToScene += (scene) =>
					{
						currentScenePlayer = null;

						sceneNavigator.JumpToScene(scene);

						Thread.Sleep(500);

						Play();
					};
				}

				currentScenePlayer.Play();
			}
		}

		private List<Character> OrderCharacters(List<DialogScene> contributions, DialogScene scene)
		{
			List<Character> orderedCharacters = new List<Character>();

			Character current_character = scene.UsedCharacters.First();

			List<Character> speakingCharacters = contributions.SelectMany(x => x.UsedCharacters).ToList();

			int actual_position = speakingCharacters.IndexOf(current_character);
			int previous_position = actual_position - 1;
			int next_position = actual_position + 1;
			
			while (orderedCharacters.Count < speakingCharacters.Count - 1 && (previous_position >= 0 || next_position < speakingCharacters.Count))
			{
				if (previous_position >= 0)
				{
					if (!orderedCharacters.Contains(speakingCharacters[previous_position]) && !speakingCharacters[previous_position].Equals(speakingCharacters[actual_position]))
					{
						orderedCharacters.Add(speakingCharacters[previous_position]);
					}

					previous_position--;
				}

				if (next_position < speakingCharacters.Count)
				{
					if (!orderedCharacters.Contains(speakingCharacters[next_position]) && !speakingCharacters[next_position].Equals(speakingCharacters[actual_position]))
					{
						orderedCharacters.Add(speakingCharacters[next_position]);
					}

					next_position++;
				}
			}

			return orderedCharacters;
		}

		private List<DialogScene> ContributionsList(Scene scene)
		{
			List<DialogScene> contributions = new List<DialogScene>();

			int currentIndex = SLO.Scenes.IndexOf(scene);

			int tempIndex = currentIndex -1;

			while (tempIndex >= 0 && SLO.Scenes[tempIndex] is DialogScene)
			{
				contributions.Insert(0, SLO.Scenes[tempIndex] as DialogScene);

				tempIndex--;
			}

			contributions.Add(SLO.Scenes[currentIndex] as DialogScene);

			tempIndex = currentIndex + 1;

			while (tempIndex < SLO.Scenes.Count && SLO.Scenes[tempIndex] is DialogScene)
			{
				contributions.Add(SLO.Scenes[tempIndex] as DialogScene);

				tempIndex++;
			}

			return contributions;
		}

		private void OnPlayEnded()
		{
			InitializePlay();

			if (PlayEnded != null)
			{
				PlayEnded();
			}
		}

		public void Stop()
		{
			if (currentScenePlayer != null)
			{
				currentScenePlayer.Stop();
			}

			InitializePlay();

			OnPlayEnded();
		}

		public void Pause()
		{
			if (currentScenePlayer != null)
			{
				currentScenePlayer.Pause();
			}
		}

		public void Next()
		{
			if (currentScenePlayer != null)
			{
				currentScenePlayer.Next();
			}
		}

		public void FastNext()
		{
			if (currentScenePlayer != null)
			{
				currentScenePlayer.FastNext();
			}
		}

		public void Previous()
		{
			if (currentScenePlayer != null)
			{
				currentScenePlayer.Previous();
			}
		}
	}
}
