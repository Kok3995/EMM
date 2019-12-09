using System.Collections.Generic;
using Data;

namespace AEMG_EX.Core
{
    public class TrashMobBattle : IAEAction, IBattle
    {
        public AEAction AEAction => AEAction.TrashMobBattle;
        public string ActionDescription { get; set; }
        public IList<TurnViewModel> TurnList { get; set; }
    }
}
