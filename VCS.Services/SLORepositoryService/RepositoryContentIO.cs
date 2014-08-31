using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.IO;
using System.Xml.Linq;

namespace VCS
{
	public static class RepositoryContentIO
	{
		public static List<SLODescriptor> GetAvailableSLOs()
		{
			string sloRepositoryDir = ConfigurationManager.AppSettings["SLORepositoryDir"];

			string repositoryContentFile = sloRepositoryDir + Path.DirectorySeparatorChar + "repositoryContent.xml";

			List<SLODescriptor> slos = new List<SLODescriptor>();

			if (File.Exists(repositoryContentFile))
			{
				XDocument doc = XDocument.Load(repositoryContentFile);

				slos.AddRange(doc.Root.Elements().Select(x => new SLODescriptor() { Id = x.Attribute("Id").Value, Name = x.Attribute("Name").Value }));
			}

			return slos;
		}

		internal static SLO GetSLOById(string id)
		{
			string sloFileName = SLOFileName(id);

			return SLOReader.ReadSLO(sloFileName);
		}

		private static string SLOFileName(int id)
		{
			return SLOFileName(id.ToString());
		}

		private static string SLOFileName(string id)
		{
			string sloRepositoryDir = ConfigurationManager.AppSettings["SLORepositoryDir"];

			// By now, SLO id is taken for the name of the file
			// Later, query the repository content file to find in which file is the SLO stored

			return sloRepositoryDir + Path.DirectorySeparatorChar + id + ".slo";
		}

		internal static string InsertSLO(SLO slo)
		{
			string sloRepositoryDir = ConfigurationManager.AppSettings["SLORepositoryDir"];

			string repositoryContentFile = sloRepositoryDir + Path.DirectorySeparatorChar + "repositoryContent.xml";

			XDocument doc = null;

			int id = 0;

			if (File.Exists(repositoryContentFile))
			{
				doc = XDocument.Load(repositoryContentFile);

				foreach (XElement xe in doc.Root.Elements())
				{
					int max;

					if (Int32.TryParse (xe.Attribute("Id").Value, out max))
					{
						id = Math.Max (id, max);
					}
				}
				
				id++;
			}
			else
			{
				doc = new XDocument();

				doc.Add(new XElement("SLOList"));
			}

			slo.Id = id.ToString();

			SLOWriter.WriteSLO(slo, SLOFileName(id));

			XElement sloElement = new XElement("SLO");

			sloElement.Add(new XAttribute("Id", id));
			sloElement.Add(new XAttribute("Name", slo.Name));

			doc.Root.Add(sloElement);

			doc.Save(repositoryContentFile);

			return id.ToString();
		}

		internal static void UpdateSLO(SLO slo)
		{
			if (slo.Id == null)
			{
				InsertSLO(slo);
			}
			else
			{
				string sloRepositoryDir = ConfigurationManager.AppSettings["SLORepositoryDir"];

				string repositoryContentFile = sloRepositoryDir + Path.DirectorySeparatorChar + "repositoryContent.xml";

				XDocument doc = null;

				if (File.Exists(repositoryContentFile))
				{
					doc = XDocument.Load(repositoryContentFile);
				}
				else
				{
					doc = new XDocument();

					doc.Add(new XElement("SLOList"));
				}

				SLOWriter.WriteSLO(slo, SLOFileName(slo.Id));

				XElement sloElement = doc.Root.Elements("SLO").Where(x => x.Attribute("Id").Value == slo.Id).FirstOrDefault();

				if (sloElement == null)
				{
					sloElement = new XElement("SLO");
					sloElement.Add(new XAttribute("Id", slo.Id));
					sloElement.Add(new XAttribute("Name", slo.Name));

					doc.Root.Add(sloElement);
				}
				else
				{
					sloElement.Attribute("Name").Value = slo.Name;
				}

				doc.Save(repositoryContentFile);
			}
		}
	}
}