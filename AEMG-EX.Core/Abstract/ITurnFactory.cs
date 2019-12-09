using Data;

namespace AEMG_EX.Core
{
    public interface ITurnFactory
    {
        TurnViewModel NewTurn(AEAction aEAction);
    }
}
