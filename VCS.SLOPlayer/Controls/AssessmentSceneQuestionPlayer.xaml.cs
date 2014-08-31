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
using System.Windows.Media.Imaging;
using System.Runtime.InteropServices.Automation;
using System.IO;
using VCS.SpeechService;
using System.Windows.Threading;
using VCS.WavDecoder;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.BZip2;
using System.Threading;

namespace VCS
{
	public partial class AssessmentSceneQuestionPlayer : ScenePartPlayer
	{
		public AssessmentSceneQuestionPlayer()
		{
			InitializeComponent();

			VariableWidth = 140;
			VariableImageHeight = 193;
			VariableHeight = VariableImageHeight + 40;

			if (Config.Language == VCS.Language.Catalan)
			{
				ContinueButton.Content = "Continuar";
			}
		}

		private DispatcherTimer timer;

		private static string latestCharacter;

		private AssessmentSceneQuestion Question { get; set; }

		public override ScenePart ScenePart
		{
			set
			{
				AssessmentSceneQuestion asq = value as AssessmentSceneQuestion;

				Question = asq;

				if (asq != null)
				{
					DialogText.Text = asq.QuestionSpeech.RemoveHTMLTags();

					QuestionText.Text = asq.QuestionText;

					foreach (Answer a in asq.Answers)
					{ 
						RadioButton rb = new RadioButton ();

						rb.Content = a.AnswerText;
						rb.Tag = a.Id;

						rb.Margin = new Thickness(0, 0, 0, 5);

						AnswersPanel.Children.Add(rb);
					}

					AnswersPanel.InvalidateMeasure();
					AnswersPanel.UpdateLayout();

					if (!asq.Evaluator.UseAnimatedAvatar && asq.Evaluator.PhotoAvatar != null)
					{
						AvatarOpenedClosed.Source = asq.Evaluator.PhotoAvatarImage;
						AvatarOpenedOpened.Source = asq.Evaluator.PhotoAvatarImage;
						AvatarClosedClosed.Source = asq.Evaluator.PhotoAvatarImage;
					}
					else
					{
						AvatarOpenedClosed.Source = asq.Evaluator.AvatarOpenedClosed;
						AvatarOpenedOpened.Source = asq.Evaluator.AvatarOpenedOpened;
						AvatarClosedClosed.Source = asq.Evaluator.AvatarClosedClosed;
					}

					NameText.Text = asq.Evaluator.Name;

					if (asq.Evaluator.Id != latestCharacter)
					{
						Thread.Sleep(500);
					}

					latestCharacter = asq.Evaluator.Id;

					PlayProcess(asq);
				}
			}
		}

//		int currentIteration = 0;

		AutoResetEvent autoEvent = new AutoResetEvent(false);

		private void PlayProcess(AssessmentSceneQuestion dsp)
		{
			byte[] audio = dsp.SpeechAudio;

			MemoryStream ms = new MemoryStream(audio);

			//MessageBox.Show(ms.Length / 1024 + " kilobytes");

			WavMediaStreamSource audioStream = new WavMediaStreamSource(ms);

			WavRiffParser parser = new WavRiffParser(ms);

			parser.ParseWAVEHeader();

			// It seems it depends on the voice used
			// With System.Speech, take 500
			// int sleep = (int)(parser.Duration / 10000d) - 500;
			// int sleep = (int)(parser.Duration / 10000d) - 800;

			int sleep = (int)(parser.Duration / 10000d);

			//if (sleep > 0)
			//{
			//    Thread.Sleep(sleep);
			//}

			//int duration = (int)Math.Round(parser.Duration / 10000000d, 0);

			//if (timer == null)
			{
				timer = new DispatcherTimer();
				//timer.Interval = new TimeSpan(0, (int)(duration / 60), (int)(duration % 60));
				timer.Interval = new TimeSpan(0, 0, 0, 0, sleep);

				timer.Tick += new EventHandler(timer_Tick);
			}

			AudioPlayer.AutoPlay = false;

			AudioPlayer.MediaOpened += new RoutedEventHandler((o, e) =>
			{
				// Quitamos el chasquido inicial
				AudioPlayer.Position = new TimeSpan(500000);
				AudioPlayer.Play();
			});

			AudioPlayer.SetSource(audioStream);

			(this.Resources["FaceAnimator"] as Storyboard).Begin();

			timer.Start();
		}

		DateTime dt;

		void timer_Tick(object sender, EventArgs e)
		{
			// end DO CLEANUP
			Stop();
		}

		public override void Stop()
		{
			if (timer != null)
			{
				// This is for the avatar to continue opening / closing eyes, but not the mouth
				AvatarOpenedOpened.Source = Question.Evaluator.AvatarOpenedClosed;

				AudioPlayer.Stop();

				timer.Stop();
				timer = null;
			}
		}

		public int VariableWidth { get; set; }
		public int VariableHeight { get; set; }
		public int VariableImageHeight { get; set; }

		public QuestionResult QuestionResult { get; private set; }

		private void ContinueButton_Click(object sender, RoutedEventArgs e)
		{
			QuestionResult = new QuestionResult();

			QuestionResult.QuestionId = Question.Id;
			
			// null if not answered
			QuestionResult.AnswerId = AnswersPanel.Children.Select(x => (x as RadioButton)).Where(x => x.IsChecked ?? false).Select(x => x.Tag.ToString()).FirstOrDefault();

			if (QuestionResult.AnswerId == null)
			{
				QuestionResult.IsCorrectAnswer = false;
			}
			else
			{
				QuestionResult.IsCorrectAnswer = Question.Answers.Where(x => x.Id == QuestionResult.AnswerId).First().IsCorrectAnswer;
			}
			
			(this.Resources["FaceAnimator"] as Storyboard).Stop();

			OnPlayEnded();
		}
	}
}
