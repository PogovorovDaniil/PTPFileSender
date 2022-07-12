using GPeerToPeer.Client;
using Microsoft.Win32;
using PTPFileSender.Constants;
using PTPFileSender.Services;
using System.IO;
using System.Threading.Tasks;
using System.Windows;

namespace PTPFileSender.Controllers
{
    internal class UploadController : IUploadController
    {
        public event IUploadController.MoveProgressBarHandler MoveProgressBar;
        private PTPNode? node;
        FileStream file;
        public UploadController()
        {
            node = null;
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
        public bool NodeIsConnected()
        {
            return node.HasValue;
        }
        public async Task<bool> ConnectNode(string key)
        {
            try
            {
                bool connected = await PTPService.ConnectNode(key);
                node = new PTPNode(key);
                return connected;
            }
            catch
            {
                MessageBox.Show(Str.InvalidKeyFormat);
                return false;
            }
        }
        public void DisconnectNode()
        {
            node = null;
        }
        public void UploadFile()
        {
            if (!node.HasValue)
            {
                MessageBox.Show(Str.NodeNotConnected);
                return;
            }
            MoveProgressBar?.Invoke(40);
        }
    }
}
