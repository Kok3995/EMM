using System.Collections.Generic;
using Data;

namespace AEMG_EX.Core
{
    public class BossBattle : IAEAction, IBattle
    {
        public AEAction AEAction => AEAction.BossBattle;
        public string ActionDescription { get; set; }
        public IList<TurnViewModel> TurnList { get; set; }
    }
}
