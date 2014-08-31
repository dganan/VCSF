using System;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using VCS;

namespace VCS
{
	public class SceneNavigator
	{
		protected SLO SLO { get; set; }
		
		private int CurrentSceneIndex { get; set; }

		public Scene CurrentScene 
		{
			get
			{ 
				if (CurrentSceneIndex >= 0 && CurrentSceneIndex < SLO.Scenes.Count)
				{
					return SLO.Scenes[CurrentSceneIndex];
				}

				return null;
			}
		}

		public SceneNavigator(SLO slo)
		{
			SLO = slo;

			FirstScene();
		}

		public Scene FirstScene()
		{
			CurrentSceneIndex = 0;

			return CurrentScene;
		}

		public Scene NextScene()
		{
			CurrentSceneIndex++;

			return CurrentScene;
		}

		public Scene PreviousScene()
		{
			CurrentSceneIndex--;

			return CurrentScene;
		}

		public PlayStatus PlayStatus { get; set; }

		internal Scene JumpToScene(int scene)
		{
			CurrentSceneIndex = scene;

			return CurrentScene;
		}
	}
}
