using System.Threading.Tasks;

namespace PTPFileSender.Controllers
{
    internal interface IUploadController
    {
        Task<bool> ConnectNode(string key);
        string ChooseFile();
        void UploadFile();
        delegate void MoveProgressBarHandler(double percent);
        event MoveProgressBarHandler MoveProgressBar;
    }
}
