using System;

namespace PTPFileSender.Controllers
{
    internal class DownloadController : IDownloadController
    {
        public event IDownloadController.MoveProgressBarHandler MoveProgressBar;

        public void DownloadFile()
        {
            MoveProgressBar?.Invoke(40);
        }
    }
}
