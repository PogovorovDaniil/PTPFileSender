﻿using GPeerToPeer.Client;
using System.Threading.Tasks;

namespace PTPFileSender.Controllers
{
    internal interface IDownloadController : IWindowEvents
    {
        Task DownloadFile(PTPNode node, bool isDownload);
        event MoveProgressBarHandler MoveProgressBar;
    }
}
