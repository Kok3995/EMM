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

        bool IsDisable { get; set; }
    }
}
