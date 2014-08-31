using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Runtime.Serialization;

namespace VCS.SLOModel_OLD
{
	public class SLOWriter
	{
		public static void WriteSLO(SLO slo, string file)
		{
			using (FileStream stream = new FileStream(file, FileMode.Create))
			{
				DataContractSerializer serializer = new DataContractSerializer(
					typeof(SLO),
					null,
					0x7FFF /*maxItemsInObjectGraph*/,
					false /*ignoreExtensionDataObject*/,
					true /*preserveObjectReferences : this is where the magic happens */,
					null /*dataContractSurrogate*/);

				serializer.WriteObject(stream, slo);
			}
		}
	}
}
