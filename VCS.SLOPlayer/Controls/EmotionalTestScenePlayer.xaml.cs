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

namespace VCS
{
	public delegate void EmotionSelectedHandler(EmotionalState emotionalState);

	public delegate void MultipleEmotionSelectedHandler(List<EmotionalState> emotionalState);

	public partial class EmotionalTestScenePlayer : ScenePlayer
	{
		public EmotionalTestScenePlayer()
		{
			InitializeComponent();
		}

		public override Scene Scene
		{
			get
			{
				return base.Scene;
			}

			set
			{
				base.Scene = value;

				EmotionalTestScene scene = Scene as EmotionalTestScene;

				switch (scene.EmotionalTestType)
				{
					case EmotionalTestType.Geneva_Wheel:

						GenevaEmotionWheelControl genevaEmotionWheel = new GenevaEmotionWheelControl();

						genevaEmotionWheel.EmotionSelected += new EmotionSelectedHandler(GenevaWheelOptionSelected);

						if (!String.IsNullOrWhiteSpace(scene.GenevaWheelQuestion))
						{
							SubTitleText.Text = scene.GenevaWheelQuestion;
						}
						else
						{
							SubTitleText.Text = "Select your current emotional state and intensity:";
						}

						LayoutRoot.Children.Add(genevaEmotionWheel);

						break;

					case EmotionalTestType.Mood:

						MoodSelectionControl moodSelection = new MoodSelectionControl();

						moodSelection.EmotionSelected += new EmotionSelectedHandler(MoodSelected);

						if (!String.IsNullOrWhiteSpace(scene.MoodQuestion))
						{
							moodSelection.MoodQuestion = scene.MoodQuestion;
						}

						LayoutRoot.Children.Add(moodSelection);

						break;

					case EmotionalTestType.Emotional:

						EmotionSelectionControl emotionSelection = new EmotionSelectionControl();

						emotionSelection.EmotionsSelected += new MultipleEmotionSelectedHandler(EmotionsSelected);

						if (!String.IsNullOrWhiteSpace(scene.EmotionFeelNowQuestion))
						{
							emotionSelection.FeelNowQuestion = scene.EmotionFeelNowQuestion;
						}

						if (!String.IsNullOrWhiteSpace(scene.EmotionFeelDuringQuestion))
						{
							emotionSelection.FeelDuringQuestion = scene.EmotionFeelDuringQuestion;
						}

						if (!String.IsNullOrWhiteSpace(scene.EmotionFeelExperiencedQuestion))
						{
							emotionSelection.FeelExperiencedQuestion = scene.EmotionFeelExperiencedQuestion;
						}

						LayoutRoot.Children.Add(emotionSelection);

						break;
				}
			}
		}

		public override void Play()
		{
		}

		public override void Stop()
		{
			OnPlayEnded();
		}

		public override void Pause()
		{

		}

		public override void Next()
		{
			OnPlayNext();
		}

		public override void FastNext()
		{
			OnPlayNext();
		}

		public override void Previous()
		{
			OnPlayPrevious();
		}

		public override string Title { set { TitleText.Text = value; } }

		public override string SubTitle { set { } }

		private void GenevaWheelOptionSelected(EmotionalState ee)
		{
			// Do whatever necessary with the selected emotion and intensity level

			//string intensity = "";

			//if (eea.Intensity == 0)
			//{
			//    intensity = "very ";
			//}
			//else if (eea.Intensity == 1)
			//{
			//    intensity = "quite ";
			//}
			//else if (eea.Intensity == 2)
			//{
			//    intensity = "";
			//}
			//else if (eea.Intensity == 3)
			//{
			//    intensity = "little ";
			//}

			//MessageBox.Show("You are " + intensity + eea.Emotion.ToString().ToLower());

			SLOPlayer.UserInfo.EmotionalInfo.Add(new UserEmotionalInfo() { EmotionalState = ee, Scene = this.Scene.Order, When = DateTime.Now });

			// Go to next scene
			OnPlayNext();
		}

		private void MoodSelected(EmotionalState ee)
		{
			// Do whatever necessary with the selected mood

			//MessageBox.Show("You are " + eea.Emotion.ToString().ToLower());

			SLOPlayer.UserInfo.EmotionalInfo.Add(new UserEmotionalInfo() { EmotionalState = ee, Scene = this.Scene.Order, When = DateTime.Now });

			// Go to next scene
			OnPlayNext();
		}

