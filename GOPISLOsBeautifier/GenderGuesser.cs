using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Configuration;

namespace VCS
{
	public static class GenderGuesser
	{
		private static Dictionary<string, Gender> namesDictionary;

		static GenderGuesser()
		{
			try
			{
				namesDictionary = new Dictionary<string, Gender>();

				DirectoryInfo di = new DirectoryInfo(ConfigurationManager.AppSettings["NamesDirectory"]);

				foreach (FileInfo fi in di.GetFiles("*.names"))
				{
					StreamReader sr = new StreamReader(fi.FullName);

					string name = sr.ReadLine();

					while (name != null)
					{
						string[] split = name.Split(';');

						if (!namesDictionary.ContainsKey(split[0]))
						{
							namesDictionary.Add(split[0], (split[1] == "M" ? Gender.Male : Gender.Female));
						}
						else 
						{
							Gender current = namesDictionary[split[0]];

							if ((split[1] == "M" && current != Gender.Male) || (split[1] == "F" && current != Gender.Female))
							{
								namesDictionary[split[0]] = Gender.Neutral;
							}
						}

						name = sr.ReadLine();
					}

					sr.Close();
				}
			}
			catch (Exception e)
			{
				Logger.LogException(e);
			}
		}

		public static Gender GuessGender(string name)
		{
			Gender gender = Gender.Neutral;

			if (name != null && namesDictionary.ContainsKey(name))
			{
				gender = namesDictionary[name];
			}
			
			return gender;
		}
	}
}
