using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.IO;
using System.Xml.Linq;

namespace VCS
{
	public static class KeywordsRepositoryContentIO
	{
		public static List<string> GetAvailableKeywords()
		{
			string repositoryFile = ConfigurationManager.AppSettings["KeywordsRepository"];

			List<string> keywords = new List<string>();

			if (File.Exists(repositoryFile))
			{
				StreamReader sr = new StreamReader(repositoryFile);

				string line = sr.ReadLine();

				while (line != null)
				{
					keywords.Add(line);

					line = sr.ReadLine();
				}
			}

			return keywords;
		}
	}
}