		private void EmotionsSelected(List<EmotionalState> eel)
		{
			// Do whatever necessary with the selected emotion

			//MessageBox.Show("You are " + eea.Emotion.ToString().ToLower());

			foreach (EmotionalState ee in eel)
			{
				SLOPlayer.UserInfo.EmotionalInfo.Add(new UserEmotionalInfo() { EmotionalState = ee, Scene = this.Scene.Order, When = DateTime.Now });
			}

			EmotionState es = CalculateEmotionStateFromTestResults(eel);


			// IWT INTEGRATION 

			if (SLOPlayer.UserInfo.CourseId != null && SLOPlayer.UserInfo.Id != null && SLOPlayer.IWTEmotionalInfo != null)
			{
				AffectiveEmotiveServices.AffectiveEmotiveServicesSoapClient affectiveEmotiveService = SLOPlayer.AffectiveEmotiveServicesClient;

				AffectiveEmotiveServices.EmotionalStateValue emotionalState = SLOPlayer.IWTEmotionalInfo;

				emotionalState.FidAns += EmitonalConversion_Matrix.Where(x => x.Item1 == es && x.Item2 == IWTEmotionalState.FIDANS).Select(x => x.Item3).FirstOrDefault();
				emotionalState.EccInd += EmitonalConversion_Matrix.Where(x => x.Item1 == es && x.Item2 == IWTEmotionalState.ECCIND).Select(x => x.Item3).FirstOrDefault();
				emotionalState.IntDis += EmitonalConversion_Matrix.Where(x => x.Item1 == es && x.Item2 == IWTEmotionalState.INTDIS).Select(x => x.Item3).FirstOrDefault();
				emotionalState.AutFru += EmitonalConversion_Matrix.Where(x => x.Item1 == es && x.Item2 == IWTEmotionalState.AUTFRU).Select(x => x.Item3).FirstOrDefault();

				//// Invert punctuation (substract) because IWT handles punctuation the reverse way
				//emotionalState.FidAns -= EmitonalConversion_Matrix.Where(x => x.Item1 == es && x.Item2 == IWTEmotionalState.FIDANS).Select(x => x.Item3).FirstOrDefault();
				//emotionalState.EccInd -= EmitonalConversion_Matrix.Where(x => x.Item1 == es && x.Item2 == IWTEmotionalState.ECCIND).Select(x => x.Item3).FirstOrDefault();
				//emotionalState.IntDis -= EmitonalConversion_Matrix.Where(x => x.Item1 == es && x.Item2 == IWTEmotionalState.INTDIS).Select(x => x.Item3).FirstOrDefault();
				//emotionalState.AutFru -= EmitonalConversion_Matrix.Where(x => x.Item1 == es && x.Item2 == IWTEmotionalState.AUTFRU).Select(x => x.Item3).FirstOrDefault();

				if (emotionalState.FidAns > 1) emotionalState.FidAns = 1;
				if (emotionalState.FidAns < -1) emotionalState.FidAns = -1;

				if (emotionalState.EccInd > 1) emotionalState.EccInd = 1;
				if (emotionalState.EccInd < -1) emotionalState.EccInd = -1;

				if (emotionalState.IntDis > 1) emotionalState.IntDis = 1;
				if (emotionalState.IntDis < -1) emotionalState.IntDis = -1;

				if (emotionalState.AutFru > 1) emotionalState.AutFru = 1;
				if (emotionalState.AutFru < -1) emotionalState.AutFru = -1;

				affectiveEmotiveService.SetUserEmotionalStateAsync(Int32.Parse(SLOPlayer.UserInfo.Id), Int32.Parse(SLOPlayer.UserInfo.CourseId), emotionalState);
			}

			// IWT INTEGRATION END

			ShowFeedback(es);
		}

		public enum IWTEmotionalState
		{
			FIDANS,
			ECCIND,
			INTDIS,
			AUTFRU,
		}

