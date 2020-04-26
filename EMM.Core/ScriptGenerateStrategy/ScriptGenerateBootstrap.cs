using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMM.Core
{
    /// <summary>
    /// Initialize the factory for script generator
    /// </summary>
    public class ScriptGenerateBootstrap
    {
        public void SetUp(out IActionToScriptFactory actionToScriptFactory, out IEmulatorToScriptFactory emulatorToScriptFactory)
        {
            actionToScriptFactory = new ActionToScriptFactory();

            var noxHelper = new NoxScriptHelper();
            var memuHelper = new MEmuScriptHelper();
            var hiroHelper = new HiroMacroScriptHelper();
            var ankuHelper = new AnkuLuaScriptHelper();
            var blueHelper = new BlueStackScriptHelper();
            var ldHelper = new LDPlayerScriptHelper();
            var autoHelper = new AutoTouchScriptHelper();
            var robotHelper = new RobotmonScriptHelper();

            var noxActionDict = new Dictionary<BasicAction, IActionScriptGenerator>
            {
                { BasicAction.Click, new ClickStrategy(noxHelper) },
                { BasicAction.Swipe, new SwipeStrategy(noxHelper) },
                { BasicAction.Wait, new WaitStrategy(noxHelper) },
                { BasicAction.AE,  new AEStrategy(noxHelper)},
                { BasicAction.ActionGroup, new ActionGroupStrategy(actionToScriptFactory, noxHelper) },
            };

            var memuActionDict = new Dictionary<BasicAction, IActionScriptGenerator>
            {
                { BasicAction.Click, new ClickStrategy(memuHelper) },
                { BasicAction.Swipe, new SwipeStrategy(memuHelper) },
                { BasicAction.Wait, new WaitStrategy(memuHelper) },
                { BasicAction.AE,  new AEStrategy(memuHelper)},
                { BasicAction.ActionGroup, new ActionGroupStrategy(actionToScriptFactory, memuHelper) },
            };

            var blueActionDict = new Dictionary<BasicAction, IActionScriptGenerator>
            {
                { BasicAction.Click, new ClickStrategy(blueHelper) },
                { BasicAction.Swipe, new SwipeStrategy(blueHelper) },
                { BasicAction.Wait, new WaitStrategy(blueHelper) },
                { BasicAction.AE,  new AEStrategy(blueHelper)},
                { BasicAction.ActionGroup, new ActionGroupStrategy(actionToScriptFactory, blueHelper) },
            };

            var ldActionDict = new Dictionary<BasicAction, IActionScriptGenerator>
            {
                { BasicAction.Click, new ClickStrategy(ldHelper) },
                { BasicAction.Swipe, new SwipeStrategy(ldHelper) },
                { BasicAction.Wait, new WaitStrategy(ldHelper) },
                { BasicAction.AE,  new AEStrategy(ldHelper)},
                { BasicAction.ActionGroup, new ActionGroupStrategy(actionToScriptFactory, ldHelper) },
            };

            var hiroActionDict = new Dictionary<BasicAction, IActionScriptGenerator>
            {
                { BasicAction.Click, new ClickStrategy(hiroHelper) },
                { BasicAction.Swipe, new SwipeStrategy(hiroHelper) },
                { BasicAction.Wait, new WaitStrategy(hiroHelper) },
                { BasicAction.AE,  new AEStrategy(hiroHelper)},
                { BasicAction.ActionGroup, new ActionGroupStrategy(actionToScriptFactory, hiroHelper) },
            };

            var ankuActionDict = new Dictionary<BasicAction, IActionScriptGenerator>
            {
                { BasicAction.Click, new ClickStrategy(ankuHelper) },
                { BasicAction.Swipe, new SwipeStrategy(ankuHelper) },
                { BasicAction.Wait, new WaitStrategy(ankuHelper) },
                { BasicAction.AE,  new AEStrategy(ankuHelper)},
                { BasicAction.ActionGroup, new ActionGroupStrategy(actionToScriptFactory, ankuHelper) },
            };

            var autoTouchActionDict = new Dictionary<BasicAction, IActionScriptGenerator>
            {
                { BasicAction.Click, new ClickStrategy(autoHelper) },
                { BasicAction.Swipe, new SwipeStrategy(autoHelper) },
                { BasicAction.Wait, new WaitStrategy(autoHelper) },
                { BasicAction.AE,  new AEStrategy(autoHelper)},
                { BasicAction.ActionGroup, new ActionGroupStrategy(actionToScriptFactory, autoHelper) },
            };

            var robotActionDict = new Dictionary<BasicAction, IActionScriptGenerator>
            {
                { BasicAction.Click, new ClickStrategy(robotHelper) },
                { BasicAction.Swipe, new SwipeStrategy(robotHelper) },
                { BasicAction.Wait, new WaitStrategy(robotHelper) },
                { BasicAction.AE,  new AEStrategy(robotHelper)},
                { BasicAction.ActionGroup, new ActionGroupStrategy(actionToScriptFactory, robotHelper) },
            };

            var actiontoScriptDict = new Dictionary<Emulator, Dictionary<BasicAction, IActionScriptGenerator>>
            {
                {Emulator.Nox, noxActionDict },
                {Emulator.Memu, memuActionDict },
                {Emulator.BlueStack, blueActionDict },
                {Emulator.LDPlayer, ldActionDict },
                {Emulator.HiroMacro, hiroActionDict },
                {Emulator.AnkuLua, ankuActionDict },
                {Emulator.AutoTouch, autoTouchActionDict },
                {Emulator.Robotmon, robotActionDict },
            };

            actionToScriptFactory.SetDependency(actiontoScriptDict);

            var noxMacroGenerator = new NoxMacroGenerator(actionToScriptFactory);
            var MemuMacroGenerator = new MEmuMacroGenerator(actionToScriptFactory);
            var blueMacroGenerator = new BlueStackMacroGenerator(actionToScriptFactory);
            var ldMacroGenerator = new LDPlayerMacroGenerator(actionToScriptFactory);
            var hiroMacroGenerator = new HiroMacroGenerator(actionToScriptFactory);
            var ankuMacroGenerator = new AnkuMacroGenerator(actionToScriptFactory);
            var autoMacroGenerator = new AutoTouchMacroGenerator(actionToScriptFactory);
            var robotMacroGenerator = new RobotmonMacroGenerator(actionToScriptFactory);

            var emulatorToScriptDict = new Dictionary<Emulator, IMacroScriptGenerator>
            {
                { Emulator.Nox, noxMacroGenerator },
                { Emulator.Memu, MemuMacroGenerator},
                { Emulator.BlueStack, blueMacroGenerator},
                { Emulator.LDPlayer, ldMacroGenerator},
                { Emulator.HiroMacro, hiroMacroGenerator},
                { Emulator.AnkuLua, ankuMacroGenerator},
                { Emulator.AutoTouch, autoMacroGenerator},
                { Emulator.Robotmon, robotMacroGenerator},
            };

            emulatorToScriptFactory = new EmulatorToScriptFactory(emulatorToScriptDict);
        }
    }
}
