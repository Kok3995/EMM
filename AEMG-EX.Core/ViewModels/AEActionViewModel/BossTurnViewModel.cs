using System.Windows.Media.Imaging;

namespace AEMG_EX.Core
{
    public class BossTurnViewModel : TurnViewModel
    {
        public BossTurnViewModel() : base()
        {

        }

        public BossTurnViewModel(BossTurnViewModel previous) : base(previous)
        {

        }

        public override BitmapImage ScreenIMG => (IsAF == false) ? AEMGStatic.BattleScreenBoss : AEMGStatic.AFScreen;

        public override int WaitNextTurnTime { get; set; } = 15000;
    }
}
