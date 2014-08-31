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
	public static class IndicatorColorExtension
	{
		public static Color ToColor(this IndicatorColor ic)
		{
			switch (ic)
			{
				case IndicatorColor.None: return Colors.White;
				case IndicatorColor.Green: return Colors.Green;
				case IndicatorColor.Yellow: return Colors.Yellow;
				case IndicatorColor.Red: return Colors.Red;
			}

			return Colors.White;
		}
	}
}