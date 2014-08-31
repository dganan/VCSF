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
using System.Windows.Media.Imaging;
using System.IO;

namespace VCS
{
	public partial class Character
	{
		public BitmapImage Avatar
		{
			get
			{
				if (!this.UseAnimatedAvatar && this.PhotoAvatar != null)
				{
					return this.PhotoAvatarImage;
				}
				else
				{
					// return new BitmapImage(new Uri("../images/" + this.Face + ".png", UriKind.Relative));
					return AvatarOpenedClosed;
				}
			}
		}

		public BitmapImage AvatarOpenedClosed
		{
			get
			{
				return new BitmapImage(new Uri("/VCS.SLOToolsLib;component/images/avatars/" + this.AnimationAvatar + "_opened_closed.jpg", UriKind.Relative));
			}
		}

		public BitmapImage AvatarOpenedOpened
		{
			get
			{
				return new BitmapImage(new Uri("/VCS.SLOToolsLib;component/images/avatars/" + this.AnimationAvatar + "_opened_opened.jpg", UriKind.Relative));
			}
		}

		public BitmapImage AvatarClosedClosed
		{
			get
			{
				return new BitmapImage(new Uri("/VCS.SLOToolsLib;component/images/avatars/" + this.AnimationAvatar + "_closed_closed.jpg", UriKind.Relative));
			}
		}

		public BitmapImage PhotoAvatarImage
		{
			get 
			{
				return (this.PhotoAvatar==null?new BitmapImage():this.PhotoAvatar.ToBitmapImage());
			}
		}
	}
}
