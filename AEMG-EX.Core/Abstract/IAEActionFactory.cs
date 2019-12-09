using Data;

namespace AEMG_EX.Core
{
    public interface IAEActionFactory
    {
        IAEActionViewModel NewAEActionViewModel(AEAction aEAction);
    }
}