		private static List<Tuple<EmotionState, IWTEmotionalState, double>> EmitonalConversion_Matrix = new List<Tuple<EmotionState, IWTEmotionalState, double>>()
		{
			new Tuple<EmotionState, IWTEmotionalState, double> (EmotionState.Flow, IWTEmotionalState.FIDANS, 0.2),
			new Tuple<EmotionState, IWTEmotionalState, double> (EmotionState.Flow, IWTEmotionalState.ECCIND, 0.2),
			new Tuple<EmotionState, IWTEmotionalState, double> (EmotionState.Flow, IWTEmotionalState.INTDIS, 0.2),
			new Tuple<EmotionState, IWTEmotionalState, double> (EmotionState.Flow, IWTEmotionalState.AUTFRU, 0.2),

			new Tuple<EmotionState, IWTEmotionalState, double> (EmotionState.Enjoyment, IWTEmotionalState.FIDANS, 0.1),
			new Tuple<EmotionState, IWTEmotionalState, double> (EmotionState.Enjoyment, IWTEmotionalState.ECCIND, 0.1),
			new Tuple<EmotionState, IWTEmotionalState, double> (EmotionState.Enjoyment, IWTEmotionalState.INTDIS, 0.2),
			new Tuple<EmotionState, IWTEmotionalState, double> (EmotionState.Enjoyment, IWTEmotionalState.AUTFRU, 0.2),

			new Tuple<EmotionState, IWTEmotionalState, double> (EmotionState.Relaxation, IWTEmotionalState.FIDANS, 0.2),
			new Tuple<EmotionState, IWTEmotionalState, double> (EmotionState.Relaxation, IWTEmotionalState.ECCIND, 0),
			new Tuple<EmotionState, IWTEmotionalState, double> (EmotionState.Relaxation, IWTEmotionalState.INTDIS, 0),
			new Tuple<EmotionState, IWTEmotionalState, double> (EmotionState.Relaxation, IWTEmotionalState.AUTFRU, 0.2),

			new Tuple<EmotionState, IWTEmotionalState, double> (EmotionState.Puzzlement, IWTEmotionalState.FIDANS, 0),
			new Tuple<EmotionState, IWTEmotionalState, double> (EmotionState.Puzzlement, IWTEmotionalState.ECCIND, 0.1),
			new Tuple<EmotionState, IWTEmotionalState, double> (EmotionState.Puzzlement, IWTEmotionalState.INTDIS, 0.1),
			new Tuple<EmotionState, IWTEmotionalState, double> (EmotionState.Puzzlement, IWTEmotionalState.AUTFRU, 0),

			new Tuple<EmotionState, IWTEmotionalState, double> (EmotionState.Mixed, IWTEmotionalState.FIDANS, 0),
			new Tuple<EmotionState, IWTEmotionalState, double> (EmotionState.Mixed, IWTEmotionalState.ECCIND, 0),
			new Tuple<EmotionState, IWTEmotionalState, double> (EmotionState.Mixed, IWTEmotionalState.INTDIS, 0),
			new Tuple<EmotionState, IWTEmotionalState, double> (EmotionState.Mixed, IWTEmotionalState.AUTFRU, 0),

			new Tuple<EmotionState, IWTEmotionalState, double> (EmotionState.Confussion, IWTEmotionalState.FIDANS, -0.3),
			new Tuple<EmotionState, IWTEmotionalState, double> (EmotionState.Confussion, IWTEmotionalState.ECCIND, -0.1),
			new Tuple<EmotionState, IWTEmotionalState, double> (EmotionState.Confussion, IWTEmotionalState.INTDIS, -0.1),
			new Tuple<EmotionState, IWTEmotionalState, double> (EmotionState.Confussion, IWTEmotionalState.AUTFRU, -0.1),

			new Tuple<EmotionState, IWTEmotionalState, double> (EmotionState.Disinterest, IWTEmotionalState.FIDANS, -0.1),
			new Tuple<EmotionState, IWTEmotionalState, double> (EmotionState.Disinterest, IWTEmotionalState.ECCIND, -0.2),
			new Tuple<EmotionState, IWTEmotionalState, double> (EmotionState.Disinterest, IWTEmotionalState.INTDIS, -0.1),
			new Tuple<EmotionState, IWTEmotionalState, double> (EmotionState.Disinterest, IWTEmotionalState.AUTFRU, -0),

			new Tuple<EmotionState, IWTEmotionalState, double> (EmotionState.Fatigue, IWTEmotionalState.FIDANS, -0.2),
			new Tuple<EmotionState, IWTEmotionalState, double> (EmotionState.Fatigue, IWTEmotionalState.ECCIND, -0.2),
			new Tuple<EmotionState, IWTEmotionalState, double> (EmotionState.Fatigue, IWTEmotionalState.INTDIS, -0.1),
			new Tuple<EmotionState, IWTEmotionalState, double> (EmotionState.Fatigue, IWTEmotionalState.AUTFRU, -0.1),

			new Tuple<EmotionState, IWTEmotionalState, double> (EmotionState.Boredom, IWTEmotionalState.FIDANS, -0.1),
			new Tuple<EmotionState, IWTEmotionalState, double> (EmotionState.Boredom, IWTEmotionalState.ECCIND, -0.2),
			new Tuple<EmotionState, IWTEmotionalState, double> (EmotionState.Boredom, IWTEmotionalState.INTDIS, -0.3),
			new Tuple<EmotionState, IWTEmotionalState, double> (EmotionState.Boredom, IWTEmotionalState.AUTFRU, -0.1),

			new Tuple<EmotionState, IWTEmotionalState, double> (EmotionState.Sadness, IWTEmotionalState.FIDANS, -0.2),
			new Tuple<EmotionState, IWTEmotionalState, double> (EmotionState.Sadness, IWTEmotionalState.ECCIND, -0.2),
			new Tuple<EmotionState, IWTEmotionalState, double> (EmotionState.Sadness, IWTEmotionalState.INTDIS, -0.2),
			new Tuple<EmotionState, IWTEmotionalState, double> (EmotionState.Sadness, IWTEmotionalState.AUTFRU, -0.2),
		};

