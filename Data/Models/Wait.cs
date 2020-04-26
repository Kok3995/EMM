using System.Text;

namespace Data
{
    /// <summary>
    /// Define Wait Time
    /// </summary>
    public class Wait : IAction
    {
        /// <summary>
        /// This is wait action
        /// </summary>
        public BasicAction BasicAction { get => BasicAction.Wait; set { } }

        /// <summary>
        /// Action's Description
        /// </summary>
        public string ActionDescription { get; set; }

        /// <summary>
        /// Time to wait for
        /// </summary>
        public int WaitTime { get; set; }

        /// <summary>
        /// True to disable this action
        /// </summary>
        public bool IsDisable { get; set; }
    }
}
