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
	public partial class ScenePlayer : UserControl
	{
		public event PlayEventHandler PlayNext;
		public event PlayEventHandler PlayPrevious;

		public event PlayEventHandler PlayEnded;
		public event PlayEventHandler StopPlay;

		public event JumpEventHandler JumpToScene;

		public virtual Scene Scene { get; set; }

		public ScenePlayer()
		{
			InitializeComponent();
		}

		public virtual void Play() {}

		public virtual void Stop() {}

		public virtual void Pause() {}

		public virtual void Next() {}

		public virtual void FastNext() {}

		public virtual void Previous() {}

		protected void OnPlayNext()
		{
			if (this.Scene.IsEndScene && this.Scene.CanBeManuallySetAsEndScene)
			{
				if (StopPlay != null)
				{
					StopPlay();
				}
			}
			else if (PlayNext != null)
			{
				PlayNext();
			}
		}

		protected void OnJumpToScene(Scene sceneToJump, bool endPlay = false)
		{
			if (endPlay)
			{
				if (StopPlay != null)
				{
					StopPlay();
				}
			}
			else 
			{
				if (sceneToJump == null)
				{
					StopPlay();
				}
				else if (JumpToScene != null)
				{
					JumpToScene(sceneToJump.Order);
				}
			}
		}

		protected void OnPlayPrevious()
		{
			if (PlayPrevious != null)
			{
				PlayPrevious();
			}
		}

		protected void OnStop()
		{
			if (StopPlay != null)
			{
				StopPlay();
			}
		}

		protected void OnPlayEnded()
		{
			if (PlayEnded != null)
			{
				PlayEnded();
			}
		}

		public virtual string Title { set; private get; }

		public virtual string SubTitle { set; private get; }

		public virtual bool IsPlayEnabled { get { return true; } }
		public virtual bool IsNextEnabled { get { return true; } }
		public virtual bool IsFastNextEnabled { get { return true; } }
		public virtual bool IsPreviousEnabled { get { return true; } }
		public virtual bool IsPauseEnabled { get { return true; } }
		public virtual bool IsStopEnabled { get { return true; } }
	}
}
