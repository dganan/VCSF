using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace VCS
{
	class Program
	{
		static void Main(string[] args)
		{
			//string boredom = "Potser necessites un descans, abans de seguir amb la propera escena.";
			//string confussion = "Vaja! S'està possant dificil... Potser hauríem de provar alguna cosa més fàcil.";
			//string enjoyment = "Caram! Sembla que t'ho passes bé!! Continua treballant així...";
			//string flow = "Ben fet!! Ho estàs fent molt bé!! Continua així...";
			//string hopefulness = "Sembla que comences a estar cansat... No ho deixis ara... Tornem-ho a intentar...";
			//string puzzlement = "Vinga! No és tan difícil! Donem-li una altra empenta... a veure que passa.";
			//string relaxation = "Ens està anant molt bé fins ara! Anem a fer una cosa més interessant...";
			//string sadness = "Suposo que no estàs d'humor... Vols continuar? ... o provar una altra cosa?";

			//CreateAudio("boredom.wav", boredom);
			//CreateAudio("confussion.wav", confussion);
			//CreateAudio("enjoyment.wav", enjoyment);
			//CreateAudio("flow.wav", flow);
			//CreateAudio("hopefulness.wav", hopefulness);
			//CreateAudio("puzzlement.wav", puzzlement);
			//CreateAudio("relaxation.wav", relaxation);
			//CreateAudio("sadness.wav", sadness);

			string s1 = "No estic segur de com et sents...";
			string s2 = "A la propera ho faré millor... ";
			string s3 = "A la propera ho faràs millor...";
			string s4 = "...o potser no!";


			string s5 = "Ei! Sembla que no estàs aquí...";
			string s6 = "No t'agraden les meves històries?";
			string s7 = "Per què no ho proves un cop més?";

			CreateAudio("s1.wav", s1);
			CreateAudio("s2.wav", s2);
			CreateAudio("s3.wav", s3);
			CreateAudio("s4.wav", s4);
			CreateAudio("s5.wav", s5);
			CreateAudio("s6.wav", s6);
			CreateAudio("s7.wav", s7);
		}

		private static void CreateAudio(string filename, string text)
		{
			byte[] bytes = Speech.Speak(text, Gender.Female, Age.Adult);

			using (FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.Write))
			{
				BinaryWriter writer = new BinaryWriter(fs);

				writer.Write(bytes);

				writer.Close();
			}
		}
	}
}
