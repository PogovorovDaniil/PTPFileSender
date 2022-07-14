using GPeerToPeer.Constants;

namespace PTPFileSender.Models
{
    internal class CancelRequest : IPacket
    {
        public bool IsCancel { get; set; }
        public void GetFromBytes(byte[] bytes)
        {
            IsCancel = bytes[0] > 0;
        }
        public byte[] GetBytes()
        {
            return new byte[] { IsCancel ? (byte)1 : (byte)0 };
        }
        public byte GetChannel()
        {
            return Channel.CHANNEL6;
        }
    }
}