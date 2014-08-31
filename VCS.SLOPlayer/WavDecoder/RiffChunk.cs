using System;

namespace VCS.WavDecoder
{
    public struct RiffChunk
    {
        public FourCC fcc;
        public UInt32 cb;
        public FourCC fccList;

        public FourCC FCC
        {
            get { return fcc; }
        }

        public UInt32 DataSize
        {
            get { return cb; }
        }

        public bool IsList()
        {
            return (fcc == FourCC.List);
        }
        public FourCC FCCList
        {
            get
            {
                if (IsList())
                {
                    return fccList;
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
        }
    }
}
