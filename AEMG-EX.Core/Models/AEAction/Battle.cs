using System.Collections.Generic;
using Data;
using EMM.Core;

namespace AEMG_EX.Core
{
    public class Battle : IAEAction, IBattle
    {
        public string Name { get; set; }

        public AEAction AEAction { get; set; }

        public IList<Turn> TurnList { get; set; }

        public IList<Action> SelectedLeftRight { get; set; }

        public int Loop { get; set; }

        public int BattleExitTime { get; set; }
    }
}
