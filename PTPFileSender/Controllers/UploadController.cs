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
                bool connected = await PeerToPeerService.ConnectNode(key);
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
            if(file == null)
            {
                MessageBox.Show(Str.FileNotChoosen);
                return;
            }
            Task.Run(() => LoadFileService.UploadProcess(file.Name, node.Value, MoveProgressBar));
        }

        public void GetUploadRequest(object obj, EventArgs e)
        {
            if(node.HasValue && PeerToPeerService.Get(out FileInformation fileInformation, node.Value))
            {
                AcceptDialog acceptDialog = new AcceptDialog(fileInformation, node.Value);
                acceptDialog.ShowDialog();
            }
        }
    }
}
