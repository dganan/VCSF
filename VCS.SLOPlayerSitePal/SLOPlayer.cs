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
using VCS.SLORepositoryService;
using VCS.SpeechService;
using System.Windows.Browser;
using VCS.AffectiveEmotiveServices;
using VCS.LearningModelServices;

namespace VCS
{
	public partial class SLOPlayer : Application
	{
		public static UserInfo UserInfo { get; set; }

		public static int[] Taxons { get; set; }

		internal static void PlaySLOWithId(Func<Uri, bool> navigate, string id, Panel mainPanel = null)
		{
			SLORepositoryServiceClient sloRepository = SLOPlayer.SLORepositoryServiceClient;

			sloRepository.GetSLOByIdCompleted += (o, ea) =>
			{
				try
				{
					StoryBoardPlayerPage.SLOToPlay = ea.Result;
					StoryBoardPlayerPage.ForceStart = true;

					navigate(Pages.StoryBoardPlayerPageUri);

					if (mainPanel != null)
					{
						mainPanel.Visibility = System.Windows.Visibility.Visible;
					}
				}
				catch (Exception ex)
				{
					ExceptionHandler.HandleException(ex);
				}
			};

			sloRepository.GetSLOByIdAsync(id, UserInfo.Id);
		}

		internal static void OpenSourceSLOWithId(string id)
		{
			SLORepositoryServiceClient sloRepository = SLOPlayer.SLORepositoryServiceClient;

			sloRepository.GetSLOByIdCompleted += (o, ea) =>
			{
				try
				{
					if (!String.IsNullOrWhiteSpace (ea.Result.SourceUrl))
					{
						HtmlPage.Window.Navigate(new Uri (ea.Result.SourceUrl), "_blank");
					}
					else
					{
						MessageBox.Show("The url of the source collaborative session for this SLO is not available");
					}
				}
				catch (Exception ex)
				{
					ExceptionHandler.HandleException(ex);
				}
			};

			sloRepository.GetSLOByIdAsync(id, UserInfo.Id);
		}

		public static SLORepositoryServiceClient SLORepositoryServiceClient
		{
			get
			{
				return new SLORepositoryServiceClient("BasicHttpBinding_ISLORepositoryService", Config.SLORepositoryServiceUrl);
			}
		}

		public static SpeechServiceClient SpeechServiceClient
		{
			get
			{
				return new SpeechServiceClient("BasicHttpBinding_ISpeechService", Config.SpeechServiceUrl);
			}
		}

		public static AffectiveEmotiveServicesSoapClient AffectiveEmotiveServicesClient
		{
			get
			{
				return new AffectiveEmotiveServices.AffectiveEmotiveServicesSoapClient("AffectiveEmotiveServicesSoap", Config.AffectiveServiceUrl);
			}
		}

		public static LearningModelServicesSoapClient LearningModelServicesClient
		{
			get
			{
				return new LearningModelServices.LearningModelServicesSoapClient("LearningModelServicesSoap", Config.LearnerModelServiceUrl);
			}
		}

		public static AffectiveEmotiveServices.EmotionalStateValue IWTEmotionalInfo { get; set; }

		private static Action _sayTextStartedAction;
		private static Action _sayTextEndedAction;

		internal static void SayText(Character character, Language language, string text, Action sayTextStartedAction, Action sayTextEndedAction)
		{
			_sayTextStartedAction = sayTextStartedAction;
			_sayTextEndedAction = sayTextEndedAction;

			text = text.EncodeHTMLSpecialChars();

			SitePalVoice voice = SitePalVoice.SitePalVoiceForCharacter (character, language);

			HtmlPage.Window.Invoke("sayTextSitePal", new string[] { text, voice.Voice, voice.Language, voice.Engine });
		}

		internal static void StopSayText(Action sayTextEndedAction)
		{
			_sayTextEndedAction = sayTextEndedAction;
		
			HtmlPage.Window.Invoke("stopSayTextSitePal", new string[] { });
		}
		

		[ScriptableMember]
		public void SayTextStarted()
		{
			if (_sayTextStartedAction != null)
			{
				_sayTextStartedAction();
			}

			_sayTextStartedAction = null;
		}

		[ScriptableMember]
		public void SayTextEnded()
		{
			if (_sayTextEndedAction != null)
			{
				Action actionToRun = _sayTextEndedAction;
				_sayTextEndedAction = null;

				actionToRun();
			}
		}

		public static bool EngineReady = false;

		public static Action EngineReadyAction;

		[ScriptableMember]
		public void EngineLoaded()
		{
			EngineReady = true;

			if (EngineReadyAction != null)
			{
				EngineReadyAction();

				EngineReadyAction = null;
			}
		}
	}
}
