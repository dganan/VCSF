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
using System.Windows.Browser;
using VCS.ActivityLogService;

namespace VCS
{
	public partial class AvailableSLOsListPage : Page
	{
		public AvailableSLOsListPage()
		{
			InitializeComponent();

			LoadList();

			if (Config.Language == VCS.Language.Catalan)
			{ 
				Titol.Text = "Reproductor de video-debats";
				SelectLabel.Text = "Selecciona un video-debat per reproduir:";
			}
		}

		private void LoadList()
		{
			try
			{
				SLORepositoryServiceClient sloRepository = SLOPlayer.SLORepositoryServiceClient;

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

				sloRepository.GetAvailableSLOsAsync(SLOPlayer.UserInfo.Id);
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

		private void PlayButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				if (SLOsList.SelectedValue == null)
				{
					MessageBox.Show("Please select an element");
				}
				else
				{
					string id = (SLOsList.SelectedValue as SLODescriptor).Id;

					ActivityLogger.LogActivity(SLOPlayer.UserInfo.Ip, SLOPlayer.UserInfo.Name, "SLOPlayer_SelectSLOToPlay"
						, new List<Argument>()	{	
													new Argument() { Key = "id", Value = id }
												});

					SLOPlayer.PlaySLOWithId(this.NavigationService.Navigate, id);
				}
			}
			catch (Exception ex)
			{
				ExceptionHandler.HandleException(ex);
			}
		}

		private void OpenSourceButton_Click(object sender, RoutedEventArgs e)
		{
			OpenSLO();
		}

		private void OpenSLO()
		{
			try
			{
				if (SLOsList.SelectedValue == null)
				{
					MessageBox.Show("Please select an element");
				}
				else
				{
					string id = (SLOsList.SelectedValue as SLODescriptor).Id;

					ActivityLogger.LogActivity(SLOPlayer.UserInfo.Ip, SLOPlayer.UserInfo.Name, "SLOPlayer_OpenSLOSourceCollaborativeSession"
						, new List<Argument>()	{	
													new Argument() { Key = "id", Value = id }
												});

					SLOPlayer.OpenSourceSLOWithId(id);
				}
			}
			catch (Exception ex)
			{
				ExceptionHandler.HandleException(ex);
			}
		}
		
		private void SLOsPanel_DoubleClick(object sender, EventArgs e)
		{
			string id = (SLOsList.SelectedValue as SLODescriptor).Id;

			ActivityLogger.LogActivity(SLOPlayer.UserInfo.Ip, SLOPlayer.UserInfo.Name, "SLOPlayer_SelectSLOToPlay"
				, new List<Argument>()	{	
													new Argument() { Key = "id", Value = id }
												});

			SLOPlayer.PlaySLOWithId(this.NavigationService.Navigate, id);
		}
	}
}
