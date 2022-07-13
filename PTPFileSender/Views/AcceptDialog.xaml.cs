using GPeerToPeer.Client;
using PTPFileSender.Controllers;
using PTPFileSender.Models;
using System.Windows;

namespace PTPFileSender.Views
{
    public partial class AcceptDialog : Window
    {
        private IDownloadController downloadController;
        private PTPNode node;
        public AcceptDialog(FileInformation fileInformation, PTPNode node)
        {
            InitializeComponent();
            downloadController = new DownloadController(fileInformation);
            this.node = node;
            downloadController.MoveProgressBar += DownloadController_MoveProgressBar;
        }
        private void DownloadController_MoveProgressBar(double percent)
        {
            Download_ProgressBar.Dispatcher.Invoke(() => { 
                Download_ProgressBar.Value = percent;
            });
        }
        private void SaveFile_Button_Click(object sender, RoutedEventArgs e)
        {
            downloadController.DownloadFile(node, true);
        }
        private void Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
