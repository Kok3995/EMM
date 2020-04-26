using System.Text;

namespace Data
{
    /// <summary>
    /// Define Another Eden specific placeholder
    /// </summary>
    public class AE : IAction
    {
        /// <summary>
        /// This is Another Eden specific Action
        /// </summary>
        public BasicAction BasicAction { get => BasicAction.AE; set { } }

        /// <summary>
        /// Action's Description
        /// </summary>
        public string ActionDescription { get; set; }

        /// <summary>
        /// Another Eden specific action
        /// </summary>
        public AEAction AnotherEdenAction { get; set; }

        /// <summary>
        /// Repeat the action. Use for Battle
        /// </summary>
        public int Repeat { get; set; } = 1;


        /// <summary>
        /// True to disable this action
        /// </summary>
        public bool IsDisable { get; set; }
    }
}
