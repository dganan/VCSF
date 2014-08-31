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
	public delegate void NavigateToSceneHandler (Scene scene);

	public partial class SceneNavigationList : UserControl
	{
		public SceneNavigationList()
		{
			InitializeComponent();
		}

		private List<Contribution> contributions;
		
		public List<Contribution> Contributions 
		{
			get { return contributions; } 
			
			set 
			{
				contributions = value;

				NavigationList.ItemsSource = contributions;
			} 
		}

		public event NavigateToSceneHandler NavigateToScene;

		private void SceneLink_Click(object sender, RoutedEventArgs e)
		{
			Button b = e.OriginalSource as Button;

			if (b != null)
			{
				int scene;

				if (NavigateToScene != null && Int32.TryParse(b.Tag.ToString(), out scene))
				{
					NavigateToScene(contributions.Where(x=>x.SceneId == scene).First().Scene);
				}
			}
		}
	}
}
