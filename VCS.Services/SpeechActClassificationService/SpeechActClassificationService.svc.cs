using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace VCS.Services.SpeechActClassificationService
{
	// NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "SpeechActClassificationService" in code, svc and config file together.
	public class SpeechActClassificationService : ISpeechActClassificationService
	{
		public List<string> GetAllSpeechActs()
		{
			List<string> list = new List<string>();

			try
			{
				list = SpeechActClassifier.GetAllSpeechActs();
			}
			catch (Exception e)
			{
				Logger.LogException(e);
			}

			return list;
		}

		public List<string> GetMainSpeechActs()
		{
			List<string> list = new List<string>();

			try
			{
				list = SpeechActClassifier.GetMainSpeechActs();
			}
			catch (Exception e)
			{
				Logger.LogException(e);
			}

			return list;
		}

		public List<string> ClassifyText(string text)
		{
			List<string> list = new List<string>();

			try
			{
				list = SpeechActClassifier.Classify (text);
			}
			catch (Exception e)
			{
				Logger.LogException(e);
			}

			return list;
		}
	}
}
