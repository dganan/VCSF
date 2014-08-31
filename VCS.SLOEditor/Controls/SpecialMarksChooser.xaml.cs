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
using System.Windows.Media.Imaging;

namespace VCS
{
	public partial class SpecialMarksChooser : UserControl
	{
		public SpecialMarksChooser()
		{
			InitializeComponent();

			LoadItems();
		}

		private void LoadItems()
		{
			foreach (string sm in DialogSpecialMarks.GetValues())
			{
				CheckBox cb = new CheckBox();
				cb.Margin = new Thickness(0, 8, 0, 0);
				cb.Tag = sm;

				Image i = new Image();

				i.Width = 32;
				i.Height = 32;

				i.Source = new BitmapImage (new Uri ("/VCS.SLOToolsLib;component/images/marks/" + sm.ToLower() + ".png", UriKind.Relative));

				StackPanel sp = new StackPanel();

				sp.Orientation = Orientation.Horizontal;
				sp.VerticalAlignment = System.Windows.VerticalAlignment.Center;
				sp.Margin = new Thickness(10);

				sp.Children.Add(cb);
				sp.Children.Add(i);
				
				wpSpecialMarks.Children.Add(sp);
			}
		}

		public List<DialogSpecialMark> SelectedValues
		{
			get 
			{
				List<DialogSpecialMark> selected = new List<DialogSpecialMark>();

				foreach (StackPanel sp in wpSpecialMarks.Children)
				{
					CheckBox cb = sp.Children[0] as CheckBox;

					if (cb.IsChecked ?? false)
					{
						DialogSpecialMark sm;

						Enum.TryParse<DialogSpecialMark>(cb.Tag.ToString(), out sm);

						selected.Add(sm);
					}
				}

				return selected;
			}
			
			set 
			{
				foreach (StackPanel sp in wpSpecialMarks.Children)
				{
					CheckBox cb = sp.Children[0] as CheckBox;

					if (value.Where (x=>x.ToString().ToLower() == cb.Tag.ToString().ToLower()).Count()>0)
					{
						cb.IsChecked = true;
					}
				}
			}
		}
	}
}
