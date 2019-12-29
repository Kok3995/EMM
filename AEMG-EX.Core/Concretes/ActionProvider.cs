using Data;
using EMM.Core;
using EMM.Core.Converter;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace AEMG_EX.Core
{
    public class ActionProvider : IPredefinedActionProvider
    {
        public ActionProvider(SimpleAutoMapper autoMapper, IMessageBoxService messageBoxService)
        {
            this.autoMapper = autoMapper;
            this.messageBoxService = messageBoxService;

            defaultAction = new DefaultAction { Version = new Version(1, 0, 0, 0) };

            if (!CreateDefaultActionIfNotExistedOrOutDated())
                LoadDefaultAction();
        }

        private SimpleAutoMapper autoMapper;
        private IMessageBoxService messageBoxService;
        private string filepath = Path.Combine(AEMGStatic.AEMG_FOLDER, AEMGStatic.AEMG_DEFAULT_PREDEFINED_ACTIONS_FILENAME);
        private DefaultAction defaultAction;

        private bool CreateDefaultActionIfNotExistedOrOutDated()
        {
            if (File.Exists(filepath))
            {
                //check version to update 
                try
                {
                    var defaultActionString = File.ReadAllText(filepath);

                    var loaddefaultAction = JsonConvert.DeserializeObject<DefaultAction>(defaultActionString, new CustomJsonConverter());

                    if (loaddefaultAction.Version.CompareTo(this.defaultAction.Version) >= 0)
                        return false;
                }
                catch
                {
                    messageBoxService.ShowMessageBox("Cannot load default action. Delete DefaultAction in /AEMG then restart the program", "ERROR", MessageButton.OK, MessageImage.Error);
                    return false;
                }
            }

            if (defaultAction.Dict == null)
            {
                defaultAction.Dict = new Dictionary<Action, List<IAction>>();
            }

            Directory.CreateDirectory(AEMGStatic.AEMG_FOLDER);

            var possibleAction = Enum.GetValues(typeof(Action)).Cast<Action>().ToList();
            foreach (var a in possibleAction)
            {
                defaultAction.Dict.Add(a, new List<IAction>());
            }

            //In Battle
            defaultAction.Dict[Action.ClickFirstCharacter].Add(new Click { ActionDescription = "ClickFirstCharacter", ClickPoint = new Point(100,660), WaitBetweenAction = 500 });
            defaultAction.Dict[Action.ClickSecondCharacter].Add(new Click { ActionDescription = "ClickSecondCharacter", ClickPoint = new Point(260,660), WaitBetweenAction = 500 });
            defaultAction.Dict[Action.ClickThirdCharacter].Add(new Click { ActionDescription = "ClickThirdCharacter", ClickPoint = new Point(430,660), WaitBetweenAction = 500 });
            defaultAction.Dict[Action.ClickFourthCharacter].Add(new Click { ActionDescription = "ClickFourthCharacter", ClickPoint = new Point(600,660), WaitBetweenAction = 500 });
            defaultAction.Dict[Action.ClickFifthCharacter].Add(new Click { ActionDescription = "ClickFifthCharacter", ClickPoint = new Point(800,660), WaitBetweenAction = 500 });
            defaultAction.Dict[Action.ClickSixthCharacter].Add(new Click { ActionDescription = "ClickSixthCharacter", ClickPoint = new Point(960,660), WaitBetweenAction = 500 });
            defaultAction.Dict[Action.ClickDefaultSkill].Add(new Click { ActionDescription = "ClickDefaultSkill", ClickPoint = new Point(150,550), WaitBetweenAction = 500 });
            defaultAction.Dict[Action.ClickFirstSkill].Add(new Click { ActionDescription = "ClickFirstSkill", ClickPoint = new Point(380,550), WaitBetweenAction = 500 });
            defaultAction.Dict[Action.ClickSecondSKill].Add(new Click { ActionDescription = "ClickSecondSKill", ClickPoint = new Point(630,550), WaitBetweenAction = 500 });
            defaultAction.Dict[Action.ClickThirdSkill].Add(new Click { ActionDescription = "ClickThirdSkill", ClickPoint = new Point(890, 550), WaitBetweenAction = 500 });
            defaultAction.Dict[Action.ClickSwitch_FrontLine].Add(new Click { ActionDescription = "ClickSwitch_FrontLine", ClickPoint = new Point(1130, 550), WaitBetweenAction = 500 });
            defaultAction.Dict[Action.ClickSub].Add(new Click { ActionDescription = "ClickSub", ClickPoint = new Point(1130,660), WaitBetweenAction = 500 });
            defaultAction.Dict[Action.ClickAF].Add(new Click { ActionDescription = "ClickAF", ClickPoint = new Point(1200,80), WaitBetweenAction = 500 });
            defaultAction.Dict[Action.ClickAttack].Add(new Click { ActionDescription = "ClickAttack", ClickPoint = new Point(1170,620), WaitBetweenAction = 500 });
            defaultAction.Dict[Action.Wait].Add(new Wait { ActionDescription = "Wait" });

            //Food in AD
            defaultAction.Dict[Action.FoodAD].Add(new Click { ActionDescription = "Click Menu", ClickPoint = new Point(80,660), WaitBetweenAction = 1000 });
            defaultAction.Dict[Action.FoodAD].Add(new Click { ActionDescription = "Click Food", ClickPoint = new Point(580, 610), WaitBetweenAction = 1000 });
            defaultAction.Dict[Action.FoodAD].Add(new Click { ActionDescription = "Click Use", ClickPoint = new Point(780, 420), WaitBetweenAction = 1000, Repeat = 4 });

            //Re eat food after AD
            defaultAction.Dict[Action.ReFoodAD].Add(new Swipe
            {
                ActionDescription = "Move Right A Little",
                PointList = new List<SwipePoint> {
                new SwipePoint { Point = new Point(900,420), SwipeSpeed = 10 } , new SwipePoint { Point = new Point(1000,420), HoldTime = 100, SwipeSpeed = 10 } },
                WaitBetweenAction = 200
            });
            defaultAction.Dict[Action.ReFoodAD].AddRange(this.GenerateSeriesOfClickBetweenPoint(new Point(1000, 70), new Point(450, 70), 50, "Find Tree"));
            defaultAction.Dict[Action.ReFoodAD].Add(new Click { ActionDescription = "Click Yes", ClickPoint = new Point(780, 400), WaitBetweenAction = 1000, Repeat = 10 });
            defaultAction.Dict[Action.ReFoodAD].AddRange(this.GenerateSeriesOfClickBetweenPoint(new Point(650, 150), new Point(50, 150), 50, "Find Gate"));

            //Run left (for exp encounter)
            defaultAction.Dict[Action.RunLeft].Add(new Swipe
            {
                ActionDescription = "Move Left",
                PointList = new List<SwipePoint> {
                new SwipePoint { Point = new Point(900,420), SwipeSpeed = 10 } , new SwipePoint { Point = new Point(700,420), HoldTime = 2000, SwipeSpeed = 10 } },
                WaitBetweenAction = 200
            });
            //Run right (for exp encounter)
            defaultAction.Dict[Action.RunRight].Add(new Swipe
            {
                ActionDescription = "Move Right",
                PointList = new List<SwipePoint> {
                new SwipePoint { Point = new Point(900,420), SwipeSpeed = 10 } , new SwipePoint { Point = new Point(1100,420), HoldTime = 2000, SwipeSpeed = 10 } },
                WaitBetweenAction = 200
            });

            File.WriteAllText(filepath, JsonConvert.SerializeObject(defaultAction));

            return true;
        }

        private void LoadDefaultAction()
        {
            var dictString = File.ReadAllText(filepath);

            defaultAction = JsonConvert.DeserializeObject<DefaultAction>(dictString, new CustomJsonConverter());
        }

        public IList<IAction> GetCharacterAction(Action action)
        {
            if (!defaultAction.Dict.ContainsKey(action))
                throw new NotImplementedException();

            return defaultAction.Dict[action];
        }

        public IList<IAction> GetAFSpamClick(CharacterAction skill, int aftime)
        {
            var list = new List<IAction>();
            var action = this.CharacterActionToAction(skill);

            if (!defaultAction.Dict.ContainsKey(action))
                throw new NotImplementedException();

            var spamSkill = defaultAction.Dict[action][0] as Click;

            //decrease spam skill time
            var newSpamSkill = this.autoMapper.SimpleAutoMap<Click, Click>(spamSkill);

            newSpamSkill.WaitBetweenAction /= 2;

            var spamNum = (int)(aftime / newSpamSkill.WaitBetweenAction);
            for (int i = 0; i < spamNum; i++)
            {
                list.Add(newSpamSkill);
            }

            return list;
        }


        public IList<IAction> GetSkillClick(CharacterAction skill)
        {
            var action = this.CharacterActionToAction(skill);

            if (!defaultAction.Dict.ContainsKey(action))
                throw new NotImplementedException();

            return defaultAction.Dict[action];
        }

        public IAction GetWait(int waittime)
        {
            var wait = this.autoMapper.SimpleAutoMap<Wait,Wait>(defaultAction.Dict[Action.Wait][0] as Wait);
            wait.WaitTime = waittime;
            return wait;
        }
        
        #region Helpers

        private Action CharacterActionToAction(CharacterAction action)
        {
            switch (action)
            {
                case CharacterAction.AF0:
                case CharacterAction.DefaultSkill:
                    return Action.ClickDefaultSkill;

                case CharacterAction.AF1:
                case CharacterAction.SkillOne:
                    return Action.ClickFirstSkill;

                case CharacterAction.AF2:
                case CharacterAction.SkillTwo:
                    return Action.ClickSecondSKill;

                case CharacterAction.AF3:
                case CharacterAction.SkillThree:
                    return Action.ClickThirdSkill;

                case CharacterAction.Move:
                    return Action.ClickSwitch_FrontLine;
                case CharacterAction.Reserve:
                    return Action.ClickSub;
            }

            return Action.ClickDefaultSkill;
        } 

        private IList<IAction> GenerateSeriesOfClickBetweenPoint(Point from, Point to, int step, string actionName)
        {
            var list = new List<IAction>();
            var points = this.GetInBetweenPoints(from, to, (sbyte)step);

            foreach (var point in points)
            {
                list.Add(new Click { ActionDescription = actionName, ClickPoint = point, WaitBetweenAction = 50 });
            }

            return list;
        }

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
            double slope = dy / dx;

            int numberOfPoint = (int)Math.Sqrt(Math.Pow(dx, 2) + Math.Pow(dy, 2)) / step;

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

        #endregion

    }

    public class DefaultAction
    {
        /// <summary>
        /// Use this to update the file if nesscessary
        /// </summary>
        public Version Version { get; set; }

        /// <summary>
        /// List of default actions
        /// </summary>
        public Dictionary<Action, List<IAction>> Dict;
    }
}
