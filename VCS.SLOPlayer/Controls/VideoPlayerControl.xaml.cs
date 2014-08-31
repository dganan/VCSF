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
	public partial class VideoPlayerControl : UserControl
	{
		public event PlayEventHandler PlayEnded;

		public VideoPlayerControl()
		{
			InitializeComponent();
		}

		public void PlayVideoByUri(string uri, UriKind uk = UriKind.Absolute)
		{ 
			VideoPlayer.Source = new Uri(uri, uk);

			VideoPlayer.MediaEnded += new RoutedEventHandler((o, e) => 
			{
				if (PlayEnded != null)
				{
					PlayEnded();
				}
			});

			VideoPlayer.Play();
		}
	}
}
