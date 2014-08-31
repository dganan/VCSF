using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace VCS
{
	public static class Pages
	{
		public static Uri AvailableSLOsListPage
		{
			get
			{
				return new Uri("/Pages/" + typeof(AvailableSLOsListPage).Name + ".xaml", UriKind.Relative);
			}
		}

		public static Uri StoryBoardEditorPage
		{
			get
			{
				return new Uri("/Pages/" + typeof(StoryBoardEditorPage).Name + ".xaml", UriKind.Relative);
			}
		}

		public static Uri CreateNewScenePage
		{
			get
			{
				return new Uri("/Pages/" + typeof(CreateNewScenePage).Name + ".xaml", UriKind.Relative);
			}
		}

		internal static Uri SceneEditorPage(SceneType type)
		{
			Uri uri = null;

			switch (type)
			{
				case SceneType.Dialog:

					uri = DialogSceneEditorPage;
					break;

				case SceneType.Assessment:

					uri = AssessmentSceneEditorPage;
					break;

				case SceneType.EmotionalTest:

					uri = EmotionalTestSceneEditorPage;
					break;
					
				case SceneType.References:

					uri = ReferencesSceneEditorPage;
					break;

				case SceneType.Video:

					MessageBox.Show("Video scenes are not supported yet");
					// uri = new Uri("/Pages/" + typeof(VideoSceneEditorPage).Name + ".xaml", UriKind.Relative);
					break;
			}

			return uri;
		}

		public static Uri DialogScenePartEditorPage
		{
			get
			{
				return new Uri("/Pages/" + typeof(DialogScenePartEditorPage).Name + ".xaml", UriKind.Relative);
			}
		}

		public static Uri AssessmentSceneEditorPage
		{
			get
			{
				return new Uri("/Pages/" + typeof(AssessmentSceneEditorPage).Name + ".xaml", UriKind.Relative);
			}
		}

		public static Uri CharactersEditorPage
		{
			get
			{
				return new Uri("/Pages/" + typeof(CharactersEditorPage).Name + ".xaml", UriKind.Relative);
			}
		}

		public static Uri CharacterEditorPage
		{
			get
			{
				return new Uri("/Pages/" + typeof(CharacterEditorPage).Name + ".xaml", UriKind.Relative);
			}
		}

		public static Uri DialogSceneEditorPage
		{
			get
			{
				return new Uri("/Pages/" + typeof(DialogSceneEditorPage).Name + ".xaml", UriKind.Relative);
			}
		}

		public static Uri AssessmentSceneQuestionEditorPage 
		{
			get
			{
				return new Uri("/Pages/" + typeof(AssessmentSceneQuestionEditorPage).Name + ".xaml", UriKind.Relative);
			}
		}

		public static Uri EmotionalTestSceneEditorPage
		{
			get
			{
				return new Uri("/Pages/" + typeof(EmotionalTestSceneEditorPage).Name + ".xaml", UriKind.Relative);
			}
		}

		public static Uri ReferencesSceneEditorPage
		{
			get
			{
				return new Uri("/Pages/" + typeof(ReferencesSceneEditorPage).Name + ".xaml", UriKind.Relative);
			}
		}
	}
}
