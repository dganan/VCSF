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
using System.Threading;
using System.Windows.Threading;
using VCS.SpeechService;
using VCS.LearningModelServices;
using System.Collections.ObjectModel;

namespace VCS
{
	public partial class AssessmentScenePlayer : ScenePlayer
	{
		private bool playing;
		private int currentQuestion;
		private AssessmentSceneQuestionPlayer currentAssessmentSceneQuestionPlayer;

		private AssessmentScene AssessmentScene
		{
			get
			{
				return Scene as AssessmentScene;
			}
		}

		public AssessmentScenePlayer()
		{
			InitializeComponent();
			
			currentQuestion = -1;
		}

		public override void Play()
		{
			SetupCurrentQuestion();
		}

		public override void Stop()
		{
			if (currentAssessmentSceneQuestionPlayer != null)
			{
				playing = false;

				currentAssessmentSceneQuestionPlayer.Stop();

				currentQuestion = -1;
			}
		}

		public override void Pause()
		{
			if (currentAssessmentSceneQuestionPlayer != null)
			{
				playing = false;

				// It stops the current scenepart. If play is clicked again, the scenepart will be rendered from the begining.
				currentAssessmentSceneQuestionPlayer.Stop();
			}
		}

		public override void Next()
		{
			//if (playing && currentAssessmentSceneQuestionPlayer != null)
			//{
			//    playing = false;

			//    currentAssessmentSceneQuestionPlayer.Stop();

			//    currentQuestion++;

			//    SetupCurrentQuestion();
			//}
		}

		public override void FastNext()
		{
			//Next();
		}

		public override void Previous()
		{
			//if (playing && currentAssessmentSceneQuestionPlayer != null)
			//{
			//    playing = false;

			//    currentAssessmentSceneQuestionPlayer.Stop();

			//    currentQuestion--;

			//    SetupCurrentQuestion();
			//}
		}

		public override string Title
		{
			set
			{
				TitleText.Text = value;
			}
		}

		public override string SubTitle
		{
			set
			{
				SubTitleText.Text = value;
			}
		}

		public UserAssessmentInfo UserAssessmentInfo { get; private set; }

		public override Scene Scene
		{
			get
			{
				return base.Scene;
			}

			set
			{
				base.Scene = value;

				UserAssessmentInfo = new UserAssessmentInfo();

				UserAssessmentInfo.Scene = value.Order;
				
				currentQuestion = 0;

				nextSceneToLoad = 0;

				new Thread(new ThreadStart(SpeechProcess)).Start();
			}
		}

		void SetupCurrentQuestion()
		{
			if (!this.Dispatcher.CheckAccess())
			{
				this.Dispatcher.BeginInvoke(new Action(
					() =>
					{
						SetupCurrentQuestion();
					}));
			}
			else
			{
				LayoutRoot.Children.Clear();

				if (currentQuestion >= 0 && currentQuestion < AssessmentScene.Questions.Count)
				{
					if (AssessmentScene.Questions[currentQuestion].SpeechAudio == null)
					{
						lock (lockObj)
						{
							nextSceneToLoad = currentQuestion;
						}

						timer = new DispatcherTimer();
						//timer.Interval = new TimeSpan(0, (int)(duration / 60), (int)(duration % 60));
						timer.Interval = new TimeSpan(0, 0, 0, 0, 1000);

						timer.Tick += new EventHandler(timer_Tick);

						timer.Start();
					}
					else
					{
						currentAssessmentSceneQuestionPlayer = new AssessmentSceneQuestionPlayer();

						LayoutRoot.Children.Add(currentAssessmentSceneQuestionPlayer);

						currentAssessmentSceneQuestionPlayer.ScenePart = AssessmentScene.Questions[currentQuestion];

						currentAssessmentSceneQuestionPlayer.PlayEnded += new PlayEventHandler(currentAssessmentSceneQuestionPlayer_PlayEnded);

						playing = true;
					}
				}
				else
				{
					// timer.Stop();

					if (currentQuestion < 0)
					{
						OnPlayPrevious();
					}
					else
					{
						EndTest();
					}

					currentQuestion = -1;
				}
			}
		}

