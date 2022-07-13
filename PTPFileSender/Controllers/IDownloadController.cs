using GPeerToPeer.Client;

namespace PTPFileSender.Controllers
{
    internal interface IDownloadController : IWindowEvents
    {
        void DownloadFile(PTPNode node, bool isDownload);
        event MoveProgressBarHandler MoveProgressBar;
    }
}
