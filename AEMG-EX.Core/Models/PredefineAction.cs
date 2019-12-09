using Data;
using System.Collections.Generic;

namespace AEMG_EX.Core
{
    public class PredefineAction
    {
        public PredefineAction(IList<IAction> list)
        {
            ActionSequence = new List<IAction>(list);
        }
        public IList<IAction> ActionSequence { get; set; }
    }
}
