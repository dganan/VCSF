using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IvanAkcheurov.NTextCat.Lib.Legacy;
using System.IO;

namespace VCS
{
	class Program
	{
		static void Main(string[] args)
		{
			//Character c1 = new Character();
			//Character c2 = new Character();
			//Character c3 = new Character();

			//c3.Id = "Pepito";

			//Character c4 = new Character();

			//Console.WriteLine(c1.Id);
			//Console.WriteLine(c2.Id);
			//Console.WriteLine(c3.Id);
			//Console.WriteLine(c4.Id);

			//Console.WriteLine(c1.NextSerial);

			//Console.ReadLine();

			PhpBBForumsUOCConverter converter = new PhpBBForumsUOCConverter();

			string csId = "";

			string sid = "";

			CollaborativeSession cs = converter.ReadCollaborativeSession(csId, sid);
			

			//Language lang = Language.English;

			//var languageIdentifier = new LanguageIdentifier();

			//IEnumerable<Tuple<string, double>> languages = languageIdentifier.ClassifyText("Hola això es un missatge en català", null).ToList();

			//var mostCertainLanguage = languages.FirstOrDefault();


			//string path = System.Reflection.Assembly.GetExecutingAssembly().Location;

			//path = Path.GetDirectoryName(path);

			////TestConverter tp = new TestConverter("test1.xml");
			
			////List<CollaborativeSessionDescriptor> cs = tp.AvailableCollaborativeSessions;

			////CollaborativeSession cs2 = tp.ReadCollaborativeSession (cs[0]);

			////CSMLWriter.WriteCSMLFile(cs2, "test1.csml");
			
			////SLO slo = CS2toSLO.CreateSLO(cs2);

			////SLOWriter.WriteSLO(slo, "test1.slo");
		}
	}
}
