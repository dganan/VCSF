﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VCS
{
	public static class SceneFactory
	{
		public static Scene CreateScene(SceneType type, string name)
		{ 
			Scene s = null;

			switch (type)
			{
				case SceneType.Dialog:

					s = new DialogScene();
					break;

				case SceneType.Assessment:

				    s = new AssessmentScene();
				    break;

				case SceneType.Video:

					s = new VideoScene();
					break;

				case SceneType.EmotionalTest:

					s = new EmotionalTestScene();
					break;

				case SceneType.References:

					s = new ReferencesScene();
					break;
			}

			if (s != null)
			{
				s.Name = name;
			}

			return s;
		}
	}
}
