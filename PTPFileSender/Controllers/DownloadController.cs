using GPeerToPeer.Client;
using Microsoft.Win32;
using PTPFileSender.Models;
using PTPFileSender.Services;

namespace PTPFileSender.Controllers
{
    internal class DownloadController : IDownloadController
    {
        public event IDownloadController.MoveProgressBarHandler MoveProgressBar;
        private FileInformation fileInformation;
        public DownloadController(FileInformation fileInformation)
        {
            this.fileInformation = fileInformation;
        }
        public void DownloadFile(PTPNode node, bool isDownload)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            string[] splittedName = fileInformation.FileName.Split('.');
            saveFileDialog.FileName = splittedName[0];
            if(splittedName.Length > 1) saveFileDialog.DefaultExt = splittedName[1];
            if(saveFileDialog.ShowDialog() ?? false)
            {
                string path = saveFileDialog.FileName;
                LoadFileService.DownloadProcess(fileInformation, path, isDownload, node);
            }
        }
    }
}
