using System.IO;

namespace PTPFileSender.Services
{
    internal static class LoadFileService
    {
        private static bool[] received;
        public static void UploadProcess(string path)
        {
            FileInfo fileInfo = new FileInfo(path);
            FileStream fs = fileInfo.OpenRead();
        }
        public static void DownloadProcess(string path)
        {
            FileStream fs = File.Create(path);
            fs.SetLength(0);
        }
        public static uint GetBytes(out byte[] bytes)
        {
            bytes = new byte[0];
            return 0;
        }
    }
}
