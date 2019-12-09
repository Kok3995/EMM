namespace EMM.Core
{
    public interface IMessageBoxService
    {
        /// <summary>
        /// Show MessageBox
        /// </summary>
        /// <param name="message">The message</param>
        /// <param name="caption">The Window caption</param>
        /// <param name="messageButton">Specify the button</param>
        /// <param name="MessageImage">Specify the image</param>
        /// <param name="messageResult">Default result</param>
        /// <returns></returns>
        MessageResult ShowMessageBox(string message, string caption = "EMM", MessageButton messageButton = MessageButton.OKCancel, MessageImage MessageImage = MessageImage.None, MessageResult messageResult = MessageResult.None);
    }
}
