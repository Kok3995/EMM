using Data;
using System.Collections.Generic;

namespace AEMG_EX.Core
{
    public interface IAEAction
    {
        /// <summary>
        /// Another Eden specific action
        /// </summary>
        AEAction AEAction { get; }

        /// <summary>
        /// Action's Description
        /// </summary>
        string ActionDescription { get; set; }
    }
}
