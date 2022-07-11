using GPeerToPeer.Client;

namespace PTPFileSender
{
    public static class PTPConnector
    {
        private static PTPClient client = new PTPClient("193.61.3.168", 22345, 22345);
        static PTPConnector()
        {
            client.ReceiveMessageFrom += Client_ReceiveMessageFrom;
        }

        private static void Client_ReceiveMessageFrom(byte[] message, PTPNode node)
        {

        }
    }
}
