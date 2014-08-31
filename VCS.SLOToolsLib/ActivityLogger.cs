using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using VCS.ActivityLogService;
using System.Collections.Generic;

namespace VCS
{
	public static class ActivityLogger
	{
		public static void LogActivity(string ip, string userName, string action, List<Argument> args = null)
		{
			if (Config.LogActivity)
			{
				ActivityLogServiceClient logService = Config.ActivityLogServiceClient;

				logService.LogActivityAsync(BuildActivity(ip, userName, action, args));
			}
		}

		private static Activity BuildActivity(string ip, string userName, string action, List<Argument> args = null)
		{
			Activity a = new Activity();

			a.IP = ip;
			a.UserName = userName;
			a.Action = action;
			a.Arguments = args;
			a.Date = DateTime.UtcNow;

			return a;
		}
	}
}
