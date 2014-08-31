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

namespace VCS
{
	public static class Config
	{
		//internal static bool yaren = true;
		//internal static string specialVersion = null; // Default
		//internal static string specialVersion = "Dev"; // Development
		//internal static string specialVersion = "22"; // Prova 22 Maig
		//internal static string specialVersion = "TestingTUG"; // Prova TUG 
		//internal static string specialVersion = "TestingCOVUNI"; // Prova COVUNI
		//internal static string specialVersion = "TestingMOMA"; // Prova MOMA

		//internal static Language language = Language.Catalan; // Prova 22 Maig
        //internal static Language language = Language.English; // Other
        internal static Language language = Language.Spanish; // Other
		
		//internal static string BaseUri
		//{
		//    get
		//    {
		//        if (yaren)
		//        {
		//            return "http://yaren.uoc.edu";
		//        }
		//        else
		//        {
		//            return "http://localhost";
		//        }
		//    }
		//}

		public static string ServicesHost { get; set; }

		//internal static string BaseApplication
		//{
		//    get
		//    {
		//        if (specialVersion != null && yaren)
		//        {
		//            return "VCS.Services" + specialVersion;
		//        }
		//        else
		//        {
		//            return "VCS.Services";
		//        }
		//    }
		//}

		public static string ConversionServiceUrl
		{
			get
			{
				return ServicesHost + "/ConversionService/ConversionService.svc";
			}
		}

		public static string SLORepositoryServiceUrl
		{
			get
			{
				return ServicesHost + "/SLORepositoryService/SLORepositoryService.svc";
			}
		}

		public static string VideosUrl
		{
			get { return ServicesHost + "/videos/"; }
		}

		public static string SpeechServiceUrl
		{
			get
			{
				return ServicesHost + "/SpeechService/SpeechService.svc";
			}
		}

		public static string ActivityLogServiceUrl
		{
			get
			{
				return ServicesHost + "/ActivityLogService/ActivityLogService.svc";
			}
		}

		public static string CategoriesRepositoryServiceUrl 
		{
			get
			{
				return ServicesHost + "/CategoriesRepositoryService/CategoriesRepositoryService.svc";
			}
		}

		public static string KeywordsRepositoryServiceUrl
		{
			get
			{
				return ServicesHost + "/KeywordsRepositoryService/KeywordsRepositoryService.svc";
			}
		}

		public static string SpeechActClassificationServiceUrl
		{
			get
			{
				return ServicesHost + "/SpeechActClassificationService/SpeechActClassificationService.svc";
			}
		}

		public static ActivityLogServiceClient ActivityLogServiceClient
		{
			get
			{
				return new ActivityLogServiceClient("BasicHttpBinding_IActivityLogService", Config.ActivityLogServiceUrl);
			}
		}

		public static bool LogActivity
		{
			get { return true; }
		}

		public static Language Language
		{
			get { return language; }
		}

		public static string AffectiveServiceUrl { get; set; }

		public static string LearnerModelServiceUrl { get; set; }
	}
}
