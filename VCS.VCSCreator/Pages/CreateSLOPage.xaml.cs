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
using System.Windows.Browser;
using VCS.ActivityLogService;
using VCS.SLORepositoryService;

namespace VCS
{
	public partial class CreateSLOPage : Page
	{
		//public static CollaborativeSession CollaborativeSession { set; private get; }
		public static string CollaborativeSessionId { set; private get; }
		public static string CollaborativeSessionSource { set; private get; }

		public CreateSLOPage()
		{
			InitializeComponent();
		}

		// Executes when the user navigates to this page.
		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
		}

		private void OkButton_Click(object sender, RoutedEventArgs e)
		{
			if (String.IsNullOrWhiteSpace(NameTextBox.Text))
			{
				MessageBox.Show("Name must be a non empty string");
			}
			else
			{
				string name = NameTextBox.Text;
				bool openPlayer = (OpenCreatedSLOInPlayer.IsChecked != null && OpenCreatedSLOInPlayer.IsChecked.Value);
				bool openEditor = (OpenCreatedSLOInEditor.IsChecked != null && OpenCreatedSLOInEditor.IsChecked.Value);
				bool automaticCategorization = (AutomaticCategorization.IsChecked != null && AutomaticCategorization.IsChecked.Value);

				CreateSLO(name, openPlayer, openEditor, automaticCategorization, true, true, LoadingPanel, this.NavigationService.Navigate);
			}
		}

		internal static void CreateSLOByDefault()
		{
			CreateSLO("", true, false, false, false, false);
		}

		private static void CreateSLO(string name, bool openPlayer, bool openEditor, bool automaticCategorization, bool openInDifferentWindow, bool showMessages, BusyIndicator loadingPanel = null, Func<Uri, bool> navigate = null)
		{
			try
			{
				ActivityLogger.LogActivity(VCSCreator.UserIp, VCSCreator.UserName, "VCSCreator_CreateSLO"
					, new List<Argument>()	{	
													new Argument() { Key = "CollaborativeSessionId", Value = CollaborativeSessionId },
													new Argument() { Key = "CollaborativeSessionThreadId", Value = (CollaborativeSessionThreadId != null? CollaborativeSessionThreadId : "") },
													new Argument() { Key = "CollaborativeSessionSource", Value = CollaborativeSessionSource },
													new Argument() { Key = "SLOName", Value = name }
												});

				SLORepositoryServiceClient sloRepository = VCSCreator.SLORepositoryServiceClient;

				sloRepository.CreateSLOFromCollaborativeSessionCompleted += (o, ea) =>
				{
					try
					{
						if (loadingPanel != null)
						{
							loadingPanel.IsBusy = false;
						}

						if (ea.Result == null)
						{
							if (showMessages)
							{
								MessageBox.Show(ea.Error.Message);
							}
						}
						else
						{
							if (showMessages)
							{
								MessageBox.Show("SLO was created successfully");
							}

							if (openPlayer)
							{
								string id = ea.Result.ToString();

								if (openInDifferentWindow)
								{
									HtmlPage.Window.Navigate(Pages.SLOPlayerPage(id), "_blank");
								}
								else
								{
									HtmlPage.Window.Navigate(Pages.SLOPlayerPage(id));
								}
							}

							if (openEditor)
							{
								string id = ea.Result.ToString();

								if (openInDifferentWindow)
								{
									HtmlPage.Window.Navigate(Pages.SLOEditorPage(id), "_blank");
								}
								else
								{
									HtmlPage.Window.Navigate(Pages.SLOEditorPage(id));
								}
							}

							if (navigate != null)
							{
								navigate(Pages.AvailableCollaborativeSessionsListPageUri);
							}
						}
					}
					catch (Exception ex)
					{
						ExceptionHandler.HandleException(ex);
					}
				};

				if (loadingPanel != null)
				{
					loadingPanel.IsBusy = true;
				}

				sloRepository.Endpoint.Binding.CloseTimeout = new TimeSpan (2, 0, 0); // 2 hours
				sloRepository.CreateSLOFromCollaborativeSessionAsync(CollaborativeSessionId, CollaborativeSessionThreadId, SecurityToken, CollaborativeSessionSource, name, automaticCategorization, VCSCreator.UserId);
			}
			catch (Exception ex)
			{
				ExceptionHandler.HandleException(ex);
			}
		}

		private void CancelButton_Click(object sender, RoutedEventArgs e)
		{
			ActivityLogger.LogActivity(VCSCreator.UserIp, VCSCreator.UserName, "VCSCreator_CancelCreateSLO");

			CollaborativeSessionId = null;
			CollaborativeSessionSource = null;

			this.NavigationService.Navigate(Pages.AvailableCollaborativeSessionsListPageUri);
		}

		public static string CollaborativeSessionThreadId { get; set; }

		public static string SecurityToken { get; set; }
	}
}
