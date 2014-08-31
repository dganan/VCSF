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
	public partial class VideoScenePlayer : ScenePlayer
	{
		public VideoScenePlayer()
		{
			InitializeComponent();
		}

		public override void Play()
		{
			throw new NotImplementedException();
		}

		public override void Stop()
		{
			throw new NotImplementedException();
		}

		public override void Pause()
		{
			throw new NotImplementedException();
		}

		public override void Next()
		{
		}

		public override void FastNext()
		{
		}

		public override void Previous()
		{
		}

		public override string Title
		{
			set { }
		}

		public override string SubTitle
		{
			set
			{
			}
		}
	}
}
