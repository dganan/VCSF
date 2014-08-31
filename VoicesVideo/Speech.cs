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
	public static class Speech
	{
		public static byte[] Speak(string text)
		{
			SpVoice speech = new SpVoice();

			var voices = speech.GetVoices();

			if (voices.Count == 0) return null;

			int selectedVoice = -1;

			for (int i = 0; i < voices.Count && selectedVoice < 0; i++)
			{
				if (voices.Item(i).GetAttribute("Language") == "809")
				{
					selectedVoice = i;
				}
			}

			if (selectedVoice < 0)
			{
				selectedVoice = 0;
			}

			speech.Voice = voices.Item(selectedVoice);

			speech.Rate = 0;
			speech.Priority = SpeechVoicePriority.SVPOver;

			speech.Volume = 100;

			SpMemoryStream stream = new SpMemoryStream();

			stream.Format.Type = SpeechAudioFormatType.SAFT48kHz16BitStereo;
				//.SAFT48kHz16BitMono;

			speech.AudioOutputStream = stream;

			speech.Speak(text, SpeechVoiceSpeakFlags.SVSFlagsAsync);

			speech.WaitUntilDone(System.Threading.Timeout.Infinite);

			stream.Seek(0, SpeechStreamSeekPositionType.SSSPTRelativeToStart);

			byte[] buffer = (byte[])stream.GetData();

			using (MemoryStream memoryStream = new MemoryStream())
			{
				BinaryWriter writer = new BinaryWriter(memoryStream);

				//HeaderWrite(writer, false, 16, buffer.Length / 2, 44100);
				HeaderWrite(writer, true, 16, buffer.Length / 2, 48000);
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
	}
}
