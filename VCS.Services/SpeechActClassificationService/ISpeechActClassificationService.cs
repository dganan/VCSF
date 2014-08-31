using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace VCS.Services.SpeechActClassificationService
{
	// NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ICategorizationService" in both code and config file together.
	[ServiceContract]
	public interface ISpeechActClassificationService
	{
		[OperationContract]
		List<string> GetAllSpeechActs();

		[OperationContract]
		List<string> GetMainSpeechActs();

		[OperationContract]
		List<string> ClassifyText(string text);
	}
}
