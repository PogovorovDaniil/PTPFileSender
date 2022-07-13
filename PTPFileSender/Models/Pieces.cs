using GPeerToPeer.Constants;
using System;

namespace PTPFileSender.Models
{
    public class Pieces : IPacket
    {
        public const int PIECE_COUNT = 256;
        public int[] PieceIndexes { get; set; }
        public double Progress { get; set; }
        public void GetFromBytes(byte[] bytes)
        {
            Progress = BitConverter.ToDouble(bytes, 0);
            PieceIndexes = new int[bytes.Length / 4 - 2];
            for (int i = 0; i < PieceIndexes.Length; i++)
            {
                PieceIndexes[i] = BitConverter.ToInt32(bytes, i * 4 + 8);
            }
        }
        public byte[] GetBytes()
        {
            byte[] bytes = new byte[PieceIndexes.Length * 4 + 8];
            Array.ConstrainedCopy(BitConverter.GetBytes(Progress), 0, bytes, 0, 8);
            for (int i = 0; i < PieceIndexes.Length; i++)
            {
                byte[] buffer = BitConverter.GetBytes(PieceIndexes[i]);
                Array.ConstrainedCopy(buffer, 0, bytes, i * 4 + 8, 4);
            }
            return bytes;
        }
        public byte GetChannel()
        {
            return Channel.CHANNEL2;
        }
    }
}
