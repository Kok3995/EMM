using System.Windows;

namespace EMM.Core.Service
{
    public class MessageBoxService : IMessageBoxService
    {
        public MessageResult ShowMessageBox(string message, string caption, MessageButton messageButton, MessageImage MessageImage, MessageResult messageResult)
        {
            MessageBoxButton button = (MessageBoxButton)messageButton;
            MessageBoxImage image = (MessageBoxImage)MessageImage;
            MessageBoxResult result = (MessageBoxResult)messageResult;

            return (MessageResult)MessageBox.Show(message, caption, button, image, result, MessageBoxOptions.ServiceNotification);
        }
    }
}
