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
        /// True to disable this action
        /// </summary>
        public bool IsDisable { get; set; }

        public void Scale()
        {
            this.ClickPoint = ClickPoint.ScalePointBaseOnMode();
        }

        public Point Randomize(Random random)
        {
            if (GlobalData.Randomize > 0)
            {
                return new Point(random.RandomCoordinate(this.ClickPoint.X, GlobalData.Randomize), random.RandomCoordinate(this.ClickPoint.Y, GlobalData.Randomize));
            }

            return this.ClickPoint;
        }
    }
}
