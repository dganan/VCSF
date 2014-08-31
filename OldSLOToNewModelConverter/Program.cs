using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using VCS;

namespace OldSLOToNewModelConverter
{
	class Program
	{
		static void Main(string[] args)
		{
			ConvertSLO(@"C:\VCS\SLORepository\7.slo");
			ConvertSLO(@"C:\VCS\SLORepository\11.slo");
		}

		private static void ConvertSLO (string filename)
		{
			string tempfilename = filename + "_temp";

			string finalfilename = filename.Substring (0, filename.Length - 4) + "_new.slo";

			StreamReader sr = new StreamReader(filename);

			string content = sr.ReadToEnd();

			sr.Close();

			content = content.Replace("xmlns=\"http://schemas.datacontract.org/2004/07/VCS\"", "xmlns=\"http://schemas.datacontract.org/2004/07/VCS.SLOModel_OLD\"");

			StreamWriter sw = new StreamWriter(tempfilename);

			sw.Write(content);

			sw.Close();

			VCS.SLOModel_OLD.SLO old_slo = VCS.SLOModel_OLD.SLOReader.ReadSLO(tempfilename);
			
			VCS.SLO new_slo = ConvertSLO(old_slo);

			VCS.SLOWriter.WriteSLO(new_slo, finalfilename);

			File.Delete(tempfilename);
		}

		private static VCS.SLO ConvertSLO(VCS.SLOModel_OLD.SLO old_slo)
		{
			VCS.SLO new_slo = new VCS.SLO();

			// Copy basic properties

			new_slo.Id = old_slo.Id;

			new_slo.Name = old_slo.Name;

			new_slo.SourceUrl = old_slo.SourceUrl;

			// Copy characters

			ConvertCharacters (old_slo, new_slo);

			// Copy scenes

			ConvertScenes(old_slo, new_slo);

			return new_slo;
		}

		private static Dictionary<string, VCS.Character> newCharacters;

		private static void ConvertCharacters(VCS.SLOModel_OLD.SLO old_slo, VCS.SLO new_slo)
		{
			int currentMaleIndex = 0;
			int currentFemaleIndex = 0;

			foreach (VCS.SLOModel_OLD.Scene s in old_slo.Scenes)
			{
				VCS.SLOModel_OLD.DialogScene ds = s as VCS.SLOModel_OLD.DialogScene;

				if (ds != null)
				{
					foreach (VCS.SLOModel_OLD.Character c in ds.Characters)
					{
						if (new_slo.Characters.Where(x => x.Name == c.Name).Count() == 0)
						{
							string name = c.Name.Split(' ').FirstOrDefault();

							Gender gender = (c.Gender != Gender.Unknown ? c.Gender : GenderGuesser.GuessGender(name));

							AnimationAvatar avatar = AnimationAvatar.Female0;

							if (gender == Gender.Male)
							{
								avatar = AnimationAvatarExtension.SuitableAnimationAvatar(gender, currentMaleIndex);
								currentMaleIndex++;
							}
							else
							{
								avatar = AnimationAvatarExtension.SuitableAnimationAvatar(Gender.Female, currentFemaleIndex);
								currentFemaleIndex++;
							}

							Character new_character = new Character();
							new_character.Name = c.Name;
							new_character.Gender = gender;
							new_character.AnimationAvatar = avatar;
							new_character.UseAnimatedAvatar = true;
							new_character.Age = c.Age;

							new_slo.Characters.Add(new_character);
						}
					}
				}
			}
		}

		private static void ConvertScenes(VCS.SLOModel_OLD.SLO old_slo, VCS.SLO new_slo)
		{
			int sceneIndex = 1;

			// Change structure (thread to post)
			foreach (VCS.SLOModel_OLD.Scene s in old_slo.Scenes)
			{ 
				VCS.SLOModel_OLD.DialogScene ds = s as VCS.SLOModel_OLD.DialogScene;

				if (ds != null)
				{
					DialogScene current_new_scene = null;

					VCS.SLOModel_OLD.Character current_character = null;

					foreach (VCS.SLOModel_OLD.DialogScenePart dsp in ds.DialogSceneParts)
					{
						if (current_character == null || current_character.Name != dsp.Character.Name)
						{
							if (current_new_scene!= null && current_new_scene.DialogSceneParts.Count > 0)
							{
								new_slo.Scenes.Add(current_new_scene);
							}

							current_new_scene = new DialogScene();
							current_new_scene.Name = "Scene " + sceneIndex;
							sceneIndex++;
						}

						current_new_scene.DialogSceneParts.Add(ConvertDialogScenePart (dsp, new_slo.Characters));

						current_character = dsp.Character;
					}

					if (current_new_scene.DialogSceneParts.Count > 0)
					{
						new_slo.Scenes.Add(current_new_scene);
					}
				}
				else 
				{
					// Conversion Not supported (too complex)
					// ConvertOtherSceneTypes(s);
				}
			}
		}

		private static DialogScenePart ConvertDialogScenePart(VCS.SLOModel_OLD.DialogScenePart dsp, List<Character> list)
		{
			DialogScenePart new_dsp = new DialogScenePart();

			// Take 90 characters for the DialogSceneParts name
			new_dsp.Name = dsp.Speech.Substring(0, Math.Min(90, dsp.Speech.Length)) + "...";
			
			// To change
			//new_dsp.EmotionalState = (Emoticon)Enum.Parse(typeof(Emoticon), dsp.EmotionalState.ToString());
			new_dsp.SpecialMarks = dsp.SpecialMarks.Select (x=>(DialogSpecialMark)Enum.Parse(typeof(DialogSpecialMark), x.ToString())).ToList();

			new_dsp.SpeechActs = dsp.SpeechActs.Select(x => x).ToList();

			new_dsp.Keywords = dsp.Keywords.Select(x => x).ToList();

			new_dsp.Speech = dsp.Speech;
			new_dsp.SpeechAudio = (dsp.SpeechAudio == null ? null : (byte[])dsp.SpeechAudio.Clone());
			new_dsp.Language = (Language)Enum.Parse(typeof(Language), dsp.Language.ToString());

			// Characters are shared inside a SLO now, so no need to clone or change them
			new_dsp.Character = list.Where (x=>x.Name == dsp.Character.Name).First();

			return new_dsp;
		}
	}
}