		private void ShowFeedback(EmotionState es)
		{
			VideoPlayerControl vpc = new VideoPlayerControl();

			LayoutRoot.Children.Clear();

			LayoutRoot.Children.Add(vpc);

			LayoutRoot.UpdateLayout();

			string uri = null;

			bool showDecissionPanel = true;

			string lang = "_en";

			if (Config.Language == VCS.Language.Catalan)
			{
				lang = "_cat";
			}

			switch (es)
			{
				case EmotionState.Sadness:

					uri = Config.VideosUrl + "sadness" + lang + ".wmv";

					ElseButton.Visibility = Visibility.Visible;

					if (Config.Language == VCS.Language.Catalan)
					{
						ContinueButton.Content = "Continua...";
						ElseButton.Content = "Prova una altra cosa...";
					}

					break;

				case EmotionState.Enjoyment:

					uri = Config.VideosUrl + "enjoyment" + lang + ".wmv";

					if (Config.Language == VCS.Language.Catalan)
					{
						ContinueButton.Content = "Continua...";
					}

					break;

				case EmotionState.Boredom:

					uri = Config.VideosUrl + "boredom" + lang + ".wmv";

					if (Config.Language == VCS.Language.Catalan)
					{
						ContinueButton.Content = "No, estic bé. Segueix";
					}
					else
					{
						ContinueButton.Content = "No, I am fine. Go on";
					}

					break;

				case EmotionState.Confussion:

					uri = Config.VideosUrl + "confussion" + lang + ".wmv";

					if (Config.Language == VCS.Language.Catalan)
					{
						ContinueButton.Content = "D'acord! Continua!";
					}
					else
					{
						ContinueButton.Content = "OK! Continue!";
					}

					break;

				case EmotionState.Flow:

					uri = Config.VideosUrl + "flow" + lang + ".wmv";

					if (Config.Language == VCS.Language.Catalan)
					{
						ContinueButton.Content = "Merci! Continua!";
					}
					else
					{
						ContinueButton.Content = "Thanx! Continue!";
					}

					break;

				case EmotionState.Fatigue:

					uri = Config.VideosUrl + "hopefulness" + lang + ".wmv";

					if (Config.Language == VCS.Language.Catalan)
					{
						ContinueButton.Content = "D'acord! Continua!";
					}
					else
					{
						ContinueButton.Content = "OK! Continue!";
					}

					break;

				case EmotionState.Puzzlement:

					uri = Config.VideosUrl + "puzzlement" + lang + ".wmv";

					if (Config.Language == VCS.Language.Catalan)
					{
						ContinueButton.Content = "D'acord! Continua!";
					}
					else
					{
						ContinueButton.Content = "OK! Continue!";
					}

					break;

				case EmotionState.Relaxation:

					uri = Config.VideosUrl + "relaxation" + lang + ".wmv";

					if (Config.Language == VCS.Language.Catalan)
					{
						ContinueButton.Content = "Continua...";
					}

					break;

				case EmotionState.Mixed:

					uri = Config.VideosUrl + "mixed" + lang + ".wmv";

					if (Config.Language == VCS.Language.Catalan)
					{
						ContinueButton.Content = "Continua...";
					}

					break;

				case EmotionState.Disinterest:

					uri = Config.VideosUrl + "disinterest" + lang + ".wmv";

					if (Config.Language == VCS.Language.Catalan)
					{
						ContinueButton.Content = "D'acord! Continua!";
					}
					else
					{
						ContinueButton.Content = "OK! Continue!";
					}

					break;
			}

			DecisionContainer.Visibility = Visibility.Visible;

			if (!String.IsNullOrWhiteSpace(uri))
			{
				vpc.PlayEnded += new PlayEventHandler(() =>
				{
					if (showDecissionPanel)
					{
						DecisionPanel.Visibility = Visibility.Visible;
					}
					else
					{
						Thread.Sleep(2000);

						// Go to next scene
						OnPlayNext();
					}
				});

				vpc.PlayVideoByUri(uri);
			}
			else
			{
				OnPlayNext();
			}
		}

