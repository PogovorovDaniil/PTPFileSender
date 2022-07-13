using GPeerToPeer.Client;

namespace PTPFileSender.Controllers
{
    internal interface IDownloadController
    {
        void DownloadFile(PTPNode node, bool isDownload);
        delegate void MoveProgressBarHandler(double percent);
        event MoveProgressBarHandler MoveProgressBar;
    }
}
