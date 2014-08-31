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
	public partial class DoubleListBox : UserControl
	{
		public DoubleListBox()
		{
			InitializeComponent();
		}

		public List<string> SourceItems
		{
			set 
			{
				SourceList.ItemsSource = value.Where(x => !TargetList.Items.Contains(x)).OrderBy(x => x);
			}	
		}

		public List<string> TargetItems
		{
			get 
			{
				return TargetList.Items.Select(x => x.ToString()).ToList();
			}

			set 
			{
				TargetList.ItemsSource = value;

				SourceList.ItemsSource = SourceList.Items.Where(x => !value.Contains(x.ToString()));
			}
		}

		public void AddItemToTarget(string item)
		{
			List<string> target = TargetList.Items.Cast<string>().ToList();

			target.Add(item);

			//target = target.OrderBy(x => x).ToList();

			List<string> source = SourceList.Items.Cast<string>().Where (x=>x!= item).ToList();

			TargetList.ItemsSource = target;

			SourceList.ItemsSource = source;
		}

		private void MoveAllToRight_Click(object sender, RoutedEventArgs e)
		{
			List<string> target = TargetList.Items.Cast<string>().Union(SourceList.Items.Cast<string>()).ToList(); 

			List<string> source = new List<string>();

			TargetList.ItemsSource = target;

			SourceList.ItemsSource = source;
		}

		private void MoveSelectedToRight_Click(object sender, RoutedEventArgs e)
		{
			List<string> target = TargetList.Items.Cast<string>().Union(SourceList.SelectedItems.Cast<string>()).ToList();

			List<string> source = SourceList.Items.Cast<string>().Except(SourceList.SelectedItems.Cast<string>()).ToList();

			TargetList.ItemsSource = target;
			
			SourceList.ItemsSource = source;
		}

		private void MoveSelectedToLeft_Click(object sender, RoutedEventArgs e)
		{
			List<string> source = TargetList.SelectedItems.Cast<string>().Union(SourceList.Items.Cast<string>()).OrderBy(x => x).ToList();

			List<string> target = TargetList.Items.Cast<string>().Except(TargetList.SelectedItems.Cast<string>()).ToList();

			TargetList.ItemsSource = target;

			SourceList.ItemsSource = source;
		}

		private void MoveAllToLeft_Click(object sender, RoutedEventArgs e)
		{
			List<string> source = TargetList.Items.Cast<string>().Union(SourceList.Items.Cast<string>()).OrderBy(x => x).ToList();

			List<string> target = new List<string>();

			TargetList.ItemsSource = target;

			SourceList.ItemsSource = source;
		}
	}
}
