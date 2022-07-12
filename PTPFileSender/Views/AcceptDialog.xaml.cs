using PTPFileSender.Controllers;
using System.Windows;

namespace PTPFileSender.Views
{
    public partial class AcceptDialog : Window
    {
        private IDownloadController downloadController;
        public AcceptDialog()
        {
            InitializeComponent();
            downloadController = new DownloadController();
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
            downloadController.DownloadFile();
        }

        private void Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
