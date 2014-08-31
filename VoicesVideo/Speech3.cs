using SpeechLib;
using System;
using System.IO;
using System.Threading;
using System.Speech.Synthesis;

namespace VoicesVideo
{
	public class Speech3
	{
		public static void Speak(string text, string filename)
		{
			SpeechSynthesizer ss = new SpeechSynthesizer();
			
			var voices = ss.GetInstalledVoices ();

			voices[1].Enabled = true;

			ss.Rate = -1;
			ss.Volume = 100;
			ss.SelectVoice(voices[1].VoiceInfo.Name);
			//ss.SelectVoiceByHints (VoiceGender.Female, VoiceAge.NotSet);

			ss.SetOutputToWaveFile(filename);

			ss.Speak(text);
		}

		public string[] getInstalledVoices()
		{
			SpVoice speech = new SpVoice();

			ISpeechObjectTokens sot = speech.GetVoices("", "");

			string[] voiceIds = new string[sot.Count];

			for (int i = 0; i < sot.Count; i++)
				voiceIds[i] = sot.Item(i).GetDescription(1033);

			return voiceIds;
		}

		//public void TextToWav(string inputText, string filePath, string voiceIdString)
		//{
		//    try
		//    {
		//        System.Web.HttpContext ctx = System.Web.HttpContext.Current;

		//        if (ctx != null)
		//        {
		//            DirectoryInfo di = new DirectoryInfo(ctx.Server.MapPath("."));

		//            FileInfo[] fi = di.GetFiles("*.wav");

		//            foreach (FileInfo f in fi)
		//                File.Delete(ctx.Server.MapPath(f.Name));
		//        }

		//        SpeechVoiceSpeakFlags SpFlags = SpeechVoiceSpeakFlags.SVSFlagsAsync;

		//        SpVoice speech = new SpVoice();

		//        if (voiceIdString != String.Empty)
		//        {
		//            ISpeechObjectTokens sot = speech.GetVoices("", "");

		//            string[] voiceIds = new string[sot.Count];

		//            for (int i = 0; i < sot.Count; i++)
		//            {
		//                voiceIds[i] = sot.Item(i).GetDescription(1033);

		//                if (voiceIds[i] == voiceIdString)
		//                    speech.Voice = sot.Item(i);
		//            }
		//        }

		//        peechStreamFileMode SpFileMode = SpeechStreamFileMode.SSFMCreateForWrite;

		//        new SpFileStream();

		//        SpFileStream.Format.Type = SpeechAudioFormatType.SAFT11kHz8BitMono;

		//        if (!filePath.ToLower().EndsWith(".wav")) filePath += ".wav";

		//        SpFileStream.Open(filePath, SpFileMode, false); speech.AudioOutputStream = SpFileStream; speech.Speak(inputText, SpFlags); speech.WaitUntilDone(Timeout.Infinite); SpFileStream.Close();
		//    }
		//    catch { throw; }

		//}

		public static byte[] TextToWav(string inputText, string voiceIdString)
		{
			byte[] buffer = null;

			try
			{
				SpeechVoiceSpeakFlags SpFlags = SpeechVoiceSpeakFlags.SVSFlagsAsync;
				
				SpVoice speech = new SpVoice(); 
				
				if (voiceIdString != String.Empty)
				{
					ISpeechObjectTokens sot = speech.GetVoices("", "");
					
					string[] voiceIds = new string[sot.Count];
				
					for (int i = 0; i < sot.Count; i++)
					{
						voiceIds[i] = sot.Item(i).GetDescription(1033);
						if (voiceIds[i] == voiceIdString) speech.Voice = sot.Item(i);
					}
				} 
				
				SpMemoryStream spMemStream = new SpMemoryStream();
				
				spMemStream.Format.Type = SpeechAudioFormatType.SAFT11kHz8BitMono;
				
				object buf = new object(); speech.AudioOutputStream = spMemStream;
				
				int r = speech.Speak(inputText, SpFlags); 
				
				speech.WaitUntilDone(Timeout.Infinite);
				
				spMemStream.Seek(0, SpeechStreamSeekPositionType.SSSPTRelativeToStart); 
				
				buf = spMemStream.GetData();
				
				buffer = (byte[])buf;

				//byte[] buffer = (byte[])stream.GetData();

				using (MemoryStream memoryStream = new MemoryStream())
				{
					BinaryWriter writer = new BinaryWriter(memoryStream);

					HeaderWrite(writer, false, 16, buffer.Length / 2, 44100);
					writer.Write(buffer);
	
					return memoryStream.ToArray();
				}
			}
			catch 
			{
				throw; 
			}
		}

		public static byte [] Speak (string text, VoiceGender gender, VoiceAge age)
		{
			return TextToWav(text, "");
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


