using GPeerToPeer.Client;
using PTPFileSender.Models;
using System.Threading.Tasks;

namespace PTPFileSender.Services
{
    internal static class PeerToPeerService
    {
        private static PTPClient client;
        static PeerToPeerService()
        {
            client = new PTPClient("194.61.3.168", 22345, 0);
            Task.Run(client.Work);
        }
        public static async Task<bool> ConnectNode(string key)
        {
            return await client.ReachConnectionAsync(key);
        }
        public static string GetSelfKey()
        {
            return client.selfNode.Key;
        }

        public static bool GetFast<T>(out T value, PTPNode node) where T : IPacket, new()
        {
            value = new T();
            if (client.ReceiveMessageWithoutConfirmationFrom(out PTPNode nodeFrom, out byte[] bytes, value.GetChannel()))
            {
                if (nodeFrom.Key != node.Key) return false;
                value.GetFromBytes(bytes);
                return true;
            }
            return false;
        }
        public static void SendFast<T>(T value, PTPNode node) where T : IPacket
        {
            byte[] bytes = value.GetBytes();
            client.SendMessageWithoutConfirmationTo(node, bytes, value.GetChannel());
        }

        public static bool Get<T>(out T value, PTPNode node) where T : IPacket, new()
        {
            value = new T();
            if (client.ReceiveMessageFrom(out PTPNode nodeFrom, out byte[] bytes, value.GetChannel()))
            {
                if (nodeFrom.Key != node.Key) return false;
                value.GetFromBytes(bytes);
                return true;
            }
            return false;
        }
        public static void Send<T>(T value, PTPNode node) where T : IPacket
        {
            byte[] bytes = value.GetBytes();
            client.SendMessageTo(node, bytes, value.GetChannel());
        }
    }
}
