using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace VCS
{
	public class SLOReader
	{
		public static SLO ReadSLO(string file)
		{
			SLO slo = null;

			if (!File.Exists(file))
			{
				throw new SLOException("File '" + file + "' does not exist");
			}

			DataContractSerializer serializer = new DataContractSerializer(
				typeof(SLO),
				null,
				0x7FFF /*maxItemsInObjectGraph*/,
				false /*ignoreExtensionDataObject*/,
				true /*preserveObjectReferences : this is where the magic happens */,
				null /*dataContractSurrogate*/);

			using (FileStream stream = new FileStream(file, FileMode.Open))
			{
				slo = (SLO)serializer.ReadObject(stream);
			}

			return slo;
		}
	}
}
