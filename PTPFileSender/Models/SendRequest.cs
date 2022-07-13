using GPeerToPeer.Constants;

namespace PTPFileSender.Models
{
    internal class SendRequest : IPacket
    {
        public byte Request { get; set; }
        public void GetFromBytes(byte[] bytes)
        {
            Request = bytes[0];
        }
        public byte[] GetBytes()
        {
            return new byte[] { Request };
        }
        public byte GetChannel()
        {
            return Channel.CHANNEL4;
        }
    }
}
