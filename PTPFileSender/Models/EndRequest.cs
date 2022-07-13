using GPeerToPeer.Constants;

namespace PTPFileSender.Models
{
    internal class EndRequest : IPacket
    {
        public bool IsEnd { get; set; }
        public void GetFromBytes(byte[] bytes)
        {
            IsEnd = bytes[0] > 0;
        }
        public byte[] GetBytes()
        {
            return new byte[] { IsEnd ? (byte)1 : (byte)0 };
        }
        public byte GetChannel()
        {
            return Channel.CHANNEL5;
        }
    }
}