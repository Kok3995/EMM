using System.Collections.Generic;
using System.Text;

namespace Data
{
    /// <summary>
    /// Group actions together
    /// </summary>
    public class ActionGroup : IAction
    {
        public BasicAction BasicAction { get => BasicAction.ActionGroup; set { } }

        public string ActionDescription { get; set; }

        /// <summary>
        /// Repeat this action group
        /// </summary>
        public int Repeat { get; set; } = 1;

        /// <summary>
        /// List of actions within the group
        /// </summary>
        public List<IAction> ActionList { get; set; }

        /// <summary>
        /// True to disable this action
        /// </summary>
        public bool IsDisable { get; set; }

    }
}
