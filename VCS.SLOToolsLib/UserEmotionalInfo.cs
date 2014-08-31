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
	public class UserEmotionalInfo
	{
		public EmotionalState EmotionalState { get; set; }

		public int Scene { get; set; }

		public DateTime When { get; set; }
	}
}
