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
	public interface ISLORepositoryService
	{
		[OperationContract]
        List<SLODescriptor> GetAvailableSLOs(string userId);

		[OperationContract]
        SLO GetSLOById(string resId, string userId);

		[OperationContract]
		[ReferencePreservingDataContractFormat]
		string CreateSLOFromCollaborativeSession(string csid, string csthread, string sid, string source, string name, bool automaticCategorization, string userId, out string error);

		[OperationContract]
		[ReferencePreservingDataContractFormat]
		void InsertSLO(SLO slo, string userId, out string error);

		[OperationContract]
		[ReferencePreservingDataContractFormat]
		void UpdateSLO(SLO slo, string userId, out string error);
	}
}
