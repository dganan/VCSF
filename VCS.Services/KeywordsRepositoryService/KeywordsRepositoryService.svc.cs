using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Configuration;
using System.IO;

namespace VCS
{
	// NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "SLORepositoryService" in code, svc and config file together.
	public class KeywordsRepositoryService : IKeywordsRepositoryService
	{
		public List<string> GetAvailableKeywords()
		{
			List<string> list = new List<string>();

			try
			{
				list = KeywordsRepositoryContentIO.GetAvailableKeywords();
			}
			catch (Exception e)
			{
				Logger.LogException(e);
			}

			return list;
		}
	}
}