		private static List<Tuple<Emotion, Emotion, EmotionState>> Trait_MoodT_Matrix = new List<Tuple<Emotion, Emotion, EmotionState>>()
		{
			new Tuple<Emotion, Emotion, EmotionState> (Emotion.Enthusiastic, Emotion.Sad, EmotionState.Mixed),
			new Tuple<Emotion, Emotion, EmotionState> (Emotion.Enthusiastic, Emotion.Unhappy, EmotionState.Mixed),
			new Tuple<Emotion, Emotion, EmotionState> (Emotion.Enthusiastic, Emotion.Neutral, EmotionState.Enjoyment),
			new Tuple<Emotion, Emotion, EmotionState> (Emotion.Enthusiastic, Emotion.Happy, EmotionState.Enjoyment),
			new Tuple<Emotion, Emotion, EmotionState> (Emotion.Enthusiastic, Emotion.Very_Happy, EmotionState.Enjoyment),

			new Tuple<Emotion, Emotion, EmotionState> (Emotion.Fine, Emotion.Sad, EmotionState.Mixed),
			new Tuple<Emotion, Emotion, EmotionState> (Emotion.Fine, Emotion.Unhappy, EmotionState.Mixed),
			new Tuple<Emotion, Emotion, EmotionState> (Emotion.Fine, Emotion.Neutral, EmotionState.Enjoyment),
			new Tuple<Emotion, Emotion, EmotionState> (Emotion.Fine, Emotion.Happy, EmotionState.Enjoyment),
			new Tuple<Emotion, Emotion, EmotionState> (Emotion.Fine, Emotion.Very_Happy, EmotionState.Enjoyment),

			new Tuple<Emotion, Emotion, EmotionState> (Emotion.Relaxed, Emotion.Sad, EmotionState.Sadness),
			new Tuple<Emotion, Emotion, EmotionState> (Emotion.Relaxed, Emotion.Unhappy, EmotionState.Disinterest),
			new Tuple<Emotion, Emotion, EmotionState> (Emotion.Relaxed, Emotion.Neutral, EmotionState.Relaxation),
			new Tuple<Emotion, Emotion, EmotionState> (Emotion.Relaxed, Emotion.Happy, EmotionState.Relaxation),
			new Tuple<Emotion, Emotion, EmotionState> (Emotion.Relaxed, Emotion.Very_Happy, EmotionState.Relaxation),

			new Tuple<Emotion, Emotion, EmotionState> (Emotion.Neutral, Emotion.Sad, EmotionState.Sadness),
			new Tuple<Emotion, Emotion, EmotionState> (Emotion.Neutral, Emotion.Unhappy, EmotionState.Disinterest),
			new Tuple<Emotion, Emotion, EmotionState> (Emotion.Neutral, Emotion.Neutral, EmotionState.Disinterest),
			new Tuple<Emotion, Emotion, EmotionState> (Emotion.Neutral, Emotion.Happy, EmotionState.Disinterest),
			new Tuple<Emotion, Emotion, EmotionState> (Emotion.Neutral, Emotion.Very_Happy, EmotionState.Disinterest),

			new Tuple<Emotion, Emotion, EmotionState> (Emotion.Tired, Emotion.Sad, EmotionState.Sadness),
			new Tuple<Emotion, Emotion, EmotionState> (Emotion.Tired, Emotion.Unhappy, EmotionState.Fatigue),
			new Tuple<Emotion, Emotion, EmotionState> (Emotion.Tired, Emotion.Neutral, EmotionState.Fatigue),
			new Tuple<Emotion, Emotion, EmotionState> (Emotion.Tired, Emotion.Happy, EmotionState.Fatigue),
			new Tuple<Emotion, Emotion, EmotionState> (Emotion.Tired, Emotion.Very_Happy, EmotionState.Relaxation),

			new Tuple<Emotion, Emotion, EmotionState> (Emotion.Bored, Emotion.Sad, EmotionState.Sadness),
			new Tuple<Emotion, Emotion, EmotionState> (Emotion.Bored, Emotion.Unhappy, EmotionState.Sadness),
			new Tuple<Emotion, Emotion, EmotionState> (Emotion.Bored, Emotion.Neutral, EmotionState.Boredom),
			new Tuple<Emotion, Emotion, EmotionState> (Emotion.Bored, Emotion.Happy, EmotionState.Boredom),
			new Tuple<Emotion, Emotion, EmotionState> (Emotion.Bored, Emotion.Very_Happy, EmotionState.Disinterest),

			new Tuple<Emotion, Emotion, EmotionState> (Emotion.Sleepy, Emotion.Sad, EmotionState.Sadness),
			new Tuple<Emotion, Emotion, EmotionState> (Emotion.Sleepy, Emotion.Unhappy, EmotionState.Boredom),
			new Tuple<Emotion, Emotion, EmotionState> (Emotion.Sleepy, Emotion.Neutral, EmotionState.Boredom),
			new Tuple<Emotion, Emotion, EmotionState> (Emotion.Sleepy, Emotion.Happy, EmotionState.Fatigue),
			new Tuple<Emotion, Emotion, EmotionState> (Emotion.Sleepy, Emotion.Very_Happy, EmotionState.Fatigue),
		};

