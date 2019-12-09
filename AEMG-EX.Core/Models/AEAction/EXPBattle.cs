using System.Collections.Generic;
using Data;

namespace AEMG_EX.Core
{
    public class EXPBattle : IAEAction, IBattle
    {
        public AEAction AEAction => AEAction.EXPBattle;
        public string ActionDescription { get; set; }
        public IList<TurnViewModel> TurnList { get; set; }
    }
}
