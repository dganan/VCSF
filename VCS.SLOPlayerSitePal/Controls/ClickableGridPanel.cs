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
	public class ClickableGridPanel : Grid
	{
		public event EventHandler DoubleClick;

		private static bool parentPageHandled;

		public ClickableGridPanel()
			: base()
		{
			this.Loaded += new RoutedEventHandler(ClickableGridPanel_Loaded);
		}

		void ClickableGridPanel_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter && DoubleClick != null)
			{
				DoubleClick(this, new EventArgs());
			}
		}

		void ClickableGridPanel_Loaded(object sender, RoutedEventArgs e)
		{
			if (!parentPageHandled)
			{
				var parent = VisualTreeHelper.GetParent(this);

				while (!(parent is Page) && parent != null)
				{
					parent = VisualTreeHelper.GetParent(parent);
				}

				(parent as Page).KeyDown += new KeyEventHandler(ClickableGridPanel_KeyDown);
			}

			Canvas canvas = new Canvas();

			canvas.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
			canvas.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
			canvas.Background = new SolidColorBrush(Colors.Transparent);
			canvas.MouseLeftButtonUp += new MouseButtonEventHandler(canvas_MouseLeftButtonUp);

			this.Children.Add(canvas);
			this.UpdateLayout();
		}

		bool singleClick = false;
		object lastSender;

		void canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			if (!singleClick || lastSender != sender)
			{
				singleClick = true;
				lastSender = sender;

				System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();
				timer.Interval = TimeSpan.FromMilliseconds(500);
				timer.Tick += new EventHandler((o, ea) => { (o as System.Windows.Threading.DispatcherTimer).Stop(); singleClick = false; });
				timer.Start();
			}
			else
			{
				if (DoubleClick != null)
				{
					DoubleClick(this, new EventArgs());
				}

				singleClick = false;
				lastSender = null;
			}
		}
	}
}
