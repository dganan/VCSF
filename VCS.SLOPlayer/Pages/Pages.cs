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
		public static Uri AvailableSLOsListPageUri
		{
			get 
			{
				return new Uri("/Pages/" + typeof(AvailableSLOsListPage).Name + ".xaml", UriKind.Relative);
			}
		}

		public static Uri StoryBoardPlayerPageUri
		{
			get
			{
				return new Uri("/Pages/" + typeof(StoryBoardPlayerPage).Name + ".xaml", UriKind.Relative);
			}
		}
	}
}
