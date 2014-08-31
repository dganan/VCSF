using System;
using System.Diagnostics;
using System.IO;

namespace VCS.WavDecoder
{
    public class WavRiffParser : RiffParser
    {
        public WaveFormatEx wfx { get { return _waveFormat; } }
        public Int64 Duration { get { return _duration; } }

        private WaveFormatEx _waveFormat;
        private UInt32 _cbWaveFormat;
        private Int64 _duration;

        public WavRiffParser(Stream stream)
            : base(stream, FourCC.Riff, 0)
        {
            _duration = 0;

            if (RiffType != FourCC.Wave)
            {
                throw new ArgumentException("File is not a WAV file");
            }
        }

        /// <summary>
        /// Parses the RIFF WAVE header.
        /// 
        /// .wav files should look like this:           
        /// RIFF ('WAVE'                                      
        /// 'fmt ' = WaveFormatEx structure             
        /// 'data' = audio data                         
        ///       )                https                           
        /// </summary>
        public void ParseWAVEHeader()
        {
            bool foundData = false;

            try
            {
                while (!foundData)
                {
                    if (Chunk.FCC == FourCC.WavFmt)
                    {
                        ReadFormatBlock();
                    }
                    else if (Chunk.FCC == FourCC.WavData || Chunk.FCC == FourCC.Wavdata)
                    {
                        foundData = true;
                        break;
                    }
                    MoveToNextChunk();
                }
            }
            catch (Exception e)
            {
                if (_waveFormat == null || !foundData)
                {
                    throw new InvalidOperationException("Invalid file format", e);
                }
            }

            _duration = _waveFormat.AudioDurationFromBufferSize(Chunk.DataSize);
        }

        private void ReadFormatBlock()
        {
            try
            {
                Debug.Assert(Chunk.FCC == FourCC.WavFmt);
                Debug.Assert(_waveFormat == null);

                // Some .wav files do not include the cbSize field of the WaveFormatEx 
                // structure. For uncompressed PCM audio, field is always zero. 
                const UInt32 cbMinFormatSize = WaveFormatEx.SizeOf - sizeof(Int16);

                UInt32 cbFormatSize = 0;
                if (Chunk.DataSize < cbMinFormatSize)
                {
                    throw new ArgumentException("File is not a WAV file");
                }

                // Allocate a buffer for the WAVEFORMAT structure.
                cbFormatSize = Chunk.DataSize;

                // We store a WaveFormatEx structure, so our format block must be at 
                // least sizeof(WaveFormatEx) even if the format block in the file
                // is smaller. See note above about cbMinFormatSize.
                _cbWaveFormat = Math.Max(cbFormatSize, WaveFormatEx.SizeOf);

                _waveFormat = new WaveFormatEx();

                byte[] data = ReadDataFromChunk(cbFormatSize);

                // Copy the read data into our WaveFormatEx
                _waveFormat.SetFromByteArray(data);
            }
            catch (Exception e)
            {
                _waveFormat = null;
                _cbWaveFormat = 0;
                throw e;
            }
        }
    }
}
