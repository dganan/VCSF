using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace VCS
{
	// NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ISLORepositoryService" in both code and config file together.
	[ServiceContract]
	public interface IKeywordsRepositoryService
	{
		[OperationContract]
		List<string> GetAvailableKeywords();
	}
}
