using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Data
{
    /// <summary>
    /// Define a Swipe
    /// </summary>
    public class Swipe : IAction
    {
        /// <summary>
        /// This is a Swipe
        /// </summary>
        public BasicAction BasicAction { get => BasicAction.Swipe; set { } }

        /// <summary>
        /// Action's Description
        /// </summary>
        public string ActionDescription { get; set; }

        /// <summary>
        /// List of points for the swipe
        /// </summary>
        public List<SwipePoint> PointList { get; set; }

        /// <summary>
        /// Number of repeat swipe
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

        /// <summary>
        /// Temporary hold the original pointList
        /// </summary>
        private List<SwipePoint> pointListTemp;

        public void Scale()
        {
            PointList = new List<SwipePoint>(PointList.Select(p => new SwipePoint(p)));

            foreach (var swipePoint in PointList)
            {
                swipePoint.Point = swipePoint.Point.ScalePointBaseOnMode();
            }
        }

        public void Randomize(Random random)
        {
            if (GlobalData.Randomize <= 0)
                return;

            if (pointListTemp == null)
                pointListTemp = PointList;

            PointList = pointListTemp.Select(p =>
            {
                var x = random.RandomCoordinate(p.Point.X, GlobalData.Randomize);
                var y = random.RandomCoordinate(p.Point.Y, GlobalData.Randomize);
                return new SwipePoint { Point = new Point(x, y), HoldTime = p.HoldTime, SwipeSpeed = p.SwipeSpeed };
            }).ToList();
        }
    }

    public class SwipePoint
    {
        public SwipePoint()
        {

        }
        public SwipePoint(SwipePoint swipePoint)
        {
            this.Point = swipePoint.Point;
            this.HoldTime = swipePoint.HoldTime;
            this.SwipeSpeed = swipePoint.SwipeSpeed;
        }
        /// <summary>
        /// Key point of the swipe
        /// </summary>
        public Point Point { get; set; }

        /// <summary>
        /// Hold at this point
        /// </summary>
        public int HoldTime { get; set; } = 12;

        /// <summary>
        /// Speed of the swipe: 5 10 20 30 40 50
        /// </summary>
        public sbyte SwipeSpeed { get; set; } = 20;
    }
}
