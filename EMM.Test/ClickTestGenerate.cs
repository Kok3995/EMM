using System;
using System.Collections.Generic;
using System.Text;
using Data;
using EMM.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EMM.Test
{
    [TestClass]
    public class ClickTestGenerate
    {
        [TestMethod]
        public void ClickTestNoxGenerate()
        {
            var click = Helpers.GetClick(1280, 720);
            var timer = 200;

            Helpers.ApplySetting(1280, 720, 1280, 720, Emulator.Nox);
            Helpers.GetScriptGenerator(out IActionToScriptFactory actionToScriptFactory, out IEmulatorToScriptFactory emulatorToScriptFactory);

            var script = actionToScriptFactory.GetActionScriptGenerator(Emulator.Nox, BasicAction.Click).ActionToScript(click, ref timer) as StringBuilder;

            Assert.IsNotNull(script);
            Assert.AreEqual("0|1280|720|0|0|0|200|1280|720" + Environment.NewLine 
                + "1|1280|720|0|0|0|700|1280|720" + Environment.NewLine, script.ToString());
        }

        [TestMethod]
        public void ClickTestMEmuGenerate()
        {
            var click = Helpers.GetClick(1280, 720);
            var timer = 200;

            Helpers.ApplySetting(1280, 720, 1280, 720, Emulator.Memu);
            Helpers.GetScriptGenerator(out IActionToScriptFactory actionToScriptFactory, out IEmulatorToScriptFactory emulatorToScriptFactory);

            var script = actionToScriptFactory.GetActionScriptGenerator(Emulator.Memu, BasicAction.Click).ActionToScript(click, ref timer) as StringBuilder;

            Assert.IsNotNull(script);
            Assert.AreEqual("200000--VINPUT--MULTI2:1:0:0:1280:720:0" + Environment.NewLine 
                + "700000--VINPUT--MULTI2:1:0:-1:-1:-1:2" + Environment.NewLine, script.ToString());
        }

        [TestMethod]
        public void ClickTestBlueStacksGenerate()
        {
            var click = Helpers.GetClick(960, 420);
            var timer = 200;

            Helpers.ApplySetting(1280, 720, 1280, 720, Emulator.BlueStack);
            Helpers.GetScriptGenerator(out IActionToScriptFactory actionToScriptFactory, out IEmulatorToScriptFactory emulatorToScriptFactory);

            var script = actionToScriptFactory.GetActionScriptGenerator(Emulator.BlueStack, BasicAction.Click).ActionToScript(click, ref timer) as List<BlueStackEvent>;

            Assert.IsNotNull(script);
            Assert.AreEqual(200, script[0].Timestamp);
            Assert.AreEqual(700, script[1].Timestamp);
            Assert.AreEqual(BSEventType.MouseDown, script[0].EventType);
            Assert.AreEqual(BSEventType.MouseUp, script[1].EventType);
            Assert.AreEqual(75.0, script[0].X);
            Assert.AreEqual(58.33, script[0].Y);
            Assert.AreEqual(75.0, script[1].X);
            Assert.AreEqual(58.33, script[1].Y);
        }

        [TestMethod]
        public void ClickTestLDPlayerGenerate()
        {
            var click = Helpers.GetClick(960, 420);
            var timer = 200;

            Helpers.ApplySetting(1280, 720, 1280, 720, Emulator.LDPlayer);
            Helpers.GetScriptGenerator(out IActionToScriptFactory actionToScriptFactory, out IEmulatorToScriptFactory emulatorToScriptFactory);

            var script = actionToScriptFactory.GetActionScriptGenerator(Emulator.LDPlayer, BasicAction.Click).ActionToScript(click, ref timer) as List<LDPlayerOperation>;

            Assert.IsNotNull(script);
            Assert.AreEqual(200, script[0].Timing);
            Assert.AreEqual(700, script[1].Timing);
            Assert.AreEqual("PutMultiTouch", script[0].OperationId);
            Assert.AreEqual("PutMultiTouch", script[1].OperationId);
            Assert.AreEqual(1, script[0].Points[0].Id);
            Assert.AreEqual(1, script[1].Points[0].Id);
            Assert.AreEqual(14400, script[0].Points[0].X);
            Assert.AreEqual(14400, script[1].Points[0].X);
            Assert.AreEqual(6300, script[0].Points[0].Y);
            Assert.AreEqual(6300, script[1].Points[0].Y);
            Assert.AreEqual(1, script[0].Points[0].State);
            Assert.AreEqual(0, script[1].Points[0].State);
        }

        [TestMethod]
        public void ClickTestRobotmonGenerate()
        {
            var click = Helpers.GetClick(960, 420);
            var timer = 200;

            Helpers.ApplySetting(1280, 720, 1280, 720, Emulator.Robotmon);
            Helpers.GetScriptGenerator(out IActionToScriptFactory actionToScriptFactory, out IEmulatorToScriptFactory emulatorToScriptFactory);

            var script = actionToScriptFactory.GetActionScriptGenerator(Emulator.Robotmon, BasicAction.Click).ActionToScript(click, ref timer) as List<string>;

            Assert.IsNotNull(script);
            Assert.AreEqual("\"tapDown(960, 420, 0)\"", script[0]);
            Assert.AreEqual("\"sleep(500)\"", script[1]);
            Assert.AreEqual("\"tapUp(960, 420, 0)\"", script[2]);
            Assert.AreEqual("\"sleep(1000)\"", script[3]);
        }

        [TestMethod]
        public void ClickTestHiroMacroGenerate()
        {
            var click = Helpers.GetClick(960, 420);
            var timer = 200;

            Helpers.ApplySetting(1280, 720, 1280, 720, Emulator.HiroMacro);
            Helpers.GetScriptGenerator(out IActionToScriptFactory actionToScriptFactory, out IEmulatorToScriptFactory emulatorToScriptFactory);

            var script = actionToScriptFactory.GetActionScriptGenerator(Emulator.HiroMacro, BasicAction.Click).ActionToScript(click, ref timer) as StringBuilder;

            Assert.IsNotNull(script);
            Assert.AreEqual("touchDown 0 300 960" + Environment.NewLine
                + "sleep 500" + Environment.NewLine
                + "touchUp 0" + Environment.NewLine
                + "sleep 1000" + Environment.NewLine, script.ToString());
        }

        [TestMethod]
        public void ClickTestAnkuLuaGenerate()
        {
            var click = Helpers.GetClick(960, 420);
            var timer = 200;

            Helpers.ApplySetting(1280, 720, 1280, 720, Emulator.AnkuLua);
            Helpers.GetScriptGenerator(out IActionToScriptFactory actionToScriptFactory, out IEmulatorToScriptFactory emulatorToScriptFactory);

            var script = actionToScriptFactory.GetActionScriptGenerator(Emulator.AnkuLua, BasicAction.Click).ActionToScript(click, ref timer) as StringBuilder;

            Assert.IsNotNull(script);
            Assert.AreEqual("{ action = \"touchDown\", target = Location(960, 420) }," + Environment.NewLine
                + "{ action = \"wait\", target = 0.500 }," + Environment.NewLine
                + "{ action = \"touchUp\", target = Location(960, 420) }," + Environment.NewLine
                + "{ action = \"wait\", target = 1.000 }," + Environment.NewLine, script.ToString());
        }

        [TestMethod]
        public void ClickTestAutoTouchGenerate()
        {
            var click = Helpers.GetClick(960, 420);
            var timer = 200;

            Helpers.ApplySetting(1280, 720, 1280, 720, Emulator.AutoTouch);
            Helpers.GetScriptGenerator(out IActionToScriptFactory actionToScriptFactory, out IEmulatorToScriptFactory emulatorToScriptFactory);

            var script = actionToScriptFactory.GetActionScriptGenerator(Emulator.AutoTouch, BasicAction.Click).ActionToScript(click, ref timer) as StringBuilder;
            
            Assert.IsNotNull(script);
            Assert.AreEqual("touchDown(0, 960, 420);" + Environment.NewLine
                + "usleep(500000);" + Environment.NewLine
                + "touchUp(0, 960, 420);" + Environment.NewLine
                + "usleep(1000000);" + Environment.NewLine, script.ToString());
        }
    }
}
