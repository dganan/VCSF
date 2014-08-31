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
using System.Collections.Generic;
using VCS.KeywordsRepositoryService;
using System.Linq;
using VCS.SpeechActClassificationService;

namespace VCS
{
	public class DialogScenePartEditor
	{
		internal static DialogScenePart EditingDialogScenePart { get; set; }

		internal static bool CheckEditingDialogScenePart(Func<Uri, bool> navigate)
		{
			if (EditingDialogScenePart == null)
			{
				if (SceneEditor.CheckEditingScene(SceneType.Dialog, navigate))
				{
					GoBack(navigate);

					return false;
				}
			}

			return true;
		}

		private static void GoBack(Func<Uri, bool> navigate)
		{
			EditingDialogScenePart = null;

			navigate(Pages.SceneEditorPage(SceneEditor.EditingScene.SceneType));
		}

		public static bool Inserting { get; set; }

		public static string AcceptEdit(string name, string text, string langstr, Character character, string emotionstr, List<string> keywords, List<string> speechActs, List<DialogSpecialMark> specialMarks, Func<Uri, bool> navigate)
		{
			if (String.IsNullOrWhiteSpace(name))
			{
				return "The name of the scene part is required";
			}

			if (String.IsNullOrWhiteSpace(text))
			{
				return "The text of the scene part is required";
			}

			Language language;

			if (!Enum.TryParse<Language>(langstr, true, out language))
			{
				return "The selected language is not valid";
			}

			if (character == null)
			{
				return "The selected character is not valid";
			}

			EmotionDeferred emotionalState;

			if (!Enum.TryParse<EmotionDeferred>(emotionstr, true, out emotionalState))
			{
				return "The selected deferred emotional state is not valid";
			}

			EditingDialogScenePart.Name = name;
			EditingDialogScenePart.Speech = text;
			EditingDialogScenePart.Language = language;

			EditingDialogScenePart.Character = character;

			EditingDialogScenePart.DeferredEmotionalState = emotionalState;

			EditingDialogScenePart.Keywords = keywords;

			EditingDialogScenePart.SpeechActs = speechActs;

			EditingDialogScenePart.SpecialMarks = specialMarks;

			if (Inserting)
			{
				DialogSceneEditor.AddDialogScenePart(EditingDialogScenePart);

				Inserting = false;
			}

			GoBack(navigate);

			return null;
		}

		public static void CancelEdit(Func<Uri, bool> navigate)
		{
			Inserting = false;

			GoBack(navigate);
		}

		public static void GetKeywordsList(DialogScenePartEditorPage page)
		{
			KeywordsRepositoryServiceClient keywordsRepository = SLOEditor.KeywordsRepositoryServiceClient;

			keywordsRepository.GetAvailableKeywordsCompleted += (o, ea) =>
			{
				try
				{
					List<string> keywords = ea.Result.Select(x=>x.ToString())
									.Union ((EditingDialogScenePart.Speech?? "").Split (new char [] { ' ', ',', '.', ';', '?', ':', '\n', '\r', '\t' })
									.Where (x=>x.Length>3))
									.OrderBy(x=>x)
									.ToList();

					page.SourceKeywords = keywords;

					if (EditingDialogScenePart.Keywords != null)
					{
						page.SelectedKeywords = EditingDialogScenePart.Keywords;
					}
				}
				catch (Exception ex)
				{
					ExceptionHandler.HandleException(ex);
				}
			};

			keywordsRepository.GetAvailableKeywordsAsync();
		}

		public static void GetSpeechActsList(DialogScenePartEditorPage page)
		{
			SpeechActClassificationServiceClient speechActClassification = SLOEditor.SpeechActClassificationServiceClient;

			speechActClassification.GetAllSpeechActsCompleted += (o, ea) =>
			{
				try
				{
					page.SourceSpeechActs = ea.Result.OrderBy(x => x).ToList();

					if (EditingDialogScenePart.SpeechActs != null)
					{
						page.SelectedSpeechActs = EditingDialogScenePart.SpeechActs;
					}
				}
				catch (Exception ex)
				{
					ExceptionHandler.HandleException(ex);
				}
			};

			speechActClassification.GetAllSpeechActsAsync();
		}

		internal static void ClassifySpeechAct(DialogScenePartEditorPage page, BusyIndicator loadingPanel)
		{
			SpeechActClassificationServiceClient speechActClassification = SLOEditor.SpeechActClassificationServiceClient;

			speechActClassification.ClassifyTextCompleted += (o, ea) =>
			{
				try
				{
					EditingDialogScenePart.SpeechActs = ea.Result.ToList();

					if (EditingDialogScenePart.SpeechActs != null)
					{
						page.SelectedSpeechActs = EditingDialogScenePart.SpeechActs;
					}

					loadingPanel.IsBusy = false;
				}
				catch (Exception ex)
				{
					ExceptionHandler.HandleException(ex);
				}
			};

			speechActClassification.ClassifyTextAsync(EditingDialogScenePart.Speech);
		}
	}
}
