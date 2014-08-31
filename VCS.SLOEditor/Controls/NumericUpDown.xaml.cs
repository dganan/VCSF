using System;
using System.Collections;
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
	public delegate void ValueChangedHandler ();

	public partial class NumericUpDown : UserControl
	{
		public event ValueChangedHandler ValueChanged;

		private double? value;

		public double? Value
		{
			get
			{
				if (value != null)
				{
					return Math.Round(value.Value, Decimals);
				}

				return null;
			}
			
			set 
			{
				if (value != null)
				{
					this.value = Math.Round(value.Value, Decimals);
				}
				else
				{
					this.value = null;
				}

				OnValueChanged(); 
			}
		}

		public NumericUpDown()
		{
			MinValue = 0;
			MaxValue = 10;
			DefaultValue = 0;

			InitializeComponent();
		}

		public double MinValue { get; set; }
		public double MaxValue { get; set; }
		public double DefaultValue { get; set; }

		private void DecreaseSlow_Click(object sender, RoutedEventArgs e)
		{
			if (value == null)
			{
				value = DefaultValue;
			}
			else if (value > (MinValue + incrementSlow))
			{
				value = value - incrementSlow;
			}
			else 
			{
				value = MinValue;
			}

			OnValueChanged();
		}

		private void OnValueChanged()
		{
			if (value != null)
			{
				tbValue.Text = value.ToString();
			}

			if (ValueChanged != null)
			{
				ValueChanged();
			}
		}

		private void DecreaseFast_Click(object sender, RoutedEventArgs e)
		{
			if (value == null)
			{
				value = DefaultValue;
			}
			else if (value > (MinValue + incrementFast))
			{
				value = value - incrementFast;
			}
			else
			{
				value = MinValue;
			}

			OnValueChanged();
		}

		private void IncreaseSlow_Click(object sender, RoutedEventArgs e)
		{
			if (value == null)
			{
				value = DefaultValue;
			}
			else if (value < (MaxValue - incrementSlow))
			{
				value = value + incrementSlow;
			}
			else
			{
				value = MaxValue;
			}

			OnValueChanged();
		}

		private void IncreaseFast_Click(object sender, RoutedEventArgs e)
		{
			if (value == null)
			{
				value = DefaultValue;
			}
			else if (value < (MaxValue - incrementFast))
			{
				value = value + incrementFast;
			}
			else
			{
				value = MaxValue;
			}

			OnValueChanged();
		}

		private double incrementFast = 1;

		//public double IncrementFast
		//{
		//    get { return incrementFast; }
		//    set { incrementFast = Math.Round(value, Decimals); }
		//}

		private double incrementSlow = 0.1;

		//public double IncrementSlow
		//{
		//    get { return incrementSlow; }
		//    set { incrementSlow = Math.Round(value, Decimals); }
		//}

		private int decimals = 1;

		public int Decimals
		{
			get { return decimals; }

			set
			{
				decimals = value;

				if (decimals == 1)
				{
					incrementFast = 1;
					incrementSlow = 0.1;
				}
				else if (decimals == 2)
				{
					incrementFast = 0.1;
					incrementSlow = 0.01;
				}

				ToolTipService.SetToolTip(DecreaseFast, "Decrease " + incrementFast);
				ToolTipService.SetToolTip(DecreaseSlow, "Decrease " + incrementSlow);
				ToolTipService.SetToolTip(IncreaseFast, "Increase " + incrementFast);
				ToolTipService.SetToolTip(IncreaseSlow, "Increase " + incrementSlow);
			}
		}

		public bool ShowIncreaseFast
		{
			get { return DecreaseFast.Visibility == Visibility.Visible; }

			set
			{
				DecreaseFast.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
				IncreaseFast.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
			}
		}
	}
}
