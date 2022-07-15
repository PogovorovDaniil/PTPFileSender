using GPeerToPeer.Client;
using Microsoft.Win32;
using PTPFileSender.Constants;
using PTPFileSender.Models;
using PTPFileSender.Services;
using PTPFileSender.Views;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;

namespace PTPFileSender.Controllers
{
    internal class UploadController : IUploadController
    {
        public event IWindowEvents.MoveProgressBarHandler MoveProgressBar;
        private PTPNode? node;
        private FileStream file;
        private string path;
        private Window window;
        public UploadController(Window window)
        {
            node = null;
            file = null;
            this.window = window;
        }
        public string ChooseFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() ?? false)
            {
                path = openFileDialog.FileName;
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
                bool connected = await PeerToPeerService.ConnectNode(key);
                if(connected) node = new PTPNode(key);
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
        public async Task UploadFile()
        {
            if (window.Dispatcher.Invoke(() =>
            {
                if (!node.HasValue)
                {
                    MessageBox.Show(Str.NodeNotConnected);
                    return true;
                }
                if (path == null)
                {
                    MessageBox.Show(Str.FileNotChoosen);
                    return true;
                }
                return false;
            })) return;
            ProcessResult result = await Task.Run(() => LoadFileService.UploadProcess(file.Name, node.Value, MoveProgressBar));
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

        public void GetUploadRequest(object obj, EventArgs e)
        {
            if(node.HasValue && PeerToPeerService.Get(out FileInformation fileInformation, node.Value))
            {
                window.Dispatcher.Invoke(() =>
                {
                    AcceptDialog acceptDialog = new AcceptDialog(fileInformation, node.Value);
                    bool? dialogResult = acceptDialog.ShowDialog();
                    if (dialogResult != null) PeerToPeerService.Send(new CancelRequest() { IsCancel = true }, node.Value);
                });
            }
        }
    }
}
