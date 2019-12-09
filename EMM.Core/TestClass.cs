using Data;
using System.Collections.Generic;
using System.Windows;

namespace EMM.Core
{
    public class TestClass
    {       
        public MacroTemplate ReturnTestTemplate()
        {
            var Click1 = new Click { ActionDescription = "Click 1", ClickPoint = new Point(50, 50), HoldTime = 10, Repeat = 2, WaitBetweenAction = 20};
            var Click2 = new Click { ActionDescription = "Click 2", ClickPoint = new Point(100, 100), HoldTime = 20, Repeat = 4, WaitBetweenAction = 40 };

            var Swipe1 = new Swipe { ActionDescription = "Swipe 1", Repeat = 1, PointList = new List<SwipePoint> { new SwipePoint { Point = new Point(308, 408), HoldTime = 400, SwipeSpeed = 20 }, new SwipePoint { Point = new Point(1112, 441), HoldTime = 700 } } };
            var Swipe2 = new Swipe { ActionDescription = "Swipe 2", Repeat = 1, PointList = new List<SwipePoint> { new SwipePoint { Point = new Point(150, 500), HoldTime = 700, SwipeSpeed=100 }, new SwipePoint { Point = new Point(1000, 50), HoldTime = 800 } } };

            var Wait1 = new Wait { ActionDescription = "Wait1", WaitTime = 500 };
            var Wait2 = new Wait { ActionDescription = "Wait2", WaitTime = 1500 };

            var AE1 = new AE { ActionDescription = "AE 1", AnotherEdenAction = AEAction.EXPBattle, Repeat = 1 };
            var AE2 = new AE { ActionDescription = "AE 2", AnotherEdenAction = AEAction.FoodAD, Repeat = 2 };

            var actionGroup1 = new ActionGroup { ActionDescription = "action group 1", ActionList = new List<IAction> { Click1, Swipe1, Wait1, AE1 }, Repeat = 1 };
            var actionGroup2 = new ActionGroup { ActionDescription = "action group 2", ActionList = new List<IAction> { Click2, Swipe2, Wait2, AE2 }, Repeat = 1 };
            var actionGroup3 = new ActionGroup { ActionDescription = "action group 3", ActionList = new List<IAction> { Click1, Swipe2, Wait1, AE1 }, Repeat = 1 };
            var actionGroup4 = new ActionGroup { ActionDescription = "action group 4", ActionList = new List<IAction> { Click2, Swipe2, Wait2, AE2 }, Repeat = 1 };
            var actionGroup5 = new ActionGroup { ActionDescription = "action group 5", ActionList = new List<IAction> { Click1, Swipe2, Wait2, AE1 }, Repeat = 1 };

            return new MacroTemplate { OriginalX=1280, OriginalY=720, MacroVersion =0, MacroName = "Test macro", ActionGroupList = new List<IAction> { actionGroup1, actionGroup2, actionGroup3, actionGroup4, actionGroup5 } };
        }
    }
}
