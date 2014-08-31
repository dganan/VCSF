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
using System.Windows.Navigation;
using VCS.KeywordsRepositoryService;
using VCS.SLORepositoryService;
using VCS.SpeechActClassificationService;

namespace VCS
{
	public partial class SLOEditor : Application
	{
		public static UserInfo UserInfo { get; set; }

		internal static void EditSLOWithId(Func<Uri, bool> navigate, string id)
		{
			SLORepositoryServiceClient sloRepository = SLOEditor.SLORepositoryServiceClient;

			sloRepository.GetSLOByIdCompleted += (o, ea) =>
			{
				try
				{
					StoryBoardEditor.EditingSLO = ea.Result;

					navigate(Pages.StoryBoardEditorPage);
				}
				catch (Exception ex)
				{
					ExceptionHandler.HandleException(ex);
				}
			};

			sloRepository.GetSLOByIdAsync(id, SLOEditor.UserInfo.Id);
		}

		internal static void CreateAndEditSLO(Func<Uri, bool> navigate)
		{
			StoryBoardEditor.EditingSLO = new SLO();

			navigate(Pages.StoryBoardEditorPage);
		}

		public static SLORepositoryServiceClient SLORepositoryServiceClient
		{
			get
			{
				return new SLORepositoryServiceClient("BasicHttpBinding_ISLORepositoryService", Config.SLORepositoryServiceUrl);
			}
		}

		public static KeywordsRepositoryServiceClient KeywordsRepositoryServiceClient
		{
			get
			{
				return new KeywordsRepositoryServiceClient("BasicHttpBinding_IKeywordsRepositoryService", Config.KeywordsRepositoryServiceUrl);
			}
		}

		public static SpeechActClassificationServiceClient SpeechActClassificationServiceClient
		{
			get
			{
				return new SpeechActClassificationServiceClient("BasicHttpBinding_ISpeechActClassificationService", Config.SpeechActClassificationServiceUrl);
			}
		}

		public static bool Embedded { get; set; }
	}
}
