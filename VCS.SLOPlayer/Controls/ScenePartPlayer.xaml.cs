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
	public partial class ScenePartPlayer : UserControl
	{
		public event PlayEventHandler PlayEnded;

		public ScenePartPlayer()
		{
			InitializeComponent();
		}

		public virtual ScenePart ScenePart { set; protected get; }

		public virtual void Stop() { }

		protected void OnPlayEnded()
		{
			if (PlayEnded != null)
			{
				PlayEnded();
			}
		}
	}
}
