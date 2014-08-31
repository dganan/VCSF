using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
//using System.Speech.Synthesis;

namespace VCS
{
	// NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ISpeechService" in both code and config file together.
	[ServiceContract]
	public interface ISpeechService
	{
		[OperationContract]
		byte[] Speak(string text, Gender gender, Age age, Language language);
	}
}
