using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.IO;
using System.Threading;
//using System.Speech.Synthesis;
//using System.Speech.AudioFormat;

namespace VCS
{
	// NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "SpeechService" in code, svc and config file together.
	[ServiceBehavior(IncludeExceptionDetailInFaults=true)]
	public class SpeechService : ISpeechService
	{
		public byte[] Speak(string text, Gender gender, Age age, Language language)
		{
			return Speech.Speak(text, gender, age, language);
		}	
	}
}
