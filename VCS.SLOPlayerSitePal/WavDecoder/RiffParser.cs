using System;
using System.Diagnostics;
using System.IO;

namespace VCS.WavDecoder
{
    public class RiffParser
    {
        private RiffChunk _chunk;
        private FourCC _fccID;
        private FourCC _fccType;
        private Stream _s;
        private BinaryReader _br;
        private UInt32 _bytesRemaining;
        private UInt32 _containerSize;
        private Int64 _containerOffset;
        private Int64 _currentChunkOffset;

        const UInt32 SizeOfRiffList = 12;
        const UInt32 SizeOfRiffChunk = 8;

        public FourCC RiffID { get { return _fccID; } }
        public FourCC RiffType { get { return _fccType; } }
        public RiffChunk Chunk { get { return _chunk; } }
        public Int64 DataPosition { get { return _currentChunkOffset + SizeOfRiffChunk; } }
        public UInt32 BytesRemainingInChunk { get { return _bytesRemaining; } }

        public RiffParser(Stream stream, FourCC id, Int64 startOfContainer)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }

            _fccID = id;
            _fccType = 0;
            _containerOffset = startOfContainer;
            _containerSize = 0;
            _currentChunkOffset = 0;
            _bytesRemaining = 0;

            _s = stream;
            _br = new BinaryReader(_s);

            ReadRiffHeader();
        }

        /// <summary>
        /// Advance to the start of the next chunk and read the chunk header
        /// </summary>
        public void MoveToNextChunk()
        {
            // chunk offset is always bigger than container offset,
            // and both are always non-negative.
            Debug.Assert(_currentChunkOffset > _containerOffset);
            Debug.Assert(_currentChunkOffset >= 0);
            Debug.Assert(_containerOffset >= 0);

            Int64 maxChunkSize;

            // Update current chunk offset to the start of the next chunk
            _currentChunkOffset = _currentChunkOffset + ChunkActualSize();

            // Are we at the end?
            if (_currentChunkOffset - _containerOffset >= _containerSize)
            {
                throw new InvalidOperationException("We are at the end of the steam");
            }

            // Seek to the start of the chunk.
            _s.Position = _currentChunkOffset;

            // Read the header.
            ReadChunkHeader();

            // This chunk cannot be any larger than (container size - (chunk offset - container offset) )
            maxChunkSize = (Int64)_containerSize - (_currentChunkOffset - _containerOffset);

            if (maxChunkSize < ChunkActualSize())
            {
                throw new ArgumentException();
            }

            _bytesRemaining = _chunk.DataSize;
        }

        /// <summary>
        /// Return a parser for a LIST
        /// </summary>
        /// <returns>A new parser for the list</returns>
        public RiffParser EnumerateChunksInList()
        {
            if (!_chunk.IsList())
            {
                throw new InvalidOperationException("The current chunk is not a list");
            }

            return new RiffParser(_s, FourCC.List, _currentChunkOffset);
        }

        /// <summary>
        /// Move the file pointer to the start of the current chunk
        /// </summary>
        public void MoveToStartOfChunk()
        {
            MoveToChunkOffset(0);
        }

        /// <summary>
        /// Move the file pointer to a byte offset from the start of the
        /// current chunk.
        /// </summary>
        /// 
        public void MoveToChunkOffset(UInt32 offset)
        {
            if (offset > _chunk.DataSize)
            {
                throw new ArgumentException("Offset specified is beyond the chunk");
            }

            _s.Position = _currentChunkOffset + offset + SizeOfRiffChunk;
            _bytesRemaining = _chunk.DataSize - offset;
        }

        /// <summary>
        /// Read data from the current chunk. (Starts at the current file ptr.) 
        /// </summary>
        /// <param name="data">The number of bytes we want to read</param>
        /// <returns>The data read</returns>
        public byte[] ReadDataFromChunk(UInt32 count)
        {
            if (count > _bytesRemaining)
            {
                throw new ArgumentException("Trying to read more than the size of the chunk");
            }
            _s.Position = _currentChunkOffset + _chunk.DataSize - _bytesRemaining + SizeOfRiffChunk;

            byte[] data = _br.ReadBytes((Int32)count);
            _bytesRemaining -= (uint)data.Length;

            return data;
        }

        /// <summary>
        /// Process data from a chunk (just like reading without actually getting the data)
        /// </summary>
        /// <param name="count">The number of bytes we want to skip</param>
        /// <returns>The number of bytes we want to skipped</returns>
        public UInt32 ProcessDataFromChunk(UInt32 count)
        {
            if (count > _bytesRemaining)
            {
                throw new ArgumentException("Trying to process more than the size of the chunk");
            }
            _bytesRemaining -= count;

            return count;
        }

        /// <summary>
        /// Read the container header section. (The 'RIFF' or 'LIST' header.)
        /// This method verifies the header is well-formed and caches the
        /// container's FOURCC type.
        /// </summary>
        private void ReadRiffHeader()
        {
            RiffChunk header;
            // Riff chunks must be WORD aligned
            if (!IsAligned(_containerOffset, 2))
            {
                throw new ArgumentException();
            }

            // Offset must be positive.
            if (_containerOffset < 0)
            {
                throw new ArgumentException();
            }

            // Seek to the start of the container.
            _s.Position = _containerOffset;

            // Read the header
            header.fcc = (FourCC)_br.ReadUInt32();
            header.cb = _br.ReadUInt32();
            header.fccList = (FourCC)_br.ReadUInt32();

            // Make sure the header ID matches what the caller expected.
            if (header.fcc != _fccID)
            {
                throw new ArgumentException();
            }

            // The size given in the RIFF header does not include the 8-byte header.
            // However, our _containerOffset is the offset from the start of the
            // header. Therefore our container size = listed size + size of header.

            _containerSize = header.DataSize + SizeOfRiffChunk;
            //Debug.Assert(header.IsList());
            _fccType = header.fccList;

            _currentChunkOffset = _containerOffset + SizeOfRiffList;


            ReadChunkHeader();
        }

        /// <summary>
        /// Reads the chunk header. Caller must ensure that the current file 
        /// pointer is located at the start of the chunk header.
        /// </summary>
        private void ReadChunkHeader()
        {
            _chunk.fcc = (FourCC)_br.ReadUInt32();
            _chunk.cb = _br.ReadUInt32();
            _bytesRemaining = _chunk.DataSize;
        }

        private Int64 ChunkActualSize() { return SizeOfRiffChunk + RiffRound(_chunk.DataSize); }

        static public bool IsAligned(int startIndex, int align)
        {
            return ((startIndex % align) == 0);
        }
        static public bool IsAligned(long startIndex, int align)
        {
            return ((startIndex % align) == 0);
        }

        static public int RiffRound(int count)
        {
            return count + (count & 1);
        }

        static public uint RiffRound(uint count)
        {
            return count + (count & 1);
        }
    }
}
