using System;

namespace PTPFileSender.Helpers
{
    internal static class FileHelper
    {
        public static string NameFromPath(string path)
        {
            path = path.Replace("\\", "/");
            string[] nodes = path.Split("/");
            return nodes[nodes.Length - 1];
        }
    }
}
