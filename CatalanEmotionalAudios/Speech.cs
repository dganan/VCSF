using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Speech.Synthesis;
using System.IO;
using System.IO.Compression;
//using ICSharpCode.SharpZipLib.Zip;
//using ICSharpCode.SharpZipLib.BZip2;
using SpeechLib;

namespace VCS
{
	public static class Speech
	{
		public static byte[] Speak(string text, Gender gender, Age age)
		{
			SpVoice speech = new SpVoice();

			var voices = speech.GetVoices();

			if (voices.Count == 0) return null;

			int selectedVoice = -1;

			//for (int i = 0; i < voices.Count; i++)
			//{
			//    Logger.LogMessage(voices.Item(i).GetDescription() + " - " + voices.Item(i).GetAttribute("Language"));

			//}

			for (int i = 0; i < voices.Count && selectedVoice < 0; i++)
			{
				// Mejorar para sexo y edad, aunque de momento no hay voces suficientes para ello
				if (voices.Item(i).GetAttribute("Language") == "403")
				{
					selectedVoice = i;
				}
			}

			if (selectedVoice < 0)
			{
				selectedVoice = 0;
			}

			speech.Voice = voices.Item(selectedVoice);

			speech.Rate = 2;
			
			speech.Volume = 100;

			SpMemoryStream stream = new SpMemoryStream();

			stream.Format.Type = SpeechAudioFormatType.SAFT48kHz16BitMono;

			speech.AudioOutputStream = stream;

			speech.Speak(text, SpeechVoiceSpeakFlags.SVSFlagsAsync);

			speech.WaitUntilDone(System.Threading.Timeout.Infinite);

			stream.Seek(0, SpeechStreamSeekPositionType.SSSPTRelativeToStart);

			byte[] buffer = (byte[])stream.GetData();

			using (MemoryStream memoryStream = new MemoryStream())
			{
				BinaryWriter writer = new BinaryWriter(memoryStream);

				HeaderWrite(writer, false, 16, buffer.Length / 2, 44100);
				writer.Write(buffer);

				//return CompressBytes(memoryStream.ToArray());
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

		//public static byte[] SpeakOld(string text, Gender gender, Age age)
		//{
		//    //List<InstalledVoice> allInstalledVoices = new SpeechSynthesizer().GetInstalledVoices().ToList(); 

		//    SpeechSynthesizer ss = new SpeechSynthesizer();

		//    //ss.SelectVoice("MSMary");

		//    ss.SelectVoiceByHints(gender.ToVoiceGender(), age.ToVoiceAge());


		//    MemoryStream ms = new MemoryStream();
		//    ss.SetOutputToWaveStream(ms);

		//    //ss.SetOutputToAudioStream(ms, new SpeechAudioFormatInfo(8000, AudioBitsPerSample.Eight, AudioChannel.Mono));

		//    DateTime dt = DateTime.Now;

		//    ss.Speak(text);

		//    byte[] bytes = CompressBytes(ms.ToArray());

		//    TimeSpan ts = DateTime.Now - dt;

		//    //Logger.LogMessage("Synthesizer takes " + ts.TotalMilliseconds + " milliseconds");
		//    //Logger.LogMessage("Synthesizer takes " + ts.TotalSeconds + " seconds");

		//    return bytes;

		//    //SpVoice speech = new SpVoice();

		//    //SpMemoryStream spMemStream = new SpMemoryStream();

		//    //SpAudioFormat audioFormat = new SpAudioFormat();
		//    //spMemStream.Format = audioFormat;
		//    ////audioFormat.Type = SpeechAudioFormatType.SAFT48kHz16BitMono; //Hard setup!!!
		//    //audioFormat.Type = SpeechAudioFormatType.SAFT22kHz16BitMono; //Hard setup!!!

		//    //speech.AudioOutputStream = spMemStream;

		//    ////speech.Rate = speechRate;
		//    ////speech.Volume = volume;
		//    //speech.Speak(text, SpeechVoiceSpeakFlags.SVSFlagsAsync);
		//    //speech.WaitUntilDone(Timeout.Infinite);

		//    //spMemStream.Seek(0, SpeechStreamSeekPositionType.SSSPTRelativeToStart);

		//    //return (byte[])spMemStream.GetData();
		//}

		//private static byte[] CompressBytes(byte[] bytes)
		//{
		//    MemoryStream ms = new MemoryStream();

		//    BZip2OutputStream bzos = new BZip2OutputStream(ms);

		//    //Logger.LogMessage("bytes size: " + bytes.Length);

		//    bzos.Write(bytes, 0, bytes.Length);

		//    //bzos.Finalize();

		//    bzos.Close();

		//    byte[] compressedBytes = ms.ToArray();

		//    //Logger.LogMessage("compressed bytes size: " + compressedBytes.Length);

		//    return compressedBytes;

		//    //MemoryStream ms = new MemoryStream();

		//    //GZipStream gs = new GZipStream(ms, CompressionMode.Compress);

		//    //gs.Write(bytes, 0, bytes.Length);

		//    //return ms.ToArray();
		//}

		public static VoiceGender ToVoiceGender(this Gender gender)
		{
			switch (gender)
			{
				case Gender.Female: return VoiceGender.Female;
				case Gender.Male: return VoiceGender.Male;
				case Gender.Neutral: return VoiceGender.Neutral;
			}

			return VoiceGender.NotSet;
		}

		public static VoiceAge ToVoiceAge(this Age age)
		{
			switch (age)
			{
				case Age.Adult: return VoiceAge.Adult;
				case Age.Child: return VoiceAge.Child;
				case Age.Senior: return VoiceAge.Senior;
				case Age.Teen: return VoiceAge.Teen;
			}

			return VoiceAge.NotSet;
		}
	}
}
