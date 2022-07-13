namespace PTPFileSender.Models
{
    internal interface IPacket
    {
        void GetFromBytes(byte[] bytes);
        byte[] GetBytes();
        byte GetChannel();
    }
}
