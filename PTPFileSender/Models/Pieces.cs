using GPeerToPeer.Constants;
using System;

namespace PTPFileSender.Models
{
    public class Pieces : IPacket
    {
        public const int PIECE_COUNT = 256;
        public uint[] PieceIndexes { get; set; }
        public void GetFromBytes(byte[] bytes)
        {
            PieceIndexes = new uint[bytes.Length / 4];
            for (int i = 0; i < PieceIndexes.Length; i++)
            {
                PieceIndexes[i] = BitConverter.ToUInt32(bytes, i * 4);
            }
        }
        public byte[] GetBytes()
        {
            byte[] bytes = new byte[PieceIndexes.Length * 4];
            for (int i = 0; i < PieceIndexes.Length; i++)
            {
                byte[] buffer = BitConverter.GetBytes(PieceIndexes[i]);
                Array.ConstrainedCopy(buffer, 0, bytes, i * 4, 4);
            }
            return bytes;
        }
        public byte GetChannel()
        {
            return Channel.CHANNEL2;
        }
    }
}
