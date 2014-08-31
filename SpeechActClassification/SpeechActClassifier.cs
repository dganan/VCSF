using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using java.io;
//import java.net.URL;
//import java.util.Date;
//import java.util.Enumeration;
//import java.util.Random;

using weka.classifiers;
//import weka.filters.Filter;
//import weka.filters.unsupervised.attribute.StringToNominal;
//import weka.filters.unsupervised.attribute.StringToWordVector;
using weka.core;
using weka.core.converters;
using System.Configuration;
using weka.filters.unsupervised.attribute;
using weka.classifiers.functions;
using weka.classifiers.meta;

namespace VCS
{
	public struct SpeechActProbability
	{
		public string SpeechAct { get; set; }

		public double Probability { get; set; }
	}

	public class SpeechActClassifier
	{
		private static string[] allSpeechActs = new string[] {
		
				"INFORM-Agree-Disagree",
				"INFORM-ExplainClarification-Extend-Lead-Suggest-Elaboration-Justify-State",
				"PROBLEM-Solution-ExtendSolution-AssentSolution",
				"PROBLEM-Statement",
				"REQUEST-Information-Elaboration-Clarification-Justification-Illustration",
				"REQUEST-Opinion",
				"SUPPORT-Greetings-Motivation-Encouragement",
		};

