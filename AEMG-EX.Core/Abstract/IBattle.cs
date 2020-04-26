using System.Collections.Generic;

namespace AEMG_EX.Core
{
    public interface IBattle
    {
        IList<Turn> TurnList { set; get; }

        IList<Action> SelectedLeftRight { get; set; }

        int Loop { get; set; }

        int BattleExitTime { get; set; }
    }
}
