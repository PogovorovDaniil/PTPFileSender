using GPeerToPeer.Client;
using System.Threading.Tasks;

namespace PTPFileSender.Controllers
{
    internal interface IDownloadController : IWindowEvents
    {
        Task<bool> DownloadFile(PTPNode node);
        event MoveProgressBarHandler MoveProgressBar;
    }
}
