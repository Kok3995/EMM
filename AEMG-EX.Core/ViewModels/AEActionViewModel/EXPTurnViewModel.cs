using System.Windows.Media.Imaging;

namespace AEMG_EX.Core
{
    public class EXPTurnViewModel : TurnViewModel
    {
        public EXPTurnViewModel() : base()
        {

        }

        public EXPTurnViewModel(EXPTurnViewModel previous) : base(previous)
        {

        }

        public override BitmapImage ScreenIMG => (IsAF == false) ? AEMGStatic.BattleScreenEXP : AEMGStatic.AFScreen;
    }
}
