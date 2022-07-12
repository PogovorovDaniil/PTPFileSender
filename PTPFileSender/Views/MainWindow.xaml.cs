using PTPFileSender.Controllers;
using PTPFileSender.Helpers;
using PTPFileSender.Services;
using System.Windows;
using System.Windows.Controls;

namespace PTPFileSender.Views
{
    public partial class MainWindow : Window
    {
        private IUploadController uploadController;
        public MainWindow()
        {
            InitializeComponent();
            uploadController = new UploadController();
            uploadController.MoveProgressBar += UploadController_MoveProgressBar;

            string selfKey = PTPService.GetSelfKey();
            SelfKey_TextBox.Text = selfKey;
        }

        private void UploadController_MoveProgressBar(double percent)
        {
            Upload_ProgressBar.Dispatcher.Invoke(() => {
                Upload_ProgressBar.Value = percent;
            });
        }

        private async void ConnectNode_Button_Click(object sender, RoutedEventArgs e)
        {
            if(sender is Button button)
            {
                button.IsEnabled = false;
                if (uploadController.NodeIsConnected())
                {
                    uploadController.DisconnectNode();
                    NodeKey_TextBox.IsEnabled = true;
                    button.Content = "Подключиться";

                }
                else
                {
                    string key = NodeKey_TextBox.Text;
                    if (await uploadController.ConnectNode(key))
                    {
                        NodeKey_TextBox.IsEnabled = false;
                        button.Content = "Отключиться";
                    }
                }
                button.IsEnabled = true;
            }
        }

        private void ChooseFile_Button_Click(object sender, RoutedEventArgs e)
        {
            if(sender is Button button)
            {
                string path = uploadController.ChooseFile();
                if(path.Length > 0)
                {
                    button.Content = FileHelper.NameFromPath(path);
                }
            }
        }

        private void SendFile_Button_Click(object sender, RoutedEventArgs e)
        {
            uploadController.UploadFile();
        }
    }
}
