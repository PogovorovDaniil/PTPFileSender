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
                DownloadRequest request;
                while (!PeerToPeerService.Get(out request, node)) ;
                if (request.IsDownload)
                {
                    
                }
            }
        }
        public static void DownloadProcess(FileInformation fileInformation, string path, bool isDownload, PTPNode node)
        {
            PeerToPeerService.Send(new DownloadRequest() { IsDownload = isDownload }, node);
            if (!isDownload) return;

            bool[] received;
            uint[] receivedIndexes;
            string filePath = path;
            using (FileStream fs = File.Create(filePath))
            {
                fs.SetLength(fileInformation.FileSize);
            }
        }
    }
}
