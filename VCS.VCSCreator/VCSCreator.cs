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
using VCS.ConversionService;
using System.Runtime.Serialization;
using System.Collections.Generic;
using VCS.ActivityLogService;
using VCS.SLORepositoryService;

namespace VCS
{
	public partial class VCSCreator
	{
		public static string UserIp
		{
			get
			{
				if (VCSCreator.Current.Resources.Contains("ip"))
				{
					return VCSCreator.Current.Resources["ip"].ToString();
				}

				return "";
			}
		}

		public static string UserName { get; set; }

		public static string UserId { get; set; }
		
		public static ConversionServiceClient ConversionServiceClient
		{
			get
			{
				return new ConversionServiceClient("BasicHttpBinding_IConversionService", Config.ConversionServiceUrl);
			}
		}

		public static SLORepositoryServiceClient SLORepositoryServiceClient
		{
			get
			{
				return new SLORepositoryServiceClient("BasicHttpBinding_ISLORepositoryService", Config.SLORepositoryServiceUrl);
			}
		}

		internal static void CreateSLOFromCollaborativeSession(Func<Uri, bool> navigate, string dataSource, string id, string thread = null, string sid = null, bool askName = true)
		{
			ActivityLogger.LogActivity(VCSCreator.UserIp, VCSCreator.UserName, "VCSCreator_SelectCollaborativeSession"
				, new List<Argument>()	{	
													new Argument() { Key = "DataSource", Value = dataSource },
													new Argument() { Key = "id", Value = id.ToString() },
													new Argument() { Key = "thread", Value = (thread != null ? thread : "") }
												});

			//ConversionServiceClient converter = VCSCreator.ConversionServiceClient;

			//converter.GetCollaborativeSessionByIdCompleted += (o, ea) =>
			//{
			try
			{
				//CreateSLOPage.CollaborativeSession = ea.Result;
				CreateSLOPage.CollaborativeSessionId = id;
				CreateSLOPage.CollaborativeSessionThreadId = thread;
				CreateSLOPage.SecurityToken = sid;
				CreateSLOPage.CollaborativeSessionSource = dataSource;

				if (askName)
				{
					navigate(Pages.CreateSLOPageUri);
				}
				else
				{
					CreateSLOPage.CreateSLOByDefault();
				}
			}
			catch (Exception ex)
			{
				ExceptionHandler.HandleException(ex);
			}
			//};

			//converter.GetCollaborativeSessionByIdAsync(id, dataSource);
		}
	}
}
