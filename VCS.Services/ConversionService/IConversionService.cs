using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace VCS
{
	// NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IConverterService" in both code and config file together.
	[ServiceContract]
	public interface IConversionService
	{
		[OperationContract]
		List<CollaborativeSessionDescriptor> GetAvailableCollaborativeSessions(string dataSource);

		[OperationContract]
		[ReferencePreservingDataContractFormat]
		CollaborativeSession GetCollaborativeSessionById(string id, string dataSource, params string[] parameters);
	}
}
