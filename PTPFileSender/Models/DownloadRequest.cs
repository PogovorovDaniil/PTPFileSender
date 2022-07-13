using GPeerToPeer.Constants;

namespace PTPFileSender.Models
{
    internal class DownloadRequest : IPacket
    {
        public bool IsDownload { get; set; }
        public void GetFromBytes(byte[] bytes)
        {
            IsDownload = bytes[0] > 0;
        }
        public byte[] GetBytes()
        {
            return new byte[] { IsDownload ? (byte)1 : (byte)0 };
        }
        public byte GetChannel()
        {
            return Channel.CHANNEL4;
        }
    }
}