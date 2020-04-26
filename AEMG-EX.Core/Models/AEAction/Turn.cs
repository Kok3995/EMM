using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AEMG_EX.Core
{
    public class Turn
    {
        public AEAction TurnType { get; set; }

        public List<Character> Frontline { get; set; }

        /// <summary>
        /// list of back line chars
        /// </summary>
        public List<Character> Backline { get; set; }

        /// <summary>
        /// True if AF is clicked, toggle the second time
        /// </summary>
        public bool IsAF { get; set; }

        /// <summary>
        /// Temporary Count frontline number to disable frontline button appropriately
        /// </summary>
        public int FrontlineTempCount { get; set; }

        /// <summary>
        /// How long should AF last. Default to 15 seconds
        /// </summary>
        public int AFTime { get; set; }

        /// <summary>
        /// Time to wait for next turn. Default: 8s for normal battle, 15s for boss
        /// </summary>
        public virtual int WaitNextTurnTime { get; set; }
    }
}
