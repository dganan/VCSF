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
				SLOEditor.UserInfo.Name = user;
			}

			string userId = qString.ContainsKey("UserId") ? qString["UserId"] : null;

			if (userId != null)
			{
				SLOEditor.UserInfo.Name = userId;
				SLOEditor.UserInfo.Id = userId;
			}

			string embedded = qString.ContainsKey("embedded") ? qString["embedded"] : null;

			if (embedded != null)
			{
				SLOEditor.Embedded = true;
			}

			if (qString.ContainsKey("SLOId"))
			{
				SLOEditor.EditSLOWithId(MainFrame.Navigate, qString["SLOId"]); 
			}
			else
			{
				MainFrame.Navigate(Pages.AvailableSLOsListPage);
			}
		}
	}
}
