using GPeerToPeer.Client;
using Microsoft.Win32;
using PTPFileSender.Constants;
using PTPFileSender.Helpers;
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
                (saveFileDialog.FileName, saveFileDialog.DefaultExt) = FileHelper.SplitFileName(fileInformation.FileName);
                saveFileDialog.Filter = $"Оригинальный формат|*.{saveFileDialog.DefaultExt}|Все файлы|*.*";
                return (saveFileDialog.ShowDialog() ?? false, saveFileDialog.FileName);
            });
        }
        public async Task<bool> DownloadFile(PTPNode node)
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
                return result == ProcessResult.OK;
            }
            return false;
        }
    }
}
