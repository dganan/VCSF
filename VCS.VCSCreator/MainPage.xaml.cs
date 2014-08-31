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

namespace VCS
{
	public partial class MainPage : UserControl
	{
		public MainPage()
		{
			InitializeComponent();
	
			CheckQueryString();
			
			//this.Loaded += new RoutedEventHandler(MainPage_Loaded);
		}

		//void MainPage_Loaded(object sender, RoutedEventArgs e)
		//{
		//    CheckQueryString();
		//}

		private bool CheckQueryString()
		{
			IDictionary<string, string> qString = HtmlPage.Document.QueryString;

			if (qString.ContainsKey("DS") && qString.ContainsKey("Id"))
			{
				this.MainFrame.Source = null;

				string thread = qString.ContainsKey("TopicId") ? qString["TopicId"] : null;

				string sid = qString.ContainsKey("sid") ? qString["sid"] : null;

				string user = qString.ContainsKey("User") ? qString["User"] : null;

				if (user != null)
				{
					VCSCreator.UserName = user;
				}

				string userId = qString.ContainsKey("UserId") ? qString["UserId"] : null;

				if (userId != null)
				{
					VCSCreator.UserId = userId;
				}

				VCSCreator.CreateSLOFromCollaborativeSession(MainFrame.Navigate, qString["DS"], qString["Id"], thread, sid, false);

				return true;
			}

			return false;
		}
	}
}
