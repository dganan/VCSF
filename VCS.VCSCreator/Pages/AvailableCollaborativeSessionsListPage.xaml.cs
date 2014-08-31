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
using System.Windows.Navigation;
using VCS.ConversionService;
using VCS.ActivityLogService;

namespace VCS
{
	public partial class AvailableCollaborativeSessionsListPage : Page
	{
		private static int latestSelectedDataSourceIndex = 0;

		public AvailableCollaborativeSessionsListPage()
		{
			InitializeComponent();

			DataSourcesList.Items.Add("phpBB");
			DataSourcesList.Items.Add("IWT");
			DataSourcesList.Items.Add("DF");

			DataSourcesList.SelectedIndex = latestSelectedDataSourceIndex;
		}

		private void LoadList()
		{
			try
			{
				DataSourcesList.IsEnabled = false;

				ConversionServiceClient converter = VCSCreator.ConversionServiceClient;

				converter.GetAvailableCollaborativeSessionsCompleted += (o, ea) =>
				{
					try
					{
						CollaborativeSessionsList.DataContext = ea.Result;
					}
					catch (Exception e)
					{
						ExceptionHandler.HandleException(e);
					}
					finally
					{
						DataSourcesList.IsEnabled = true;
					}
				};

				converter.GetAvailableCollaborativeSessionsAsync(DataSourcesList.SelectedValue.ToString());
			}
			catch (Exception e)
			{
				ExceptionHandler.HandleException(e);
				DataSourcesList.IsEnabled = true;
			}
		}

		// Executes when the user navigates to this page.
		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
		}

		private void CreateButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				if (CollaborativeSessionsList.SelectedValue == null)
				{
					MessageBox.Show("Please select a collaborative session");
				}
				else
				{
					VCSCreator.CreateSLOFromCollaborativeSession(this.NavigationService.Navigate, DataSourcesList.SelectedValue.ToString(), (CollaborativeSessionsList.SelectedValue as CollaborativeSessionDescriptor).Id);
				}
			}
			catch (Exception ex)
			{
				ExceptionHandler.HandleException(ex);
			}
		}

		private void DataSourcesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			latestSelectedDataSourceIndex = DataSourcesList.SelectedIndex;

			LoadList();
		}
	}
}
