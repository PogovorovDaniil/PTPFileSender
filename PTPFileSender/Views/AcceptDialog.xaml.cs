using GPeerToPeer.Client;
using PTPFileSender.Constants;
using PTPFileSender.Controllers;
using PTPFileSender.Models;
using PTPFileSender.Services;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace PTPFileSender.Views
{
    public partial class AcceptDialog : Window
    {
        private IDownloadController downloadController;
        private PTPNode node;
        public AcceptDialog(FileInformation fileInformation, PTPNode node)
        {
            InitializeComponent();
            downloadController = new DownloadController(this, fileInformation);
            this.node = node;
            downloadController.MoveProgressBar += DownloadController_MoveProgressBar;
            Task.Run(AcceptDialog_Initialized);
        }

        private async void AcceptDialog_Initialized()
        {
            await downloadController.DownloadFile(node);
            Dispatcher.Invoke(() =>
            {
                DialogResult = true;
            });
        }

        private void DownloadController_MoveProgressBar(double percent)
        {
            Download_ProgressBar.Dispatcher.Invoke(() => { 
                Download_ProgressBar.Value = percent;
            });
        }
        private void Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            LoadFileService.StopProcess();
            DialogResult = false;
        }
    }
}
