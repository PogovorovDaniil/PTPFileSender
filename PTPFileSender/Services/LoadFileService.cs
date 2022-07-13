using GPeerToPeer.Client;
using PTPFileSender.Helpers;
using PTPFileSender.Models;
using System.IO;

namespace PTPFileSender.Services
{
    internal static class LoadFileService
    {
        public static void UploadProcess(string path, PTPNode node)
        {
            FileInfo fileInfo = new FileInfo(path);
            using (FileStream fs = fileInfo.OpenRead())
            {
                FileInformation fileInformation = new FileInformation()
                {
                    FileName = fileInfo.Name,
                    FileSize = fileInfo.Length
                };
                PeerToPeerService.Send(fileInformation, node);
            }
        }
        public static void DownloadProcess(string path, PTPNode node)
        {
            bool[] received;
            uint[] receivedIndexes;

            if (PeerToPeerService.Get(out FileInformation fileInformation, node))
            {
                string filePath = FileHelper.AddNameToPath(path, fileInformation.FileName);
                using (FileStream fs = File.Create(filePath))
                {
                    fs.SetLength(fileInformation.FileSize);
                }
            }
        }
    }
}
