using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IvanAkcheurov.NTextCat.Lib.Legacy;
using System.Configuration;

namespace VCS
{
	public class CS2toSLO
	{
		public static SLO CreateSLO(CollaborativeSession cs2, bool automaticCategorization)
		{
			SLO slo = new SLO();

			slo.Name = cs2.Name;

			//Logger.LogMessage("Name: " + slo.Name);

			if (cs2.Posts.Count > 0 && cs2.Posts[0].Categories.Count > 0 && !String.IsNullOrWhiteSpace(cs2.Posts[0].Categories[0].Name))
			{
				slo.Name = slo.Name + "#;#" + cs2.Posts[0].Categories[0].Name;

				//Logger.LogMessage("Name from category: " + slo.Name);
			}

			slo.SourceUrl = cs2.Url;
			
			slo.Characters = CreateCharacters(cs2.UserAccounts);

			slo.Scenes = CreateScenes(cs2, cs2.Posts, slo.Characters, automaticCategorization);

			for (int i = 0; i<slo.Scenes.Count;i++)
			{
				slo.Scenes[i].Order = i;
			}

			return slo;
		}

		private static List<Scene> CreateScenes(CollaborativeSession cs2, List<Post> posts, List<Character> characters, bool automaticCategorization)
		{
			// Create a Scene for each thread
			// return CreateScenesByThread(cs2, posts, characters, automaticCategorization);

			// Create a Scene for each post
			return CreateScenesByPost(cs2, posts, characters, automaticCategorization);
		}
		
		private static List<Scene> CreateScenesByThread(CollaborativeSession cs2, List<Post> posts, List<Character> characters, bool automaticCategorization)
		{
			List<Scene> scenes = new List<Scene>();

			IEnumerable<Post> mainPosts = posts.Where(x => x.ReplyOf == null);

			//IEnumerable<Post> mainPosts = Posts.Except (Posts.SelectMany(x => x.Replies).Distinct());

			//IEnumerable<Post> mainPosts = new List<Post> () { Posts.First() };

			int i = 1;

			foreach (Post p in mainPosts)
			{
				DialogScene ds = new DialogScene();

				ds.Name = "Scene " + i;

				i++;

				// Add only those characters who interact in the scene, no all from the SLO

				var users = posts.Select(x => x.Creator.Id);

				CreateDialogScenePartsByThread(characters, posts, p, ds, automaticCategorization);

				scenes.Add(ds);
			}

			return scenes;
		}

		private static List<Scene> CreateScenesByPost(CollaborativeSession cs2, List<Post> posts, List<Character> characters, bool automaticCategorization)
		{
			List<Scene> scenes = new List<Scene>();

			IEnumerable<Post> mainPosts = posts.Where(x => x.ReplyOf == null);

			int i = 1;
			
			foreach (Post p in mainPosts)
			{
				scenes.AddRange(CreateDialogScenesByPost(characters, posts, p, automaticCategorization, ref i));
			}

			return scenes;
		}

		private static List<Scene> CreateDialogScenesByPost(List<Character> characters, List<Post> posts, Post p, bool automaticCategorization, ref int index)
		{
			List<Scene> scenes = new List<Scene>();

			DialogScene ds = new DialogScene();

			ds.Name = "Scene " + index;

			ds.DialogSceneParts.AddRange(CreateDialogScenePart(characters, p, automaticCategorization));

			scenes.Add(ds);

			index++;

			List<Post> replies = posts.Where(x => x.ReplyOf != null && x.ReplyOf.Id == p.Id).OrderBy(x => x.CreationDate).ToList();

			foreach (Post p2 in replies)
			{
				scenes.AddRange(CreateDialogScenesByPost(characters, posts, p2, automaticCategorization, ref index));
			}
			
			return scenes;
		}

		private static void CreateDialogScenePartsByThread(List<Character> characters, List<Post> posts, Post p, DialogScene ds, bool automaticCategorization)
		{
			ds.DialogSceneParts.AddRange(CreateDialogScenePart(characters, p, automaticCategorization));

			// TODO : Order posts by thread and date5

			List<Post> replies = posts.Where(x => x.ReplyOf != null && x.ReplyOf.Id == p.Id).OrderBy(x => x.CreationDate).ToList();

			foreach (Post p2 in replies)
			{
				CreateDialogScenePartsByThread(characters, posts, p2, ds, automaticCategorization);
			}
		}

		private static List<DialogScenePart> CreateDialogScenePart(List<Character> characters, Post p, bool automaticCategorization)
		{
			string postText = PostContentPreProcessor.PreProcessPostText(p.Content);

			Language lang = LanguageGesser.IdentifyLanguage(postText);

			string[] subPosts = postText.SplitWithSeparators(250, new char[] { '.', '?', '!' }, new char[] { ',', ':', ';' });

			Character c = characters.Where(x => x.Id == p.Creator.Id).FirstOrDefault();

			if (automaticCategorization)
			{
				return subPosts.Where(x => x.Length > 1)
					.Select(x => new DialogScenePart() { Name = x.Substring(0, Math.Min(90, x.Length)) + "...", Speech = x, Character = c, Language = lang, SpeechActs = AutomaticSpeechActsClassification(x) }).ToList();
			}
			else 
			{
				return subPosts.Where(x => x.Length > 1)
					.Select(x => new DialogScenePart() { Name = x.Substring(0, Math.Min(90, x.Length)) + "...", Speech = x, Character = c, Language = lang }).ToList();
			}

			//dsp.EmotionalState = EmotionalState.Normal;
		}

		private static List<string> AutomaticSpeechActsClassification(string x)
		{
			return SpeechActClassifier.Classify(x);
		}