		private static string[] mainSpeechActs = new string[] {
		
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

		public static List<string> GetAllSpeechActs()
		{
			return allSpeechActs.ToList();
		}

		public static List<string> GetMainSpeechActs()
		{
			return mainSpeechActs.ToList();
		}

		private static Instances instances;
		private static FilteredClassifier classifier;
		//    //private static boolean initialized = false;

		private static Classifier LoadClassifier()
		{
			string filename = ConfigurationManager.AppSettings["SpeechActClassificationDirectory"] + "\\SpeechActClassification.model";

			return (Classifier)SerializationHelper.read(filename);
		}

		private static void SaveClassifier(Classifier classifier)
		{
			string filename = ConfigurationManager.AppSettings["SpeechActClassificationDirectory"] + "\\SpeechActClassification.model";

			SerializationHelper.write(filename, classifier);
		}

		public static List<string> Classify(string textToClassify)
		{
			return Classify(mainSpeechActs, textToClassify);
		}

		public static List<string> Classify(String[] speechActs, string textToClassify)
		{
			string speechAct = null;

			List<SpeechActProbability> speechActsProbabilities = new List<SpeechActProbability>();

			try
			{
				//Classifier classifier = LoadClassifier();
				InitializeClassifier();

				Logger.LogMessage("STEP 2");
				Logger.LogMessage("'" + textToClassify + "'");

				//Treure codi HTML (fa falta algo més?)
				textToClassify = textToClassify.Replace("<br />", "").Replace("\n", "");

				// STEP 7 : CREEM UNA INSTANCIA PER A CLASSIFICAR

				//ArffLoader loader = new ArffLoader();

				//File file = new File(ConfigurationManager.AppSettings["SpeechActClassificationDirectory"] + "\\SpeechActClassification.arff");

				//loader.setSource(file);

				//Instances instances = loader.getDataSet();

				//instances.setClassIndex(1);

				Instance iUse = new DenseInstance(instances.numAttributes());
				instances.add(iUse);
				iUse.setDataset(instances);
				iUse.setValue((weka.core.Attribute)instances.attribute(0), textToClassify);
				iUse.setClassMissing();
				
				Logger.LogMessage("STEP 3");

				// STEP 8 : EXECUTAR LA CLASSIFICACIO
				// 			EL RESULTAT ES UN ARRAY AMB LES PROBABILITATS PER CADA CATEGORIA
				//			CAL DETERMINAR QUINA ES LA CATEGORIA AMB MÉS PROBABILITAT
				double[] fDistribution = classifier.distributionForInstance(iUse);
				double speechAct2 = classifier.classifyInstance(iUse);

				Logger.LogMessage("STEP 4");

				int index = 0;

				for (int i = 0; i < fDistribution.Length; i++)
				{
					Logger.LogMessage("Speech act " + speechActs[i] + " - Distribution " + fDistribution[index]);

					if (fDistribution[i] > fDistribution[index])
					{
						index = i;
					}

					speechActsProbabilities.Add (new SpeechActProbability () { SpeechAct = speechActs [i], Probability = fDistribution[i] });
				}

				speechAct = speechActs[(int)speechAct2];

				Logger.LogMessage("SPEECH ACT:" + speechAct);
				Logger.LogMessage("SPEECH ACT 2:" + speechAct2);
			}
			catch (Exception e)
			{
				Logger.LogException(e);
			}

			return speechActsProbabilities.Where (x=>x.Probability > 0.25).OrderByDescending(x=>x.Probability).Select (x=>x.SpeechAct + " (" + x.Probability + ")").ToList();
		}

		private static void InitializeClassifier()
		{
			if (classifier == null)
			{
				try
				{
					ArffLoader loader = new ArffLoader();

					string filename = ConfigurationManager.AppSettings["SpeechActClassificationDirectory"] + "\\SpeechActClassification.arff";

					loader.setSource(new File(filename));

					instances = loader.getDataSet();

					instances.setClassIndex(1);

					StringToWordVector filter = new StringToWordVector();
					filter.setInputFormat(instances);

					// STEP 6 : CREEM UN CLASSIFIER
					Classifier cModel = (Classifier)new SMO();

					classifier = new FilteredClassifier();
					classifier.setFilter(filter);
					classifier.setClassifier(cModel);

					classifier.buildClassifier(instances);

					Logger.LogMessage("INITIALIZED");
				}
				catch (Exception e)
				{
					Logger.LogException(e);
				}
			}
		}

		////
		////	public static String Categorize_WorksFine(String[] categories, String textToCategorize)
		////	{
		////		String category = null;
		////
		////		try
		////		{
		////			InitializeCategorization ();
		////		
		////			Logger.Log("STEP 2"); 
		////			Logger.Log("'" + textToCategorize + "'");
		////			
		////			//Treure codi HTML (fa falta algo més?)
		////			textToCategorize = textToCategorize.replace("<br />", "").replace("\n", "");
		////
		////			// STEP 7 : CREEM UNA INSTANCIA PER A CLASSIFICAR
		////
		////			Instance iUse = new Instance(instances.numAttributes()); 
		////			instances.add(iUse);
		////			iUse.setDataset(instances);                   
		////			iUse.setValue((Attribute) instances.attribute(0), textToCategorize);
		////			iUse.setClassMissing();
		////
		////			Logger.Log("STEP 3"); 
		////
		////			// STEP 8 : EXECUTAR LA CLASSIFICACIO
		////			// 			EL RESULTAT ES UN ARRAY AMB LES PROBABILITATS PER CADA CATEGORIA
		////			//			CAL DETERMINAR QUINA ES LA CATEGORIA AMB MÉS PROBABILITAT
		////			double[] fDistribution = fClass.distributionForInstance(iUse);
		////			double category2 = fClass.classifyInstance(iUse);
		////
		////			Logger.Log("STEP 4"); 
		////
		////			int index = 0;
		////
		////			for (int i = 0; i < fDistribution.length; i++)
		////			{
		////				Logger.Log("Category " + categories[i] + " - Distribution " + fDistribution[index]); 
		////
		////				if (fDistribution[i] > fDistribution[index])
		////				{
		////					index = i;
		////				}
		////			}
		////
		////			category = categories[(int)category2];
		////			
		////			Logger.Log("CATEGORY:" + category);
		////			Logger.Log("CATEGORY2:" + category2);
		////		}
		////		catch (Exception e)
		////		{
		////			Logger.Log(e);
		////		}
		////
		////		return category;
		////	}
	}
}