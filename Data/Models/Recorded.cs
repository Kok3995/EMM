using System.Text;

namespace Data
{
    public class Recorded : IAction
    {
        public BasicAction BasicAction { get => BasicAction.Recorded; set { } }

        public string ActionDescription { get; set; }

        public string RecordedString { get; set; }

        public Emulator RecordedStringEmulator { get; set; }

        public StringBuilder GenerateAction(ref int timer)
        {
            StringBuilder script = new StringBuilder();

            return script;
        }
    }
}
