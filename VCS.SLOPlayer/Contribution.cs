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
	public class Contribution
	{
		public int Id { get; set; }
		public int SceneId { get; set; }
		public string Author { get; set; }
		public string Description { get; set; }
		public Scene Scene { get; set; }
	}
}
