using GPeerToPeer.Constants;
using System;

namespace PTPFileSender.Models
{
    public class FilePiece : IPacket
    {
        public const int PIECE_SIZE = 1024;
        public uint Location { get; set; }
        public byte[] Piece { get; set; }
        public void GetFromBytes(byte[] bytes)
        {
            byte[] location = new byte[4];
            Array.Copy(bytes, location, location.Length);
            Location = BitConverter.ToUInt32(location);
            Array.ConstrainedCopy(bytes, location.Length, Piece, 0, bytes.Length - location.Length);
        }
        public byte[] GetBytes()
        {
            byte[] location = BitConverter.GetBytes(Location);
            byte[] bytes = new byte[Piece.Length + location.Length];
            Array.Copy(location, bytes, location.Length);
            Array.ConstrainedCopy(Piece, 0, bytes, 4, Piece.Length);
            return bytes;
        }
        public byte GetChannel()
        {
            return Channel.CHANNEL1;
        }
    }
}
