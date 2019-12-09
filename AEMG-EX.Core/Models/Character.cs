using EMM.Core;
using System.Windows.Media.Imaging;

namespace AEMG_EX.Core
{
    public class Character : BaseViewModel
    {
        public Character()
        {

        }

        public Character(Character previous)
        {
            this.CharacterIMG = previous.CharacterIMG;
            if (previous.CurrentAction == CharacterAction.SkillOne 
                || previous.CurrentAction == CharacterAction.SkillTwo
                || previous.CurrentAction == CharacterAction.SkillThree)
                this.CurrentAction = previous.CurrentAction;
            else
                this.CurrentAction = CharacterAction.DefaultSkill;

        }

        public BitmapImage CharacterIMG { get; set; }

        public Character MoveCharacter { get; set; }

        public int MoveIndex { get; set; } = -1;

        public bool IsBacklineSkillVisible => CurrentAction == CharacterAction.FrontLine || CurrentAction == CharacterAction.Move;

        public CharacterAction CurrentAction { get; set; }
    }
}
