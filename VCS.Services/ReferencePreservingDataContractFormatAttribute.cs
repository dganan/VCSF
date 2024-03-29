﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel.Description;
using System.ServiceModel.Channels;

namespace VCS
{
	public class ReferencePreservingDataContractFormatAttribute : Attribute, IOperationBehavior
	{
		#region IOperationBehavior Members

		public void AddBindingParameters(OperationDescription description, BindingParameterCollection parameters)
		{

		}

		public void ApplyClientBehavior(OperationDescription description, System.ServiceModel.Dispatcher.ClientOperation proxy)
		{
			IOperationBehavior innerBehavior = new ReferencePreservingDataContractSerializerOperationBehavior(description);

			innerBehavior.ApplyClientBehavior(description, proxy);
		}
		
		public void ApplyDispatchBehavior(OperationDescription description, System.ServiceModel.Dispatcher.DispatchOperation dispatch)
		{
			IOperationBehavior innerBehavior = new ReferencePreservingDataContractSerializerOperationBehavior(description);

			innerBehavior.ApplyDispatchBehavior(description, dispatch);
		}

		public void Validate(OperationDescription description)
		{

		}

		#endregion
	}
}