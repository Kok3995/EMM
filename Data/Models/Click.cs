using System;
using System.Text;
using System.Windows;

namespace Data
{
    /// <summary>
    /// Define a Click
    /// </summary>
    public class Click : IAction
    {
        /// <summary>
        /// This is a click
        /// </summary>
        public BasicAction BasicAction { get => BasicAction.Click; set { } }

        /// <summary>
        /// Action's Description
        /// </summary>
        public string ActionDescription { get; set; }

        /// <summary>
        /// The click location
        /// </summary>
        public Point ClickPoint { get; set; }

        /// <summary>
        /// 0 = click, > 0 = hold
        /// </summary>
        public int HoldTime { get; set; }

        /// <summary>
        /// Number of repeated click
        /// </summary>
        public int Repeat { get; set; } = 1;

        /// <summary>
        /// Wait time between each action
        /// </summary>
        public int WaitBetweenAction { get; set; }

        /// <summary>
        /// Generate click script
        /// </summary>
        /// <param name="timer">The timer</param>
        public StringBuilder GenerateAction(ref int timer)
        {
            var x = (GlobalData.ScaleX.Equals(1.0)) ? ClickPoint.X : Math.Round(ClickPoint.X * GlobalData.ScaleX);
            var y = (GlobalData.ScaleY.Equals(1.0)) ? ClickPoint.Y : Math.Round(ClickPoint.Y * GlobalData.ScaleY);
            string xString = x.ToString();
            string yString = y.ToString();

            var random = new Random();

            StringBuilder script = new StringBuilder();

            for (int i = 1; i <= Repeat; i++)
            {
                if (GlobalData.Randomize > 0)
                {
                    xString = random.RandomCoordinate(x, GlobalData.Randomize).ToString();
                    yString = random.RandomCoordinate(y, GlobalData.Randomize).ToString();
                }

                //Mouse down
                script.AppendAction(ScriptMouseAction.MouseDown, xString, yString, ref timer);

                //hold time
                timer += HoldTime;

                //mouse up
                script.AppendAction(ScriptMouseAction.MouseUp, xString, yString, ref timer);

                //wait for next action
                timer += WaitBetweenAction;
            }               

            return script;
        }       
    }
}
