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

namespace VCS
{
	public partial class SLOEditor : Application
	{
		public SLOEditor()
		{
			UserInfo = new UserInfo();

			UserInfo.Name = "Anonymous";

			this.Startup += this.Application_Startup;
			this.Exit += this.Application_Exit;
			this.UnhandledException += this.Application_UnhandledException;

			InitializeComponent();
		}

		private void Application_Startup(object sender, StartupEventArgs e)
		{
			if (e.InitParams != null)
			{
				foreach (var data in e.InitParams)
					this.Resources.Add(data.Key, data.Value);
			}

			if (SLOEditor.Current.Resources.Contains("ip"))
			{
				UserInfo.Ip = SLOEditor.Current.Resources["ip"].ToString();
			}

			if (SLOEditor.Current.Resources.Contains("serviceshost"))
			{
				Config.ServicesHost = SLOEditor.Current.Resources["serviceshost"].ToString();
			}

			ActivityLogger.LogActivity(UserInfo.Ip, UserInfo.Name, "SLOEditor_Startup");
		
			this.RootVisual = new MainPage();
		}

		private void Application_Exit(object sender, EventArgs e)
		{
			ActivityLogger.LogActivity(UserInfo.Ip, UserInfo.Name, "SLOEditor_Exit");
		}

		private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
		{
			// If the app is running outside of the debugger then report the exception using
			// the browser's exception mechanism. On IE this will display it a yellow alert 
			// icon in the status bar and Firefox will display a script error.
			if (!System.Diagnostics.Debugger.IsAttached)
			{

				// NOTE: This will allow the application to continue running after an exception has been thrown
				// but not handled. 
				// For production applications this error handling should be replaced with something that will 
				// report the error to the website and stop the application.
				e.Handled = true;
				Deployment.Current.Dispatcher.BeginInvoke(delegate { ReportErrorToDOM(e); });
			}
		}

		private void ReportErrorToDOM(ApplicationUnhandledExceptionEventArgs e)
		{
			try
			{
				string errorMsg = e.ExceptionObject.Message + e.ExceptionObject.StackTrace;
				errorMsg = errorMsg.Replace('"', '\'').Replace("\r\n", @"\n");

				System.Windows.Browser.HtmlPage.Window.Eval("throw new Error(\"Unhandled Error in Silverlight Application " + errorMsg + "\");");
			}
			catch (Exception)
			{
			}
		}
	}
}