		private static List<Tuple<Emotion, EmotionState, EmotionState>> state_Temp_Matrix = new List<Tuple<Emotion, EmotionState, EmotionState>>()
		{
			new Tuple<Emotion, EmotionState, EmotionState> (Emotion.Excited, EmotionState.Sadness, EmotionState.Mixed),
			new Tuple<Emotion, EmotionState, EmotionState> (Emotion.Excited, EmotionState.Fatigue, EmotionState.Fatigue),
			new Tuple<Emotion, EmotionState, EmotionState> (Emotion.Excited, EmotionState.Boredom, EmotionState.Mixed),
			new Tuple<Emotion, EmotionState, EmotionState> (Emotion.Excited, EmotionState.Disinterest, EmotionState.Mixed),
			new Tuple<Emotion, EmotionState, EmotionState> (Emotion.Excited, EmotionState.Relaxation, EmotionState.Flow),
			new Tuple<Emotion, EmotionState, EmotionState> (Emotion.Excited, EmotionState.Enjoyment, EmotionState.Flow),

			new Tuple<Emotion, EmotionState, EmotionState> (Emotion.Inspired, EmotionState.Sadness, EmotionState.Mixed),
			new Tuple<Emotion, EmotionState, EmotionState> (Emotion.Inspired, EmotionState.Fatigue, EmotionState.Mixed),
			new Tuple<Emotion, EmotionState, EmotionState> (Emotion.Inspired, EmotionState.Boredom, EmotionState.Mixed),
			new Tuple<Emotion, EmotionState, EmotionState> (Emotion.Inspired, EmotionState.Disinterest, EmotionState.Mixed),
			new Tuple<Emotion, EmotionState, EmotionState> (Emotion.Inspired, EmotionState.Relaxation, EmotionState.Flow),
			new Tuple<Emotion, EmotionState, EmotionState> (Emotion.Inspired, EmotionState.Enjoyment, EmotionState.Flow),

			new Tuple<Emotion, EmotionState, EmotionState> (Emotion.Interested, EmotionState.Sadness, EmotionState.Mixed),
			new Tuple<Emotion, EmotionState, EmotionState> (Emotion.Interested, EmotionState.Fatigue, EmotionState.Mixed),
			new Tuple<Emotion, EmotionState, EmotionState> (Emotion.Interested, EmotionState.Boredom, EmotionState.Mixed),
			new Tuple<Emotion, EmotionState, EmotionState> (Emotion.Interested, EmotionState.Disinterest, EmotionState.Mixed),
			new Tuple<Emotion, EmotionState, EmotionState> (Emotion.Interested, EmotionState.Relaxation, EmotionState.Relaxation),
			new Tuple<Emotion, EmotionState, EmotionState> (Emotion.Interested, EmotionState.Enjoyment, EmotionState.Enjoyment),

			new Tuple<Emotion, EmotionState, EmotionState> (Emotion.Puzzled, EmotionState.Sadness, EmotionState.Sadness),
			new Tuple<Emotion, EmotionState, EmotionState> (Emotion.Puzzled, EmotionState.Fatigue, EmotionState.Puzzlement),
			new Tuple<Emotion, EmotionState, EmotionState> (Emotion.Puzzled, EmotionState.Boredom, EmotionState.Boredom),
			new Tuple<Emotion, EmotionState, EmotionState> (Emotion.Puzzled, EmotionState.Disinterest, EmotionState.Disinterest),
			new Tuple<Emotion, EmotionState, EmotionState> (Emotion.Puzzled, EmotionState.Relaxation, EmotionState.Puzzlement),
			new Tuple<Emotion, EmotionState, EmotionState> (Emotion.Puzzled, EmotionState.Enjoyment, EmotionState.Puzzlement),

			new Tuple<Emotion, EmotionState, EmotionState> (Emotion.Neutral, EmotionState.Sadness, EmotionState.Sadness),
			new Tuple<Emotion, EmotionState, EmotionState> (Emotion.Neutral, EmotionState.Fatigue, EmotionState.Fatigue),
			new Tuple<Emotion, EmotionState, EmotionState> (Emotion.Neutral, EmotionState.Boredom, EmotionState.Boredom),
			new Tuple<Emotion, EmotionState, EmotionState> (Emotion.Neutral, EmotionState.Disinterest, EmotionState.Disinterest),
			new Tuple<Emotion, EmotionState, EmotionState> (Emotion.Neutral, EmotionState.Relaxation, EmotionState.Relaxation),
			new Tuple<Emotion, EmotionState, EmotionState> (Emotion.Neutral, EmotionState.Enjoyment, EmotionState.Enjoyment),

			new Tuple<Emotion, EmotionState, EmotionState> (Emotion.Confused, EmotionState.Sadness, EmotionState.Sadness),
			new Tuple<Emotion, EmotionState, EmotionState> (Emotion.Confused, EmotionState.Fatigue, EmotionState.Confussion),
			new Tuple<Emotion, EmotionState, EmotionState> (Emotion.Confused, EmotionState.Boredom, EmotionState.Confussion),
			new Tuple<Emotion, EmotionState, EmotionState> (Emotion.Confused, EmotionState.Disinterest, EmotionState.Confussion),
			new Tuple<Emotion, EmotionState, EmotionState> (Emotion.Confused, EmotionState.Relaxation, EmotionState.Puzzlement),
			new Tuple<Emotion, EmotionState, EmotionState> (Emotion.Confused, EmotionState.Enjoyment, EmotionState.Puzzlement),

			new Tuple<Emotion, EmotionState, EmotionState> (Emotion.Anxious, EmotionState.Sadness, EmotionState.Sadness),
			new Tuple<Emotion, EmotionState, EmotionState> (Emotion.Anxious, EmotionState.Fatigue, EmotionState.Confussion),
			new Tuple<Emotion, EmotionState, EmotionState> (Emotion.Anxious, EmotionState.Boredom, EmotionState.Confussion),
			new Tuple<Emotion, EmotionState, EmotionState> (Emotion.Anxious, EmotionState.Disinterest, EmotionState.Confussion),
			new Tuple<Emotion, EmotionState, EmotionState> (Emotion.Anxious, EmotionState.Relaxation, EmotionState.Mixed),
			new Tuple<Emotion, EmotionState, EmotionState> (Emotion.Anxious, EmotionState.Enjoyment, EmotionState.Mixed),

			new Tuple<Emotion, EmotionState, EmotionState> (Emotion.Angry, EmotionState.Sadness, EmotionState.Sadness),
			new Tuple<Emotion, EmotionState, EmotionState> (Emotion.Angry, EmotionState.Fatigue, EmotionState.Sadness),
			new Tuple<Emotion, EmotionState, EmotionState> (Emotion.Angry, EmotionState.Boredom, EmotionState.Sadness),
			new Tuple<Emotion, EmotionState, EmotionState> (Emotion.Angry, EmotionState.Disinterest, EmotionState.Sadness),
			new Tuple<Emotion, EmotionState, EmotionState> (Emotion.Angry, EmotionState.Relaxation, EmotionState.Mixed),
			new Tuple<Emotion, EmotionState, EmotionState> (Emotion.Angry, EmotionState.Enjoyment, EmotionState.Mixed),
		};

