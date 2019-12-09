using System.Collections.Generic;

namespace AEMG_EX.Core
{
    public interface IBattle
    {
        IList<TurnViewModel> TurnList { set; get; }
    }
}
