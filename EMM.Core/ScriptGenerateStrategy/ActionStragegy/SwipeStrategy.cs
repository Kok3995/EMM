using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Data;

namespace EMM.Core
{
    /// <summary>
    /// Swipte to Nox Script
    /// </summary>
    public class SwipeStrategy : BaseScriptGenerateStragegy
    {
        public SwipeStrategy(IScriptHelper helper) : base(helper)
        {
        }

        public override object ActionToScript(IAction action, ref int timer)
        {
            if (!(action is Swipe swipe))
            {
                throw new InvalidOperationException("Action is not a swipe");
            }

            var script = helper.CreateScriptObject();

            if (swipe.PointList.Count < 2 || action.IsDisable)
                return script;

            swipe.Scale();

            for (int j = 1; j <= swipe.Repeat; j++)
            {
                swipe.Randomize(random);

                for (int i = 0; i < swipe.PointList.Count - 1; i++)
                {
                    var startPoint = swipe.PointList[i].Point;
                    var nextPoint = swipe.PointList[i + 1].Point;

                    //First Point start the swipe
                    if (i == 0)
                    {
                        //Start swipe              
                        helper.AppendAction(script, MouseAction.MouseDown, startPoint.X, startPoint.Y, ref timer);
                    }
                    else
                        helper.AppendAction(script, MouseAction.MouseDrag, startPoint.X, startPoint.Y, ref timer);

                    helper.Hold(script, swipe.PointList[i].HoldTime, ref timer);

                    //swiping
                    var inbetweenPoints = Helpers.GetInBetweenPoints(startPoint, nextPoint, GetInBetweenSwipeStep(swipe.PointList[i].SwipeSpeed));

                    foreach (var point in inbetweenPoints)
                    {
                        helper.AppendAction(script, MouseAction.MouseDrag, point.X, point.Y, ref timer);

                        helper.Hold(script, timeBetweenPoint, ref timer);
                    }
                }

                var lastPoint = swipe.PointList.LastOrDefault();

                //End swipe           
                helper.AppendAction(script, MouseAction.MouseDrag, lastPoint.Point.X, lastPoint.Point.Y, ref timer);

                helper.Hold(script, lastPoint.HoldTime, ref timer);

                //mouse up           
                helper.AppendAction(script, MouseAction.MouseUp, lastPoint.Point.X, lastPoint.Point.Y, ref timer);

                //wait for another action
                helper.WaitNext(script, swipe.WaitBetweenAction, ref timer);
            }

            return script;
        }
    }
}
