using GPeerToPeer.Client;
using System.Threading.Tasks;

namespace PTPFileSender.Services
{
    internal static class PTPService
    {
        private static PTPClient client;
        static PTPService()
        {
            client = new PTPClient("194.61.3.168", 22345, 22345);
        }
        public static async Task<bool> ConnectNode(string key)
        {
            return await client.ReachConnectionAsync(new PTPNode(key));
        }
        public static string GetSelfKey()
        {
            return client.selfNode.Key;
        }
    }
}
