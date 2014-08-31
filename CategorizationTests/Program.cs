using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VCS
{
	class Program
	{
		static void Main(string[] args)
		{
			ModelGenerator.GenerateModel();

			Console.WriteLine("categoria1: " + String.Join(", ", SpeechActClassifier.Classify(new string[] { "CAT1", "CAT2" }, "categoria1")));
			Console.WriteLine("categoria2: " + String.Join(", ", SpeechActClassifier.Classify(new string[] { "CAT1", "CAT2" }, "categoria2")));
			Console.WriteLine("categoria3: " + String.Join(", ", SpeechActClassifier.Classify(new string[] { "CAT1", "CAT2" }, "categoria3")));

			Console.WriteLine("categoria1 categoria2: " + String.Join(", ", SpeechActClassifier.Classify(new string[] { "CAT1", "CAT2" }, "categoria1 categoria2")));
			Console.WriteLine("categoria2 categoria1: " + String.Join(", ", SpeechActClassifier.Classify(new string[] { "CAT1", "CAT2" }, "categoria2 categoria1")));

			Console.WriteLine("cat1: " + String.Join(", ", SpeechActClassifier.Classify(new string[] { "CAT1", "CAT2" }, "cat1")));
			Console.WriteLine("cat2: " + String.Join(", ", SpeechActClassifier.Classify(new string[] { "CAT1", "CAT2" }, "cat2")));
			Console.WriteLine("cat3: " + String.Join(", ", SpeechActClassifier.Classify(new string[] { "CAT1", "CAT2" }, "cat3")));

			Console.WriteLine("categoria1 cat1: " + String.Join(", ", SpeechActClassifier.Classify(new string[] { "CAT1", "CAT2" }, "categoria1 cat1")));
			Console.WriteLine("categoria2 cat2: " + String.Join(", ", SpeechActClassifier.Classify(new string[] { "CAT1", "CAT2" }, "categoria2 cat2")));
			Console.WriteLine("cat1 categoria1: " + String.Join(", ", SpeechActClassifier.Classify(new string[] { "CAT1", "CAT2" }, "cat1 categoria1")));
			Console.WriteLine("cat2 categoria2: " + String.Join(", ", SpeechActClassifier.Classify(new string[] { "CAT1", "CAT2" }, "cat2 categoria2")));

			Console.WriteLine("categoria1 cat2: " + String.Join(", ", SpeechActClassifier.Classify(new string[] { "CAT1", "CAT2" }, "categoria1 cat2")));
			Console.WriteLine("categoria2 cat1: " + String.Join(", ", SpeechActClassifier.Classify(new string[] { "CAT1", "CAT2" }, "categoria2 cat1")));
			Console.WriteLine("cat2 categoria1: " + String.Join(", ", SpeechActClassifier.Classify(new string[] { "CAT1", "CAT2" }, "cat2 categoria1")));
			Console.WriteLine("cat1 categoria2: " + String.Join(", ", SpeechActClassifier.Classify(new string[] { "CAT1", "CAT2" }, "cat1 categoria2")));

			Console.WriteLine("categoria3 cat2: " + String.Join(", ", SpeechActClassifier.Classify(new string[] { "CAT1", "CAT2" }, "categoria3 cat2")));
			Console.WriteLine("categoria2 cat3: " + String.Join(", ", SpeechActClassifier.Classify(new string[] { "CAT1", "CAT2" }, "categoria2 cat3")));
			Console.WriteLine("cat2 categoria3: " + String.Join(", ", SpeechActClassifier.Classify(new string[] { "CAT1", "CAT2" }, "cat2 categoria3")));
			Console.WriteLine("cat3 categoria2: " + String.Join(", ", SpeechActClassifier.Classify(new string[] { "CAT1", "CAT2" }, "cat3 categoria2")));

			Console.ReadLine();
		}
	}
}