		private void EndTest()
		{
			// Score test

			double score = 0;

			foreach (QuestionResult qr in UserAssessmentInfo.QuestionsResults)
			{
				if (qr.IsCorrectAnswer)
				{
					score += AssessmentScene.Questions.Where (x=>x.Id == qr.QuestionId).First ().Score;
				}
			}

			score = score / AssessmentScene.Questions.Sum (x=>x.Score);

			UserAssessmentInfo.Score = score;

			UserAssessmentInfo.When = DateTime.Now;

			SLOPlayer.UserInfo.AssessmentInfo.Add(UserAssessmentInfo);

			// IWT INTEGRATION

			if (SLOPlayer.UserInfo.CourseId != null && SLOPlayer.UserInfo.Id != null && SLOPlayer.Taxons != null)
			{
				LearningModelServices.LearningModelServicesSoapClient learningModelService = SLOPlayer.LearningModelServicesClient;

				var taxonLevelList = new ObservableCollection<TaxonLevelValue> ();

				foreach (int taxon in SLOPlayer.Taxons)
				{
					taxonLevelList.Add(new TaxonLevelValue() { TaxonID = taxon,  Level = (score * 10) });
				}

				learningModelService.SetUserCognitiveStateCompleted += new EventHandler<SetUserCognitiveStateCompletedEventArgs>((o, ea) => {

					if (ea.Error != null)
					{
						string s = ea.Error.Message;

						MessageBox.Show(s);
					}
				});

				learningModelService.SetUserCognitiveStateAsync(Int32.Parse(SLOPlayer.UserInfo.Id), taxonLevelList);
			}

			// IWT INTEGRATION END

			int testAttempt = SLOPlayer.UserInfo.AssessmentInfo.Where(x => x.Scene == UserAssessmentInfo.Scene).Count();

			AssessmentJumpRule ruleToApply = AssessmentScene.JumpRules.Where (x=> x.IsSatisfied(score, testAttempt, SLOPlayer.UserInfo.MeetsPrerequisites)).FirstOrDefault();

			ActivityLogger.LogActivity(SLOPlayer.UserInfo.Ip, SLOPlayer.UserInfo.Name, String.Format ("Test completed. Score: {0}, Attempts: {1}, Previes: {2}", score, testAttempt, SLOPlayer.UserInfo.MeetsPrerequisites));

			if (ruleToApply != null)
			{
				// give feedback to the user about the result of the test
				GiveFeedback(ruleToApply.FeedbackMessage);

				// Decide what to do, and jump to the corresponding scene
				sceneToJump = ruleToApply.SceneToJump;
				endInsteadOfJump = ruleToApply.End;
			}
			else
			{
				// give feedback to the user about the result of the test
				GiveFeedback(AssessmentScene.DefaultFeedbackMessage);

				// Decide what to do, and jump to the corresponding scene
				sceneToJump = AssessmentScene.DefaultSceneToJump;
				endInsteadOfJump = AssessmentScene.DefaultEnd;
			}

			FeedbackPanel.Visibility = Visibility.Visible;
		}

		Scene sceneToJump;
		bool endInsteadOfJump;

		private void GiveFeedback(string feedbackMessage)
		{
			if (feedbackMessage != null)
			{
				//MessageBox.Show(feedbackMessage, "", MessageBoxButton.OK);
				FeedbackText.Text = feedbackMessage;
			}
			else
			{
				if (UserAssessmentInfo.Score >= 0.5)
				{
					//MessageBox.Show("Congratulations, you passed the test!", "Succeed!", MessageBoxButton.OK);

					if (Config.Language == VCS.Language.Catalan)
					{
						FeedbackText.Text = "Felicitats, has passat el test!";
					}
					else
					{
						FeedbackText.Text = "Congratulations, you passed the test!";
					}
				}
				else
				{
					//MessageBox.Show("Sorry, you failed the test", "Failed!", MessageBoxButton.OK);

					if (Config.Language == VCS.Language.Catalan)
					{
						FeedbackText.Text = "Ho sento, has fallat el test";
					}
					else
					{
						FeedbackText.Text = "Sorry, you failed the test";
					}
				}
			}
		}

		void currentAssessmentSceneQuestionPlayer_PlayEnded()
		{
			playing = false;

			UserAssessmentInfo.QuestionsResults.Add(currentAssessmentSceneQuestionPlayer.QuestionResult);

			currentQuestion++;

			SetupCurrentQuestion();
		}

		private DispatcherTimer timer;

		void timer_Tick(object sender, EventArgs e)
		{
			new Thread(new ThreadStart(SetupCurrentQuestion)).Start();

			timer.Stop();
		}

		public struct SpeechParam
		{
			public int scene;
			public bool loop;
		}

		private int nextSceneToLoad;
		private object lockObj = new object();

		private void SpeechProcess()
		{
			int sceneToLoad;

			lock (lockObj)
			{
				sceneToLoad = nextSceneToLoad;
				nextSceneToLoad++;

				if (nextSceneToLoad >= AssessmentScene.Questions.Count)
				{
					// Si arribem al final, tornem al principi per si queda alguna escena per baixar
					nextSceneToLoad = 0;
				}
			}

			if (sceneToLoad < 0 || sceneToLoad >= AssessmentScene.Questions.Count)
			{

			}
			else
			{
				AssessmentSceneQuestion asq = AssessmentScene.Questions[sceneToLoad];

				if (asq.SpeechAudio == null)
				{
					string textToSpeech = asq.Speech;

					textToSpeech = textToSpeech.RemoveHTMLTags();

					SpeechServiceClient client = SLOPlayer.SpeechServiceClient;

					client.SpeakCompleted += (o, ea) =>
					{
						AssessmentScene.Questions[sceneToLoad].SpeechAudio = Utils.DecompressBytes(ea.Result);

						new Thread(new ThreadStart(SpeechProcess)).Start();
					};

					client.SpeakAsync(textToSpeech, asq.Evaluator.Gender, asq.Evaluator.Age, VCS.Language.Unknown);
				}
				else
				{
					// Si l'escena actual per carregar ja te audio, es programa la seguent iteració del SpeechProcess a menys que totes les 
					// escenes ja tinguin audio carregat. En aquest ultim cas, s'aborta el thread de càrrega (no es torna a cridar)

					if (AssessmentScene.Questions.Any(x => x.SpeechAudio == null))
					{
						new Thread(new ThreadStart(SpeechProcess)).Start();
					}
				}
			}
		}

		public override bool IsFastNextEnabled { get { return false; } }

		public override bool IsNextEnabled { get { return false; } }

		public override bool IsPreviousEnabled { get { return false; } }

		private void FeedbackButton_Click(object sender, RoutedEventArgs e)
		{
			OnJumpToScene(sceneToJump, endInsteadOfJump);
		}
	}
}
