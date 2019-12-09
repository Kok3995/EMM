using System.Text;

namespace Data
{
    public interface IAction : IModel
    {
        /// <summary>
        /// Basic action: click, swipe, wait, ae
        /// </summary>
        BasicAction BasicAction { get; set; }

        string ActionDescription { get; set; }        

        /// <summary>
        /// Generate the action script
        /// </summary>
        /// <param name="timer">the timer</param>
        StringBuilder GenerateAction(ref int timer);
    }
}
