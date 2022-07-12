using GPeerToPeer.Client;
using Microsoft.Win32;
using PTPFileSender.Services;
using System;
using System.IO;
using System.Threading.Tasks;

namespace PTPFileSender.Controllers
{
    internal class UploadController : IUploadController
    {
        public event IUploadController.MoveProgressBarHandler MoveProgressBar;

        private PTPNode node;
        FileStream file;

        public UploadController()
        {
            node = default;
            file = null;
        }

        public string ChooseFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() ?? false)
            {
                file = File.OpenRead(openFileDialog.FileName);
                return openFileDialog.FileName;
            }
            return "";
        }

        public async Task<bool> ConnectNode(string key)
        {
            bool connected = await PTPService.ConnectNode(key);
            node = new PTPNode(key);
            return connected;
        }

        public void UploadFile()
        {
            MoveProgressBar?.Invoke(40);
        }
    }
}
