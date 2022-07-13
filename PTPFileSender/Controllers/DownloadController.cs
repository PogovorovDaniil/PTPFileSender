﻿using GPeerToPeer.Client;
using Microsoft.Win32;
using PTPFileSender.Models;
using PTPFileSender.Services;
using System.Threading.Tasks;

namespace PTPFileSender.Controllers
{
    internal class DownloadController : IDownloadController
    {
        public event IWindowEvents.MoveProgressBarHandler MoveProgressBar;
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
                Task.Run(() => LoadFileService.DownloadProcess(fileInformation, path, isDownload, node, MoveProgressBar));
            }
        }
    }
}
