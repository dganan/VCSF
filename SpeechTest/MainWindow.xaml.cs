using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Speech.Synthesis;
using System.IO;
using System.Threading;

using SpeechLib;
using VCS.WavDecoder;

namespace SpeechTest
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		public static void Main (string [] args)
		{
			string text = "Hola aixo es una prova de com la veu en castellà parla català";

			SpeechSynthesizer ss = new SpeechSynthesizer();

			StreamWriter sw = new StreamWriter("C:\\voices.txt");

			var voices = ss.GetInstalledVoices();

			foreach (var voice in voices)
			{
				sw.WriteLine(voice.VoiceInfo.Description);
			}

			sw.Close();

			ss.SelectVoice("Cepstral Allison");

			MemoryStream ms = new MemoryStream();

			ss.SetOutputToWaveStream(ms);

			ss.Speak(text);

			SaveAudio(ms.ToArray(), "C:\\audio.wav");





			text = "Hola esto es una prueba de como la voz en catalán habla castellano";

			//string text = "Hola, esto es una prueba en castellano";
			//string text = "Hola, aixo es una prova en català";

			byte[] audio = Speaker.Speak(text);

			//Example1.Speak(text, VoiceGender.Male, VoiceAge.Adult);
			
			//byte[] audio = Example2.Speak(text, VoiceGender.Male, VoiceAge.Adult);

			SaveAudio(audio, "audio_castella_catala.wav");
			
			//PlayAudio(audio, "audio_1.wav");

			//audio = Example3.Speak(text, VoiceGender.Male, VoiceAge.Adult);

			//SaveAudio(audio, "audio_3.wav");
			//PlayAudio(audio, "audio_2.wav");
		}		

		//public static byte[] Speak2(string text)
		//{
		//    SpeechSynthesizer ss = new SpeechSynthesizer();

		//    MemoryStream ms = new MemoryStream();

		//    ss.SetOutputToWaveStream(ms);

		//    ss.Speak(text);

		//    return ms.ToArray();
		//}
		
		private void button1_Click(object sender, RoutedEventArgs e)
		{
			Example1.Speak("Rise and shine, you human being!", VoiceGender.Male, VoiceAge.Adult);

			byte[] audio = Example2.Speak("Rise and shine, you human being!", VoiceGender.Male, VoiceAge.Adult);

			SaveAudio(audio, "audio_2.wav");
			//PlayAudio(audio, "audio_1.wav");

			audio = Example3.Speak("Rise and shine, you human being!", VoiceGender.Male, VoiceAge.Adult);

			SaveAudio(audio, "audio_3.wav");
			//PlayAudio(audio, "audio_2.wav");
		}

		//private static void PlayAudio(byte[] p, string p_2)
		//{
		//    DateTime dt = DateTime.Now;

		//    MemoryStream ms = new MemoryStream(p);

		//    //WavRiffParser parser = new WavRiffParser(ms);

		//    //parser.ParseWAVEHeader();

		//    //int duration = (int)Math.Round(parser.Duration / 10000000d, 0);

		//    //sw.WriteLine("PREV: " + (DateTime.Now - dt).TotalMilliseconds);

		//    dt = DateTime.Now;

		//    AudioPlayer.Source = new Uri(p_2, UriKind.Relative);
		//    AudioPlayer.Play();

		//    //sw.WriteLine("AFTER: " + (DateTime.Now - dt).TotalMilliseconds);

		//    //int sleep = (int)(parser.Duration / 10000d) - 600;
		//    int sleep = 5000;

		//    //sw.WriteLine("SLEEP: " + sleep);

		//    Thread.Sleep(sleep);
		//}

		private static void SaveAudio(byte[] p, string p_2)
		{
			FileStream fs = new FileStream(p_2, FileMode.Create);

			fs.Write(p, 0, p.Length);

			fs.Close();
		}
	}
}
