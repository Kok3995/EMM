using EMM.Core;
using Newtonsoft.Json;
using System.Windows.Media.Imaging;

namespace AEMG_EX.Core
{
    public class Character
    {
        public Position CharacterIMG { get; set; }

        public Position? MoveCharacterPosition { get; set; }

        public int MoveIndex { get; set; }

        public CharacterAction CurrentAction { get; set; }
    }
}
