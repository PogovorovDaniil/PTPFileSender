using PTPFileSender.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PTPFileSender
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
