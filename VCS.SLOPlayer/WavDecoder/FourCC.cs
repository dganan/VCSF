
namespace VCS.WavDecoder
{
    public enum FourCC
    {
        Wave = 0x45564157, //FCC.FourCC('W', 'A', 'V', 'E'),
        WavFmt = 0x20746d66, //FCC.FourCC('f', 'm', 't', ' '),
        WavData = 0x41544144, //FCC.FourCC('D', 'A', 'T', 'A'),
        Wavdata = 0x61746164, //FCC.FourCC('D', 'A', 'T', 'A'),
        Riff = 0x46464952, //FCC.FourCC('R', 'I', 'F', 'F'),
        List = 0x5453494c, //FCC.FourCC('L', 'I', 'S', 'T'),
    }
}
