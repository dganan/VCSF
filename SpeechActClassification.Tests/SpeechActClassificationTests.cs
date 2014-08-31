using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VCS;

namespace Categorization.Tests
{
	[TestClass]
	public class SpeechActClassificationTests
	{
		private static string[] speechActs = new string[] {
		
		"INFORM-Agree-Disagree",
		"INFORM-ExplainClarification-Extend-Lead-Suggest-Elaboration-Justify-State",
		"PROBLEM-Solution-ExtendSolution-AssentSolution",
		"PROBLEM-Statement",
		"REQUEST-Information-Elaboration-Clarification-Justification-Illustration",
		"REQUEST-Opinion",
		"SUPPORT-Greetings-Motivation-Encouragement",

//		"Encouragement", 
//		"Greeting", 
//		"INFORM-Agree", 
//		"INFORM-Assert", 
//		"INFORM-Disagree", 
//		"INFORM-Elaborate", 
//		"INFORM-Explain - Clarify", 
//		"INFORM-Extend", 
//		"INFORM-Justify", 
//		"INFORM-Lead", 
//		"INFORM-Suggest", 
//		"Motivation", 
//		"PROBLEM-Extend solution",
//		"PROBLEM-Solution", 
//		"PROBLEM-Solution assentient", 
//		"PROBLEM-Statement", 
//		"REQUEST-Clarification", 
//		"REQUEST-Elaboration", 
//		"REQUEST-Illustration", 
//		"REQUEST-Information", 
//		"REQUEST-Justification", 
//		"REQUEST-Opinion" 
		
	};

		[TestMethod]
		public void TestCategorization()
		{
			//String filename = args[0];

			//BufferedReader br = new BufferedReader(new InputStreamReader(new FileInputStream(filename))); //, "ANSI"));

			//PrintStream ps = new PrintStream(filename.substring(0, filename.lastIndexOf(".")) + ".cat"); //, "ANSI");

			//String s = br.readLine();

			//while (s != null)
			//{
			//    s = s.Replace(';', ' ');

			//    String category = Categorizer.Categorize(categories, s);

			//    ps.println(s + ";" + category);

			//    s = br.readLine();
			//}

			//br.close();

			string s = "Hola esto es una prueba de categorización";

			String speechAct = SpeechActClassifier.Classify(speechActs, "GRÀCIES!").FirstOrDefault();

			speechAct = SpeechActClassifier.Classify(speechActs, s).FirstOrDefault();

			Console.WriteLine(s);
		}
	}
}
