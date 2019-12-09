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
        /// Generate swipe script
        /// </summary>
        /// <param name="timer">the timer</param>
        public StringBuilder GenerateAction(ref int timer)
        {
            if (PointList.Count < 2)
                return new StringBuilder(string.Empty);

            var random = new Random();
            int timeBetweenPoint = 12;

            StringBuilder script = new StringBuilder();

            for (int j = 1; j <= Repeat; j++)
            {
                for (int i = 0; i < PointList.Count - 1; i++)
                {                  
                    var startPoint = this.ScaleAndRandomize(PointList[i], random, out string xString, out string yString);
                    var nextPoint = this.ScaleAndRandomize(PointList[i + 1], random, out _, out _);

                    //First Point start the swipe
                    if (i == 0)
                    {
                        //Start swipe              
                        script.AppendAction(ScriptMouseAction.MouseDown, xString, yString, ref timer);
                    }
                    else
                        script.AppendAction(ScriptMouseAction.MouseDrag, xString, yString, ref timer);

                    timer += PointList[i].HoldTime;

                    //swiping
                    var inbetweenPoints = GetInBetweenPoints(startPoint, nextPoint, PointList[i].SwipeSpeed);                   

                    foreach (var point in inbetweenPoints)
                    {
                        script.AppendAction(ScriptMouseAction.MouseDrag, point.X.ToString(), point.Y.ToString(), ref timer);

                        timer += timeBetweenPoint;
                    }
                }

                var lastPoint = PointList.LastOrDefault<SwipePoint>();
                this.ScaleAndRandomize(lastPoint, random, out string lastXString, out string lastYString);

                //End swipe           
                script.AppendAction(ScriptMouseAction.MouseDrag, lastXString, lastYString, ref timer);

                timer += lastPoint.HoldTime;

                //mouse up           
                script.AppendAction(ScriptMouseAction.MouseUp, lastXString, lastYString, ref timer);

                //wait for another action
                timer += this.WaitBetweenAction;
            }

            return script;
        }

        #region Helpers

        /// <summary>
        /// Return a list of in-between points to make up a swipe motion
        /// </summary>
        /// <param name="startPoint">Starting Point</param>
        /// <param name="endPoint">End Point</param>
        /// <returns></returns>
        private IEnumerable<Point> GetInBetweenPoints(Point startPoint, Point endPoint, sbyte step)
        {
            double dx = endPoint.X - startPoint.X;
            double dy = endPoint.Y - startPoint.Y;
            double slope = dy/dx;

            int numberOfPoint = (int)Math.Sqrt(Math.Pow(dx, 2) + Math.Pow(dy, 2))/step;

            if (numberOfPoint == 0)
                yield break;

            double xstep = dx / numberOfPoint;
            double ystep = dy / numberOfPoint;
            --numberOfPoint;
            

            for (int i = 1; i <= numberOfPoint; i++)
            {
                double x;
                double y;

                if (Math.Abs(dx) >= Math.Abs(dy))
                {
                    x = xstep * i;
                    y = (dy == 0) ? 0 : x * slope;
                }
                else
                {
                    y = ystep * i;
                    x = (dx == 0) ? 0 : y / slope;
                }

                yield return new Point(Math.Round(startPoint.X + x), Math.Round(startPoint.Y + y));
            }          
        }

        /// <summary>
        /// Scale and randomized the point
        /// </summary>
        /// <param name="point">The <see cref="SwipePoint"/> to randomize</param>
        /// <param name="random">The <see cref="Random"/> instance</param>
        /// <param name="xString">output x as string</param>
        /// <param name="yString">output y as string</param>
        /// <returns>The point that has been scaled and randomized</returns>
        private Point ScaleAndRandomize(SwipePoint point, Random random, out string xString, out string yString)
        {
            //scale the point
            var x = (GlobalData.ScaleX.Equals(1.0)) ? point.Point.X : Math.Round(point.Point.X * GlobalData.ScaleX);
            var y = (GlobalData.ScaleX.Equals(1.0)) ? point.Point.Y : Math.Round(point.Point.Y * GlobalData.ScaleY);

            //randomized it
            x = (GlobalData.Randomize > 0) ? random.RandomCoordinate(x, GlobalData.Randomize) : x;
            y = (GlobalData.Randomize > 0) ? random.RandomCoordinate(y, GlobalData.Randomize) : y;

            //convert to string
            xString = x.ToString();
            yString = y.ToString();

            return new Point(x, y);
        }

        #endregion

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
