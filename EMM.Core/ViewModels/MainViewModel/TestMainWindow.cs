using Data;
using EMM.Core.Converter;
using EMM.Core.Service;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace EMM.Core.ViewModels
{
    public class TestMainWindow : BaseViewModel
    {
        public TestMainWindow()
        {
            var test = new TestClass();           

            var macro = test.ReturnTestTemplate();

            SaveCommand = new RelayCommand(p =>
            {
                var dataIO = new DataIO();

                dataIO.SaveAsToFile(macro, "test");
            });

            //LoadCommand = new RelayCommand(p =>
            //{
            //    var dataIO = new DataIO();
            //    var converter = new EMMEngine(new SimpleAutoMapper(), new DataIO(), new ScriptGenerator(new MessageBoxService()));

            //    var templatemodel = dataIO.LoadFromFile();

            //    if (templatemodel == null)
            //        return;

            //    MacroTemplate = converter.LoadMacroViewModel(templatemodel.MacroTemplate);
            //});

            SaveSettingCommand = new RelayCommand(p =>
            {
                //var setting = new Settings();               
            });

            SaveActionsCommand = new RelayCommand(p =>
            {
                var Click1 = new Click { ActionDescription = "click 1", ClickPoint = new System.Windows.Point(30,50), WaitBetweenAction = 200 };
                var Click2 = new Click { ActionDescription = "click 2" };

                var dataIO = new DataIO();
            });

            LoadActionsCommand = new RelayCommand(p =>
            {
                var savedlist = (new DataIO()).LoadCustomActions(null);
            });

            TestClickCommand = new RelayCommand(p =>
            {
                var Click = new Click { ClickPoint = new System.Windows.Point(812, 648), HoldTime = 2000, Repeat = 200000, WaitBetweenAction = 100, ActionDescription = "Click test" };

                var timer = 200;
                var script = Click.GenerateAction(ref timer);
                File.Create("test").Close();
                File.WriteAllText("test", script.ToString());
                MessageBox.Show("Done");
            });

            TestSwipeCommand = new RelayCommand(p =>
            {
                int timer = 6200;
                var point1 = new Point(1000, 420);
                var point2 = new Point(1000, 420);
                var point3 = new Point(1000, 620);
                var swipe = new Swipe
                {
                    PointList = new List<SwipePoint>
                    {
                        new SwipePoint { Point = point1, HoldTime = 2000, SwipeSpeed = 10 },
                        new SwipePoint { Point = point2, HoldTime = 1000, SwipeSpeed = 20 },
                    },

                    Repeat = 20,
                    WaitBetweenAction = 100
                };

                var script = swipe.GenerateAction(ref timer);
                File.Create("test").Close();
                File.WriteAllText("test", script.ToString());
                MessageBox.Show("Done");
            });
        }

        public string Serialize { get; set; }

        public MacroViewModel MacroTemplate { get; set; }

        public ICommand SaveCommand { get; set; }
        public ICommand LoadCommand { get; set; }
        public ICommand SaveSettingCommand { get; set; }     
        public ICommand SaveActionsCommand { get; set; }     
        public ICommand LoadActionsCommand { get; set; }

        public ICommand TestClickCommand { get; set; }
        public ICommand TestClickCommand2 { get; set; }

        public ICommand TestSwipeCommand { get; set; }
    }
}
