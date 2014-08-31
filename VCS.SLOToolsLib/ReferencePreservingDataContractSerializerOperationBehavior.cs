using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Description;
using System.Runtime.Serialization;
using System.Xml;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Channels;

namespace VCS
{
//    public class SLSerializerOperationBehavior : IOperationBehavior
//    {
//    OperationDescription operationDescription;

//    public SLSerializerOperationBehavior(OperationDescription operationDescription)
//    {
//        this.operationDescription = operationDescription;
//    }

//    public void AddBindingParameters(OperationDescription operationDescription, BindingParameterCollection bindingParameters)
//    {

//    }
		
//    public void ApplyClientBehavior(OperationDescription operationDescription, ClientOperation clientOperation)

//    {

//        clientOperation.Formatter = new MyClientFormatter(operationDescription, this);

//        clientOperation.SerializeRequest = !IsUntypedMessage(operationDescription.Messages[0]);

//        clientOperation.DeserializeReply = operationDescription.Messages.Count > 1 && !IsUntypedMessage(operationDescription.Messages[1]);

//    }
//    }
 
//    public class ReferencePreservingDataContractSerializerOperationBehavior : DataContractSerializerOperationBehavior
//    {
//        public ReferencePreservingDataContractSerializerOperationBehavior(OperationDescription operationDescription)
//            : base(operationDescription) { }

//        public override XmlObjectSerializer CreateSerializer(Type type, string name, string ns, IList<Type> knownTypes)
//        {
//            return CreateDataContractSerializer(type, name, ns, knownTypes);
//        }

//        private static XmlObjectSerializer CreateDataContractSerializer(Type type, string name, string ns, IList<Type> knownTypes)
//        {
//            return CreateDataContractSerializer(type, name, ns, knownTypes);
//        }

//        public override XmlObjectSerializer CreateSerializer(Type type, XmlDictionaryString name, XmlDictionaryString ns,IList<Type> knownTypes)
//        {
//            return new DataContractSerializer(type, name, ns, knownTypes,

//                0x7FFF /*maxItemsInObjectGraph*/,

//                false/*ignoreExtensionDataObject*/,

//                true/*preserveObjectReferences*/,

//                null/*dataContractSurrogate*/);
//        }
//    }

//public void  AddBindingParameters(OperationDescription operationDescription, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
//{
//    throw new NotImplementedException();
//}

//public void  ApplyClientBehavior(OperationDescription operationDescription, System.ServiceModel.Dispatcher.ClientOperation clientOperation)
//{
//    throw new NotImplementedException();
//}

//public void  ApplyDispatchBehavior(OperationDescription operationDescription, System.ServiceModel.Dispatcher.DispatchOperation dispatchOperation)
//{
//    throw new NotImplementedException();
//}

//public void  Validate(OperationDescription operationDescription)
//{
//    throw new NotImplementedException();
//}
}
