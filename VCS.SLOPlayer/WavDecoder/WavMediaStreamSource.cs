using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media;
using VCS.WavDecoder;

namespace VCS.WavDecoder
{
    public class WavMediaStreamSource : MediaStreamSource
    {
        #region Members

        private Stream _stream;
        private WavRiffParser _RiffParser;
        private MediaStreamDescription _audioDescription;
        private long _currentPosition = 0L;
        private long _startPosition;
        private long _currentTimeStamp;
        private Dictionary<MediaSampleAttributeKeys, string> _emptySampleDict = new Dictionary<MediaSampleAttributeKeys, string>();

        #endregion //Members

        #region Constructors

        public WavMediaStreamSource(Stream stream)
        {
            _stream = stream;
        }

        #endregion //Constructors

        #region Base Class Overrides

        protected override void CloseMedia()
        {
            _startPosition = _currentPosition = 0;
            _RiffParser = null;
            _audioDescription = null;
        }

        protected override void GetDiagnosticAsync(MediaStreamSourceDiagnosticKind diagnosticKind)
        {
            throw new NotImplementedException();
        }

        protected override void GetSampleAsync(MediaStreamType mediaStreamType)
        {
            // Start with one second of data, rounded up to the nearest block.
            uint cbBuffer = (uint)AlignUp(
                _RiffParser.wfx.AvgBytesPerSec,
                _RiffParser.wfx.BlockAlign);

            // Figure out how much data we have left in the chunk compared to the
            // data that we need.
            cbBuffer = Math.Min(cbBuffer, (uint)_RiffParser.BytesRemainingInChunk);
            if (cbBuffer > 0)
            {
                _RiffParser.ProcessDataFromChunk(cbBuffer);
                // Send out the next sample
                MediaStreamSample msSamp = new MediaStreamSample(
                    _audioDescription,
                    _stream,
                    _currentPosition,
                    cbBuffer,
                    _currentTimeStamp,
                    _emptySampleDict);

                // Move our timestamp and position forward
                _currentTimeStamp += _RiffParser.wfx.AudioDurationFromBufferSize(cbBuffer);
                _currentPosition += cbBuffer;

                // If there are no more bytes in the chunk, start again from the beginning
                if (_RiffParser.BytesRemainingInChunk == 0)
                {
                    _RiffParser.MoveToStartOfChunk();
                }

                ReportGetSampleCompleted(msSamp);
            }
            else // Report EOS
            {
                ReportGetSampleCompleted(new MediaStreamSample(_audioDescription, null, 0, 0, 0, _emptySampleDict));
            }
        }

        protected override void OpenMediaAsync()
        {
            // Create a parser
            _RiffParser = new WavRiffParser(_stream);

            // Parse the header
            _RiffParser.ParseWAVEHeader();

            _RiffParser.wfx.ValidateWaveFormat();

            _startPosition = _currentPosition = _RiffParser.DataPosition;

            // Init
            Dictionary<MediaStreamAttributeKeys, string> streamAttributes = new Dictionary<MediaStreamAttributeKeys, string>();
            Dictionary<MediaSourceAttributesKeys, string> sourceAttributes = new Dictionary<MediaSourceAttributesKeys, string>();
            List<MediaStreamDescription> availableStreams = new List<MediaStreamDescription>();

            // Stream Description 
            streamAttributes[MediaStreamAttributeKeys.CodecPrivateData] = _RiffParser.wfx.ToHexString(); // wfx
            MediaStreamDescription msd = new MediaStreamDescription(MediaStreamType.Audio, streamAttributes);
            _audioDescription = msd;
            availableStreams.Add(_audioDescription);

            sourceAttributes[MediaSourceAttributesKeys.Duration] = _RiffParser.Duration.ToString();
            ReportOpenMediaCompleted(sourceAttributes, availableStreams);
        }

        protected override void SeekAsync(long seekToTime)
        {
            _currentPosition = _RiffParser.wfx.BufferSizeFromAudioDuration(seekToTime);
            _currentTimeStamp = seekToTime;
            ReportSeekCompleted(seekToTime);
        }

        protected override void SwitchMediaStreamAsync(MediaStreamDescription mediaStreamDescription)
        {
            throw new NotImplementedException();
        }

        #endregion //Base Class Overrides

        #region Helper Methods

        private static int AlignUp(int a, int b)
        {
            int tmp = a + b - 1;
            return tmp - (tmp % b);
        }

        #endregion //Helper Methods
    }
}
