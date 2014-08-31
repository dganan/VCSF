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
using VCS.SLORepositoryService;

namespace VCS
{
	public partial class AvailableSLOsListPage : Page
	{
		public AvailableSLOsListPage()
		{
			InitializeComponent();

			// Initialize SLO in edit (none at this point)
			StoryBoardEditor.EditingSLO = null;
			SceneEditor.EditingScene = null;

			LoadList();
		}

		private void LoadList()
		{
			try
			{
				SLORepositoryServiceClient sloRepository = SLOEditor.SLORepositoryServiceClient;

				sloRepository.GetAvailableSLOsCompleted += (o, ea) =>
				{
					try
					{
						SLOsList.DataContext = ea.Result;
					}
					catch (Exception e)
					{
						ExceptionHandler.HandleException(e);
					}
				};

				sloRepository.GetAvailableSLOsAsync(SLOEditor.UserInfo.Id);
			}
			catch (Exception e)
			{
				ExceptionHandler.HandleException(e);
			}
		}

		// Executes when the user navigates to this page.
		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
		}

		private void EditButton_Click(object sender, RoutedEventArgs e)
		{
			EditSLO();
		}

		private void EditSLO()
		{
			try
			{
				if (SLOsList.SelectedValue == null)
				{
					MessageBox.Show("Please select an element");
				}
				else
				{
					SLOEditor.EditSLOWithId(this.NavigationService.Navigate, (SLOsList.SelectedValue as SLODescriptor).Id);
				}
			}
			catch (Exception ex)
			{
				ExceptionHandler.HandleException(ex);
			}
		}

		private void CreateButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				SLOEditor.CreateAndEditSLO(this.NavigationService.Navigate);
			}
			catch (Exception ex)
			{
				ExceptionHandler.HandleException(ex);
			}
		}

		private void SLOsPanel_DoubleClick(object sender, EventArgs e)
		{
			EditSLO();
		}
	}
}
