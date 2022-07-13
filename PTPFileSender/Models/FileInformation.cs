using GPeerToPeer.Constants;
using System;
using System.Text;

namespace PTPFileSender.Models
{
    public class FileInformation : IPacket
    {
        public long FileSize { get; set; }
        public string FileName { get; set; }
        public void GetFromBytes(byte[] bytes)
        {
            byte[] count = new byte[4];
            Array.ConstrainedCopy(bytes, 0, count, 0, count.Length);
            byte[] nameSize = new byte[4];
            Array.ConstrainedCopy(bytes, count.Length, nameSize, 0, nameSize.Length);
            byte[] name = new byte[BitConverter.ToInt32(nameSize)];
            Array.ConstrainedCopy(bytes, count.Length + nameSize.Length, name, 0, name.Length);
            FileSize = BitConverter.ToInt64(count);
            FileName = Encoding.UTF8.GetString(name);
        }
        public byte[] GetBytes()
        {
            byte[] count = BitConverter.GetBytes(FileSize);
            byte[] name = Encoding.UTF8.GetBytes(FileName);
            byte[] nameSize = BitConverter.GetBytes(name.Length);
            byte[] bytes = new byte[name.Length + count.Length + nameSize.Length];
            Array.ConstrainedCopy(count, 0, bytes, 0, count.Length);
            Array.ConstrainedCopy(nameSize, 0, bytes, count.Length, nameSize.Length);
            Array.ConstrainedCopy(name, 0, bytes, count.Length + nameSize.Length, name.Length);
            return bytes;
        }
        public byte GetChannel()
        {
            return Channel.CHANNEL3;
        }
    }
}
