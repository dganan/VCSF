using System;
using System.Collections.Generic;
using System.Text;
using System.Speech.Synthesis;
using SpeechLib;
using System.IO;

namespace SpeechTest
{
	public class Example1
	{
		public static void Speak(string text, VoiceGender gender, VoiceAge age)
		{
			SpVoice t2s = new SpVoice();

			var voices = t2s.GetVoices();

			List<string> ids = new List<string>();
			List<string> desc = new List<string>();
			List<string> attr = new List<string>();

			for (int i = 0; i < voices.Count; i++)
			{
				ids.Add(voices.Item(i).Id);
				desc.Add(voices.Item(i).GetDescription());
				attr.Add(voices.Item(i).GetAttribute("Gender"));
			}

			t2s.Voice = voices.Item(5); // Ingles
			//t2s.Voice = voices.Item(8); // Castellano
			//t2s.Voice = voices.Item(7); // Catalan

			//t2s.Rate = 1;

			t2s.Volume = 100;

			SpFileStream stream = new SpFileStream();

			stream.Format.Type = SpeechAudioFormatType.SAFT44kHz16BitMono;

			stream.Open("audio_1.wav", SpeechStreamFileMode.SSFMCreateForWrite);

			t2s.AudioOutputStream = stream;

			t2s.Speak(text, SpeechVoiceSpeakFlags.SVSFlagsAsync);

			t2s.WaitUntilDone(System.Threading.Timeout.Infinite);

			//stream.Seek(0, SpeechStreamSeekPositionType.SSSPTRelativeToStart);
			
			//byte[] buffer = (byte[])stream.GetData();

			//using (MemoryStream memoryStream = new MemoryStream())
			//{
			//    BinaryWriter writer = new BinaryWriter(memoryStream);

			//    HeaderWrite(writer, false, 16, buffer.Length / 2, 44100);
			//    writer.Write(buffer);

			//    return memoryStream.ToArray();
			//}
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
	}
}
