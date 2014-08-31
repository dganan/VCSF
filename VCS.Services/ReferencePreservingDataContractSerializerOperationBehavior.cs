﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Description;
using System.Runtime.Serialization;
using System.Xml;

namespace VCS
{
	public class ReferencePreservingDataContractSerializerOperationBehavior : DataContractSerializerOperationBehavior
	{
		public ReferencePreservingDataContractSerializerOperationBehavior(OperationDescription operationDescription)
			: base(operationDescription) { }

		public override XmlObjectSerializer CreateSerializer(Type type, string name, string ns, IList<Type> knownTypes)
		{
			return CreateDataContractSerializer(type, name, ns, knownTypes);
		}

		private static XmlObjectSerializer CreateDataContractSerializer(Type type, string name, string ns, IList<Type> knownTypes)
		{
			return CreateDataContractSerializer(type, name, ns, knownTypes);
		}

		public override XmlObjectSerializer CreateSerializer(Type type, XmlDictionaryString name, XmlDictionaryString ns,IList<Type> knownTypes)
		{
			return new DataContractSerializer(type, name, ns, knownTypes,

				0x7FFF /*maxItemsInObjectGraph*/,

				false/*ignoreExtensionDataObject*/,

				true/*preserveObjectReferences*/,

				null/*dataContractSurrogate*/);
		}
	}
}
