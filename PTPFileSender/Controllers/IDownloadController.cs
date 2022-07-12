namespace PTPFileSender.Controllers
{
    internal interface IDownloadController
    {
        void DownloadFile();
        delegate void MoveProgressBarHandler(double percent);
        event MoveProgressBarHandler MoveProgressBar;
    }
}
