using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Speech.Synthesis;
using System.IO;
using System.IO.Compression;
using SpeechLib;

namespace VoicesVideo
{
	public static class Speech2
	{
		public static void Speak(string text, string filename)
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

			t2s.Voice = voices.Item(0); // Ingles
			//t2s.Voice = voices.Item(8); // Castellano
			//t2s.Voice = voices.Item(7); // Catalan

			t2s.Rate = -2;

			t2s.Volume = 100;
			
			SpFileStream stream = new SpFileStream();

			stream.Format.Type = SpeechAudioFormatType.SAFT16kHz16BitStereo;
			
			stream.Open(filename, SpeechStreamFileMode.SSFMCreateForWrite);

			t2s.AudioOutputStream = stream;

			t2s.Speak(text, SpeechVoiceSpeakFlags.SVSFlagsAsync);

			t2s.WaitUntilDone(System.Threading.Timeout.Infinite);
		}
	}
}
