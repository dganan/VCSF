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
			
			//t2s.Speak("Rise and shine, you human being!");
						
			//hVoiceSpeakFlags SpFlags = SpeechVoiceSpeakFlags.SVSFlagsAsync;

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

				//memoryStream.Position = 0;

				//SoundPlayer player = new SoundPlayer(memoryStream);

				//player.PlaySync();

				return memoryStream.ToArray();
			}
 


			//SpFileStream fs = new SpFileStream ();
 
			//fs.Open ("audio.wav", SpeechStreamFileMode.SSFMCreateForWrite);

			//t2s.SpeakStream(fs);

			//t2s.WaitUntilDone(0);
			
			////List<InstalledVoice> allInstalledVoices = new SpeechSynthesizer().GetInstalledVoices().ToList(); 

			////SpeechSynthesizer ss = new SpeechSynthesizer();

			////ss.SelectVoice("LH Michael");
			////    //.SelectVoiceByHints(VoiceGender.Male, VoiceAge.Adult); //

			////ss.SelectVoiceByHints(gender, age);
			
			////FileStream fs = new FileStream ("audio.wav", FileMode.Create, FileAccess.Write);

			////// MemoryStream ms = new MemoryStream();
			////ss.SetOutputToWaveStream(fs);

			//////ss.SetOutputToAudioStream(ms, new SpeechAudioFormatInfo(8000, AudioBitsPerSample.Eight, AudioChannel.Mono));

			////ss.Speak(text);

			
			//////SpVoice speech = new SpVoice();

			//////SpMemoryStream spMemStream = new SpMemoryStream();

			//////SpAudioFormat audioFormat = new SpAudioFormat();
			//////spMemStream.Format = audioFormat;
			////////audioFormat.Type = SpeechAudioFormatType.SAFT48kHz16BitMono; //Hard setup!!!
			//////audioFormat.Type = SpeechAudioFormatType.SAFT22kHz16BitMono; //Hard setup!!!

			//////speech.AudioOutputStream = spMemStream;

			////////speech.Rate = speechRate;
			////////speech.Volume = volume;
			//////speech.Speak(text, SpeechVoiceSpeakFlags.SVSFlagsAsync);
			//////speech.WaitUntilDone(Timeout.Infinite);

			//////spMemStream.Seek(0, SpeechStreamSeekPositionType.SSSPTRelativeToStart);

			//////return (byte[])spMemStream.GetData();

			////Thread.Sleep(2000);

			////fs.Close();
			
			//////WavMediaStreamSource audioStream = new WavMediaStreamSource(ms);

			//AudioPlayer.Source = new Uri ("audio.wav", UriKind.Relative);
			//AudioPlayer.Play();

			//////WavRiffParser parser = new WavRiffParser(ms);

			//////parser.ParseWAVEHeader();

			//return audio;
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
			
			//Speak2();
		}

		private void Speak2()
		{
			StreamWriter sw = new StreamWriter("log.txt");

			int count = 4;

			byte[][] audios = new byte[count][];

			audios[0] = Speak2("Hi, this");
			audios[1] = Speak2("is a test");
			audios[2] = Speak2("of");
			audios[3] = Speak2("using phrase splitting");

			//audios[0] = Speak2("Hola esto");
			//audios[1] = Speak2("es una prueba");
			//audios[2] = Speak2("de");
			//audios[3] = Speak2("partir frases");

			//audios[0] = Speak2("Hola aixo");
			//audios[1] = Speak2("es una prova");
			//audios[2] = Speak2("de");
			//audios[3] = Speak2("partir frases");

			for (int i = 0; i < count; i++)
			{
				SaveAudio(audios[i], "audio" + i + ".wav");
			}

			for (int i = 0; i < count; i++)
			{
				PlayAudio(audios[i], "audio" + i + ".wav");
			}

			sw.Close();
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
