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
	public static class ImageExtensions
	{
		public static BitmapImage ToBitmapImage(this byte[] imageBytes)
		{
			BitmapImage b = new BitmapImage();

			try
			{
				MemoryStream stream = new MemoryStream(imageBytes);

				b.SetSource(stream);
			}
			catch (Exception e)
			{
				b = new BitmapImage(new Uri("../images/avatars/nofoto.jpg", UriKind.Relative));
			}

			return b;
		}
	}
}