		private static List<Character> CreateCharacters(IEnumerable<UserAccount> userAccounts)
		{
			List<Character> characters = new List<Character>();

			Dictionary<Tuple<Role, Gender>, int> currentAvatarIndexWithSameRoleAndGender = new Dictionary<Tuple<Role, Gender>, int>();

			foreach (UserAccount ua in userAccounts)
			{
				Character c = new Character();

				c.Id = ua.Id;

				//// Temporal fix for convert Surname Name to Name Surname

				//List<string> parts = ua.Name.Split(' ').ToList();

				//if (parts.Count > 2)
				//{
				//    parts.Insert(0, parts[parts.Count - 1]);
				//    parts.RemoveAt(parts.Count - 1);
				//}
				//
				//c.Name = string.Join(" ", parts).Trim();

				c.Name = ua.Name;

				// FIX, may be VCS Creator can ask if use photos or avatars

				if (ua.Avatar != null && ua.Avatar.Length > 0)
				{
				    c.PhotoAvatar = ua.Avatar;
				}
				else
				{
					// TEMPORAL

					byte[] bytes = null;

					using (System.IO.FileStream fs = new System.IO.FileStream("C:\\VCS\\images\\nofoto.jpg", System.IO.FileMode.Open, System.IO.FileAccess.Read))
					{
					    bytes = new byte[(int)fs.Length];

					    fs.Read(bytes, 0, (int)fs.Length);
					}

					if (bytes != null)
					{
					    c.PhotoAvatar = bytes.ToArray();
					}

					//int index = 0;

					//// if name has surnames also, take them off

					//string name = ua.Name.Split(' ').FirstOrDefault();

					//Gender gender = (ua.Gender != Gender.Unknown ? ua.Gender : GenderGuesser.GuessGender(name));

					//Tuple<Role, Gender> roleAndGenderTuple = new Tuple<Role, Gender>(ua.Role, gender);

					//if (currentAvatarIndexWithSameRoleAndGender.ContainsKey(roleAndGenderTuple))
					//{
					//    index = currentAvatarIndexWithSameRoleAndGender[roleAndGenderTuple];
					//}

					//c.AnimationAvatar = AnimationAvatarExtension.SuitableAnimationAvatar(ua.Role, gender, index);
					//c.UseAnimatedAvatar = true;

					//currentAvatarIndexWithSameRoleAndGender[roleAndGenderTuple] = index + 1;
				}

				//c.Age = (ua.Age != Age.Unknown ? ua.Age : Age.Adult);

				c.Age = Age.Unknown;

				characters.Add(c);
			}

			return characters;
		}
	}

	public static class StringsExtensions
	{
		public static string[] SplitWithSeparators(this string str, int maxPhraseLength, char[] principalSeparators, char[] secondarySeparators)
		{
			List<string> splits = new List<string>();

			splits.AddRange(str.SplitWithSeparators(principalSeparators));

			List<string> splitsTemp = new List<string>();

			foreach (string str2 in splits)
			{
				if (str2.Length <= maxPhraseLength)
				{
					splitsTemp.Add(str2);
				}
				else
				{
					splitsTemp.AddRange(str2.SplitWithSeparators(secondarySeparators));
				}
			}

			splits = splitsTemp;

			splitsTemp = new List<string>();

			foreach (string str2 in splits)
			{
				if (str2.Length <= maxPhraseLength)
				{
					splitsTemp.Add(str2);
				}
				else
				{
					splitsTemp.AddRange(str2.SplitByLength(maxPhraseLength));
				}
			}

			return splitsTemp.ToArray();
		}

		private static List<string> SplitWithSeparators(this string str, char[] separators)
		{
			List<string> tempSplits = new List<string>();

			int lastIndex = 0;

			for (int i = 0; i < str.Length; i++)
			{
				if (separators.Contains(str[i]))
				{
					tempSplits.Add(str.SubstringFromTo(lastIndex, i));

					lastIndex = i + 1;
				}
			}

			if (lastIndex < str.Length)
			{
				tempSplits.Add(str.SubstringFromTo(lastIndex, str.Length - 1));
			}

			return tempSplits;
		}

		public static string SubstringFromTo(this string str, int from, int to)
		{
			return str.Substring(from, (to - from) + 1);
		}

		private static List<string> SplitByLength(this string str, int maxPhraseLength)
		{
			List<string> tempSplits = new List<string>();

			string[] words = str.Split(' ');

			if (words.Where(x => x.Length > maxPhraseLength).Count() > 0)
			{
				words = SplitWords(words, maxPhraseLength);
			}

			StringBuilder temp = new StringBuilder();

			int index = 0;

			while (index < words.Length)
			{
				while (index < words.Length && temp.Length + words[index].Length <= maxPhraseLength)
				{
					temp.Append(words[index]);
					temp.Append(" ");

					index++;
				}

				tempSplits.Add(temp.ToString().Trim());

				temp = new StringBuilder();
			}

			return tempSplits;
		}

		private static string[] SplitWords(string[] words, int maxPhraseLength)
		{
			List<string> wordsSplitted = new List<string>();

			foreach (string s in words)
			{
				if (s.Length <= maxPhraseLength)
				{
					wordsSplitted.Add(s);
				}
				else
				{
					int index = 0;

					while (index < s.Length)
					{
						wordsSplitted.Add(s.SubstringFromTo(index, Math.Min(s.Length - 1, index + maxPhraseLength - 1)));

						index += maxPhraseLength;
					}
				}
			}

			return wordsSplitted.ToArray();
		}
	}
}