		private EmotionState CalculateEmotionStateFromTestResults(List<EmotionalState> eel)
		{
			Emotion moodS = SLOPlayer.UserInfo.EmotionalInfo.Where(x => x.EmotionalState.Context == MoodSelectionControl.NowMoodContext).OrderByDescending(x => x.When).Select(x => x.EmotionalState.Emotion).FirstOrDefault();
			Emotion moodT = eel.Where(x => x.Context == EmotionSelectionControl.NowEmotionalContext).Select(x => x.Emotion).FirstOrDefault();
			Emotion trait = eel.Where(x => x.Context == EmotionSelectionControl.DuringSLOEmotionalContext).Select(x => x.Emotion).FirstOrDefault();
			Emotion state = eel.Where(x => x.Context == EmotionSelectionControl.OtherExperiencedEmotionalContext).Select(x => x.Emotion).FirstOrDefault();

			EmotionState emotionalState = Trait_MoodT_Matrix.Where(x => x.Item1 == trait && x.Item2 == moodT).Select(x => x.Item3).FirstOrDefault();

			emotionalState = state_Temp_Matrix.Where(x => x.Item1 == state && x.Item2 == emotionalState).Select(x => x.Item3).FirstOrDefault();

			ActivityLogger.LogActivity(SLOPlayer.UserInfo.Ip, SLOPlayer.UserInfo.Name, String.Format("Emotional test completed. moodT: {0}, trait: {1}, state: {2}, emotionalState:{3}", moodT, trait, state, emotionalState));

			return emotionalState;
		}

