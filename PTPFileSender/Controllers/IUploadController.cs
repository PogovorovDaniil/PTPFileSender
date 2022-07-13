using System;
using System.Threading.Tasks;

namespace PTPFileSender.Controllers
{
    internal interface IUploadController
    {
        Task<bool> ConnectNode(string key);
        public void DisconnectNode();
        public bool NodeIsConnected();
        string ChooseFile();
        void UploadFile();
        public void GetUploadRequest(object obj, EventArgs e);
        delegate void MoveProgressBarHandler(double percent);
        event MoveProgressBarHandler MoveProgressBar;
    }
}
