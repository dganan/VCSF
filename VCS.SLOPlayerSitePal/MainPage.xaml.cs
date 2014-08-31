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
using System.Windows.Browser;
using System.Windows.Navigation;

namespace VCS
{
	public partial class MainPage : UserControl
	{
		public MainPage()
		{
			InitializeComponent();
		
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
        }
 
        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            IDictionary<string, string> qString = HtmlPage.Document.QueryString;
			
			// TODO : username is needed anymore?
			string user = qString.ContainsKey("User") ? qString["User"] : null;

			if (user != null)
			{
				SLOPlayer.UserInfo.Name = user;
			}

			string login = qString.ContainsKey("login") ? qString["login"] : null;

			if (login != null)
			{
				SLOPlayer.UserInfo.Name = login;
			}

			string userId = qString.ContainsKey("UserId") ? qString["UserId"] : null;

			if (userId != null)
			{
				SLOPlayer.UserInfo.Name = userId;
				SLOPlayer.UserInfo.Id = userId;
			}

			string courseId = qString.ContainsKey("CourseId") ? qString["CourseId"] : null;

			if (userId != null)
			{
				SLOPlayer.UserInfo.CourseId = courseId;
			}

			if (qString.ContainsKey("SLOId"))
			{
				SLOPlayer.PlaySLOWithId(MainFrame.Navigate, qString["SLOId"], MainPanel);
			}
			else 
			{
				MainPanel.Visibility = System.Windows.Visibility.Visible;
			}

			string taxons = qString.ContainsKey("Taxons") ? qString["Taxons"] : null;

			if (taxons != null)
			{
				SLOPlayer.Taxons = taxons.Split('_').Select(x=>Int32.Parse (x)).ToArray();
			}

			if (qString.ContainsKey("previes"))
			{
				SLOPlayer.UserInfo.MeetsPrerequisites = (qString["previes"] == "1");
			}

			//// IWT INTEGRATION

			//if (courseId != null && userId != null)
			//{
			//    LearningModelServices.LearningModelServicesSoapClient learningModelService = new LearningModelServices.LearningModelServicesSoapClient();

			//    learningModelService.GetUserCognitiveStateCompleted += (o, cse) => { 
					
			//        var taxonLevelList = cse.Result;

			//        foreach (var taxonLevel in taxonLevelList)
			//        { 
			//            SLOPlayer.UserInfo.AssessmentInfo.Add (new UserAssessmentInfo () { When = DateTime.Now, Score = taxonLevel.Level });
			//        }
			//    };

			//    learningModelService.GetUserCognitiveStateAsync(Int32.Parse (SLOPlayer.UserInfo.Id));
			//}

			//// IWT INTEGRATION END

			// IWT INTEGRATION 

			if (SLOPlayer.UserInfo.CourseId != null && SLOPlayer.UserInfo.Id != null)
			{
				AffectiveEmotiveServices.AffectiveEmotiveServicesSoapClient affectiveEmotiveService = SLOPlayer.AffectiveEmotiveServicesClient;

				affectiveEmotiveService.GetLastUserEmotionalStateCompleted += new EventHandler<AffectiveEmotiveServices.GetLastUserEmotionalStateCompletedEventArgs>((o, ea) => { 
					
					SLOPlayer.IWTEmotionalInfo = ea.Result;

					if (qString.ContainsKey("showemotional"))
					{
						if (ea.Result != null)
						{
							MessageBox.Show("AUTFRU: " + ea.Result.AutFru + ", ECCIND: " + ea.Result.EccInd + ", FIDANS: " + ea.Result.FidAns + ", INTDIS: " + ea.Result.IntDis);
						}
						else 
						{
							MessageBox.Show("NO DATA");
						}
					}				
				
				});

				affectiveEmotiveService.GetLastUserEmotionalStateAsync(Int32.Parse(SLOPlayer.UserInfo.Id), Int32.Parse(SLOPlayer.UserInfo.CourseId));
			}

			// IWT INTEGRATION END

        }

		private void OpenButton_Click(object sender, RoutedEventArgs e)
		{
			MainFrame.Navigate(new Uri("/Pages/AvailableSLOsListPage.xaml", UriKind.Relative));
		}
	}
}