		//private EmotionState CalculateEmotionStateFromTestResults(List<EmotionalState> eel)
		//{
		//    Emotion moodS = SLOPlayer.UserInfo.EmotionalInfo.Where(x => x.EmotionalState.Context == MoodSelectionControl.NowMoodContext).OrderByDescending(x => x.When).Select(x => x.EmotionalState.Emotion).FirstOrDefault();
		//    Emotion moodT = eel.Where(x => x.Context == EmotionSelectionControl.NowEmotionalContext).Select(x => x.Emotion).FirstOrDefault();
		//    Emotion trait = eel.Where(x => x.Context == EmotionSelectionControl.DuringSLOEmotionalContext).Select(x => x.Emotion).FirstOrDefault();
		//    Emotion state = eel.Where(x => x.Context == EmotionSelectionControl.OtherExperiencedEmotionalContext).Select(x => x.Emotion).FirstOrDefault();

		//    EmotionState emotionalState = EmotionState.None;

		//    if (moodS != Emotion.Unhappy && moodS != Emotion.Sad)
		//    {
		//        if (moodT != Emotion.Unhappy && moodT != Emotion.Sad)
		//        {
		//            if (state != Emotion.Confused && state != Emotion.Anxious && state != Emotion.Angry)
		//            {
		//                if (trait != Emotion.Tired && trait != Emotion.Sleepy && trait != Emotion.Bored)
		//                {
		//                    emotionalState = EmotionState.Enjoyment;
		//                }
		//                else // if (trait == Emotion.Tired || trait == Emotion.Sleepy || trait == Emotion.Bored)
		//                {
		//                    emotionalState = EmotionState.Hopefulness;
		//                }
		//            }
		//            if ((trait == Emotion.Enthusiastic) && (state == Emotion.Excited || state == Emotion.Inspired || state == Emotion.Interested))
		//            {
		//                emotionalState = EmotionState.Flow;
		//            }
		//            if ((trait == Emotion.Fine || trait == Emotion.Relaxed || trait == Emotion.Neutral || trait == Emotion.Tired) && (state == Emotion.Puzzled))
		//            {
		//                emotionalState = EmotionState.Puzzlement;
		//            }
		//            if ((trait == Emotion.Sleepy || trait == Emotion.Bored) && (state != Emotion.Excited && state != Emotion.Anxious && state != Emotion.Angry))
		//            {
		//                emotionalState = EmotionState.Relaxation;
		//            }
		//        }
		//    }
		//    else if ((moodT == Emotion.Unhappy || moodT == Emotion.Sad) && (trait == Emotion.Neutral || trait == Emotion.Tired || trait == Emotion.Sleepy || trait == Emotion.Bored) && (state != Emotion.Excited && state != Emotion.Inspired && state != Emotion.Interested))
		//    {
		//        emotionalState = EmotionState.Sadness;
		//    }


		//    if ((moodT != Emotion.Unhappy && moodT != Emotion.Sad) && (trait == Emotion.Neutral || trait == Emotion.Tired || trait == Emotion.Sleepy || trait == Emotion.Bored) && (state == Emotion.Puzzled || state == Emotion.Confused || state == Emotion.Anxious))
		//    {
		//        emotionalState = EmotionState.Confussion;
		//    }

		//    if (trait == Emotion.Sleepy || trait == Emotion.Bored)
		//    {
		//        emotionalState = EmotionState.Boredom;
		//    }

		//    return emotionalState;
		//}

		private void ContinueButton_Click(object sender, RoutedEventArgs e)
		{
			// Go to next scene
			OnPlayNext();
		}

		private void ElseButton_Click(object sender, RoutedEventArgs e)
		{
			System.Windows.Browser.HtmlPage.Window.Navigate(new Uri("http://lab.andre-michelle.com/crash/", UriKind.Absolute), "_blank");
		}

		public override bool IsFastNextEnabled { get { return false; } }

		public override bool IsNextEnabled { get { return false; } }

		public override bool IsPreviousEnabled { get { return false; } }
	}
}
