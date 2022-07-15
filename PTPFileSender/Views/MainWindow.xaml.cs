using PTPFileSender.Constants;
using PTPFileSender.Controllers;
using PTPFileSender.Helpers;
using PTPFileSender.Services;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace PTPFileSender.Views
{
    public partial class MainWindow : Window
    {
        private IUploadController uploadController;
        public MainWindow()
        {
            InitializeComponent();
            uploadController = new UploadController(this);
            uploadController.MoveProgressBar += UploadController_MoveProgressBar;

            string selfKey = PeerToPeerService.GetSelfKey();
            SelfKey_TextBox.Content = selfKey;

            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += uploadController.GetUploadRequest;
            dispatcherTimer.Interval = TimeSpan.FromMilliseconds(100);
            dispatcherTimer.Start();
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
                    button.Content = Str.Connect;

                }
                else
                {
                    string key = NodeKey_TextBox.Text;
                    if (await uploadController.ConnectNode(key))
                    {
                        NodeKey_TextBox.IsEnabled = false;
                        button.Content = Str.Disconnect;
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
        private async void SendFile_Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                if (LoadFileService.IsProcess)
                {
                    LoadFileService.StopProcess();
                    button.IsEnabled = false;
                }
                else
                {
                    button.Content = Str.Cancel;
                    await uploadController.UploadFile();
                    button.Content = Str.Send;
                    Upload_ProgressBar.Value = 0;
                    button.IsEnabled = true;
                }
            }
        }

        private async void CopyKey_Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                string selfKey = PeerToPeerService.GetSelfKey();
                Clipboard.SetText(selfKey);
                button.Content = string.Format(Str.Copied, selfKey);
                button.IsEnabled = false;
                await Task.Delay(2000);
                button.Content = selfKey;
                button.IsEnabled = true;
            }
        }
    }
}
