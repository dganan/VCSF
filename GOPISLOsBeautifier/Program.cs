using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VCS;

namespace GOPISLOsBeautifier
{
	class Program
	{
		static List<SLO> slos;

		static Dictionary<string, Tuple<string, AnimationAvatar>> map;

		static int currentMaleIndex = 0;
		static int currentFemaleIndex = 0;

		static void Main(string[] args)
		{
			slos = new List<SLO>();

			map = new Dictionary<string, Tuple<string, AnimationAvatar>>();

			// Read

			slos.Add(SLOReader.ReadSLO(@"C:\VCS\SLORepository\1.slo"));
			slos.Add(SLOReader.ReadSLO(@"C:\VCS\SLORepository\2.slo"));
			slos.Add(SLOReader.ReadSLO(@"C:\VCS\SLORepository\3.slo"));
			slos.Add(SLOReader.ReadSLO(@"C:\VCS\SLORepository\4.slo"));
			slos.Add(SLOReader.ReadSLO(@"C:\VCS\SLORepository\5.slo"));

			// Preprocessing

			foreach (SLO slo in slos)
			{
				Preprocess(slo);
			}

			// Modification

			foreach (SLO slo in slos)
			{
				ProcessCharacters(slo);
			}

			foreach (SLO slo in slos)
			{
				ProcessShortDialogs(slo);
			}

			// Write

			SLOWriter.WriteSLO(slos[0], @"C:\VCS\SLORepository\11.slo");
			SLOWriter.WriteSLO(slos[1], @"C:\VCS\SLORepository\12.slo");
			SLOWriter.WriteSLO(slos[2], @"C:\VCS\SLORepository\13.slo");
			SLOWriter.WriteSLO(slos[3], @"C:\VCS\SLORepository\14.slo");
			SLOWriter.WriteSLO(slos[4], @"C:\VCS\SLORepository\15.slo");
		}

		private static void ProcessShortDialogs(SLO slo)
		{
			foreach (Scene s in slo.Scenes)
			{
				DialogScene ds = s as DialogScene;

				string ch = null; 

				if (ds != null)
				{
					int partindex = 0;

					while (partindex < ds.DialogSceneParts.Count)
					{
						DialogScenePart currentPart = ds.DialogSceneParts[partindex];

						if (ch == null || ch == currentPart.Character.Name)
						{
							if (currentPart.Speech.Length < 30)
							{
								// Unificate if there is other scene after

								if (partindex < ds.DialogSceneParts.Count - 1)
								{
									ds.DialogSceneParts[partindex + 1].Speech = ds.DialogSceneParts[partindex].Speech + " " + ds.DialogSceneParts[partindex + 1].Speech;

									ds.DialogSceneParts.RemoveAt(partindex);
								}
								else
								{
									// else unificate with the previous one (if any)

									if (partindex > 0)
									{
										ds.DialogSceneParts[partindex - 1].Speech = ds.DialogSceneParts[partindex - 1].Speech + " " + ds.DialogSceneParts[partindex].Speech;

										ds.DialogSceneParts.RemoveAt(partindex);
									}
								}
							}
							else
							{
								partindex++;
							}
						}

						ch = currentPart.Character.Name;
					}
				}
			}
		}

		private static void ProcessCharacters(SLO slo)
		{
			foreach (Scene s in slo.Scenes)
			{
				DialogScene ds = s as DialogScene;

				if (ds != null)
				{
					foreach (Character c in ds.Characters)
					{
						Tuple<string, AnimationAvatar> tuple = map[c.Name];

						c.Name = tuple.Item1;
						c.AnimationAvatar = tuple.Item2;
					}
				}
			}
		}

		private static void Preprocess(SLO slo)
		{
			foreach (Scene s in slo.Scenes)
			{
				DialogScene ds = s as DialogScene;

				if (ds != null)
				{
					foreach (Character c in ds.Characters)
					{
						if (!map.ContainsKey(c.Name))
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

							int count = map.Where(x => x.Value.Item1.StartsWith(name)).Count();
							string key = map.Where(x => x.Value.Item1.StartsWith(name)).Select (x=>x.Key).FirstOrDefault();

							if (count == 0)
							{
								map.Add(c.Name, new Tuple<string, AnimationAvatar>(name, avatar));
							}
							else if (count == 1)
							{
								Tuple<string, AnimationAvatar> existing = map[key];

								map[key] = new Tuple<string, AnimationAvatar>(name + " 1", existing.Item2);
								map.Add(c.Name, new Tuple<string, AnimationAvatar>(name + " 2", avatar));
							}
							else
							{
								map.Add(c.Name, new Tuple<string, AnimationAvatar>(name + " " + (count + 1), avatar));
							}
						}
					}
				}
			}
		}
	}
}
