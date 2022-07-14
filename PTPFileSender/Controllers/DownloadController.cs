using GPeerToPeer.Client;
using Microsoft.Win32;
using PTPFileSender.Constants;
using PTPFileSender.Models;
using PTPFileSender.Services;
using System.Threading.Tasks;
using System.Windows;

namespace PTPFileSender.Controllers
{
    internal class DownloadController : IDownloadController
    {
        public event IWindowEvents.MoveProgressBarHandler MoveProgressBar;
        private Window window;
        private FileInformation fileInformation;
        public DownloadController(Window window, FileInformation fileInformation)
        {
            this.fileInformation = fileInformation;
            this.window = window;
        }
        private async Task<(bool, string)> SaveFile()
        {
            return await window.Dispatcher.InvokeAsync(() =>
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                string[] splittedName = fileInformation.FileName.Split('.');
                saveFileDialog.FileName = splittedName[0];
                if (splittedName.Length > 1) saveFileDialog.DefaultExt = splittedName[1];
                return (saveFileDialog.ShowDialog() ?? false, saveFileDialog.FileName);
            });
        }
        public async Task DownloadFile(PTPNode node)
        {
            (bool dialogResult, string path) = await SaveFile();
            if (dialogResult)
            {
                ProcessResult result = await Task.Run(() => LoadFileService.DownloadProcess(fileInformation, path, node, MoveProgressBar));
                window.Dispatcher.Invoke(() =>
                {
                    switch (result)
                    {
                        case ProcessResult.OK:
                            MessageBox.Show(Str.FileLoaded);
                            break;
                        case ProcessResult.Canceled:
                            MessageBox.Show(Str.ProcessCaneled);
                            break;
                        case ProcessResult.Locked:
                            MessageBox.Show(Str.ProcessLocked);
                            break;
                        case ProcessResult.Lost:
                            MessageBox.Show(Str.ConnectLost);
                            break;
                    }
                });
            }
        }
    }
}
