using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data
{
    /// <summary>
    /// Macro template to save to file
    /// </summary>
    public class MacroTemplate
    {
        /// <summary>
        /// Name of the macro
        /// </summary>
        public string MacroName { get; set; }

        /// <summary>
        /// Get or Set the version of the macro 
        /// </summary>
        public int MacroVersion { get; set; }

        /// <summary>
        /// The template x resolution
        /// </summary>
        public int OriginalX { get; set; }

        /// <summary>
        /// The template y resolution
        /// </summary>
        public int OriginalY { get; set; }

        /// <summary>
        /// List of action group
        /// </summary>
        public List<IAction> ActionGroupList { get; set; }

        public StringBuilder GenerateScript(ref int timer)
        {
            var script = new StringBuilder();

            foreach (var actionGroup in this.ActionGroupList)
            {
                script.Append(actionGroup.GenerateAction(ref timer));
            }

            return script;
        }

    }
}
