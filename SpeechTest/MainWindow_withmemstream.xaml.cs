using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

		public static byte[] Speak2(string text)
		{
			SpeechSynthesizer ss = new SpeechSynthesizer();

			MemoryStream ms = new MemoryStream();

			ss.SetOutputToWaveStream(ms);

			ss.Speak(text);

			return ms.ToArray();
		}

		public byte [] Speak(string text, VoiceGender gender, VoiceAge age)
		{
			SpVoice t2s = new SpVoice();

			var voices = t2s.GetVoices ();

			List<string> ids = new List<string>();
			List<string> desc = new List<string>();
			List<string> attr = new List<string>();

			for (int i = 0;i<voices.Count;i++)
			{
				ids.Add(voices.Item(i).Id);
				desc.Add(voices.Item(i).GetDescription());
				attr.Add(voices.Item(i).GetAttribute("Gender"));
			}

			t2s.Voice = voices.Item(3);

			//t2s.Rate = 1;

			t2s.Volume = 50;
			
			SpMemoryStream stream = new SpMemoryStream();
			
			t2s.AudioOutputStream = stream;

			stream.Format.Type = SpeechAudioFormatType.SAFT48kHz16BitMono;

			t2s.Speak("Rise and shine, you human being!", SpeechVoiceSpeakFlags.SVSFlagsAsync);

			t2s.WaitUntilDone(System.Threading.Timeout.Infinite);

			stream.Seek(0, SpeechStreamSeekPositionType.SSSPTRelativeToStart);

			byte[] buffer = (byte[])stream.GetData();

			using (MemoryStream memoryStream = new MemoryStream())
			{
				BinaryWriter writer = new BinaryWriter(memoryStream);

				HeaderWrite(writer, false, 16, buffer.Length / 2, 44100);
				writer.Write(buffer);

				return memoryStream.ToArray();
			}
		}

		private static void HeaderWrite(BinaryWriter writer, bool stereo, short bitsPerSample, int numberOfSamples, int sampleRate)
		{
			writer.Write(0x46464952); // "RIFF" in ASCII
			writer.Write((int)(44 + (numberOfSamples * bitsPerSample * (stereo ? 2 : 1) / 8)) - 8);
			writer.Write(0x45564157); // "WAVE" in ASCII

			writer.Write(0x20746d66); // "fmt " in ASCII
			writer.Write(16);
			writer.Write((short)1);
			writer.Write((short)(stereo ? 2 : 1));
			writer.Write(sampleRate);
			writer.Write(sampleRate * (stereo ? 2 : 1) * bitsPerSample / 8);
			writer.Write((short)((stereo ? 2 : 1) * bitsPerSample / 8));
			writer.Write(bitsPerSample);

			writer.Write(0x61746164); // "data" in ASCII
			writer.Write((int)(numberOfSamples * bitsPerSample * (stereo ? 2 : 1) / 8));
		} 

		private void button1_Click(object sender, RoutedEventArgs e)
		{
			byte [] audio = Speak("Hello everybody", VoiceGender.Male, VoiceAge.Adult);

			SaveAudio(audio, "audio.wav");
			PlayAudio(audio, "audio.wav");
		}

		private void PlayAudio(byte[] p, string p_2)
		{
			DateTime dt = DateTime.Now;

			MemoryStream ms = new MemoryStream(p);

			//WavRiffParser parser = new WavRiffParser(ms);

			//parser.ParseWAVEHeader();

			//int duration = (int)Math.Round(parser.Duration / 10000000d, 0);

			//sw.WriteLine("PREV: " + (DateTime.Now - dt).TotalMilliseconds);

			dt = DateTime.Now;

			AudioPlayer.Source = new Uri(p_2, UriKind.Relative);
			AudioPlayer.Play();

			//sw.WriteLine("AFTER: " + (DateTime.Now - dt).TotalMilliseconds);

			//int sleep = (int)(parser.Duration / 10000d) - 600;
			int sleep = 5000;

			//sw.WriteLine("SLEEP: " + sleep);

			Thread.Sleep(sleep);
		}

		private void SaveAudio(byte[] p, string p_2)
		{
			FileStream fs = new FileStream(p_2, FileMode.Create);

			fs.Write(p, 0, p.Length);

			fs.Close();
		}
	}
}
