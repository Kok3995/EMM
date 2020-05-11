using System;
using Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EMM.Test
{
    [TestClass]
    public class ClickTestLDPLayer
    {
        [TestMethod]
        public void NormalScaleInStretchMode()
        {
            var click = Helpers.GetClick(1280, 720);
            var click2 = Helpers.GetClick(0, 0);

            Helpers.ApplySetting(1280, 720, 1280, 720, Emulator.LDPlayer);

            click.Scale();
            click2.Scale();

            Assert.AreEqual(click.ClickPoint.X, 1280 * 15);
            Assert.AreEqual(click.ClickPoint.Y, 720 * 15);
            Assert.AreEqual(click2.ClickPoint.X, 0);
            Assert.AreEqual(click2.ClickPoint.Y, 0);
        }

        [TestMethod]
        public void NormalScaleInStretchModePortrait()
        {
            var click = Helpers.GetClick(720, 1280);
            var click2 = Helpers.GetClick(0, 0);

            Helpers.ApplySetting(720, 1280, 720, 1280, Emulator.LDPlayer);

            click.Scale();
            click2.Scale();

            Assert.AreEqual(click.ClickPoint.X, 1280 * 15);
            Assert.AreEqual(click.ClickPoint.Y, 720 * 15);
            Assert.AreEqual(click2.ClickPoint.X, 0);
            Assert.AreEqual(click2.ClickPoint.Y, 0);
        }

        [TestMethod]
        public void UpScaleInStretchMode()
        {
            var click = Helpers.GetClick(1280, 720);
            var click2 = Helpers.GetClick(0, 0);

            Helpers.ApplySetting(1280, 720, 1920, 1080, Emulator.LDPlayer);

            click.Scale();
            click2.Scale();

            Assert.AreEqual(click.ClickPoint.X, 1920 * 10);
            Assert.AreEqual(click.ClickPoint.Y, 1080 * 10);
            Assert.AreEqual(click2.ClickPoint.X, 0);
            Assert.AreEqual(click2.ClickPoint.Y, 0);
        }

        [TestMethod]
        public void UpScaleInStretchModePortrait()
        {
            var click = Helpers.GetClick(720, 1280);
            var click2 = Helpers.GetClick(0, 0);

            Helpers.ApplySetting(720, 1280, 1080, 1920, Emulator.LDPlayer);

            click.Scale();
            click2.Scale();

            Assert.AreEqual(click.ClickPoint.X, 1920 * 10);
            Assert.AreEqual(click.ClickPoint.Y, 1080 * 10);
            Assert.AreEqual(click2.ClickPoint.X, 0);
            Assert.AreEqual(click2.ClickPoint.Y, 0);
        }

        [TestMethod]
        public void DownScaleInStretchMode()
        {
            var click = Helpers.GetClick(1280, 720);
            var click2 = Helpers.GetClick(0, 0);


            Helpers.ApplySetting(1280, 720, 960, 540, Emulator.LDPlayer);

            click.Scale();
            click2.Scale();

            Assert.AreEqual(click.ClickPoint.X, 960 * 19200 / 960);
            Assert.AreEqual(click.ClickPoint.Y, 540 * 10800 / 540);
            Assert.AreEqual(click2.ClickPoint.X, 0);
            Assert.AreEqual(click2.ClickPoint.Y, 0);
        }

        [TestMethod]
        public void DownScaleInStretchModePortrait()
        {
            var click = Helpers.GetClick(720, 1280);
            var click2 = Helpers.GetClick(0, 0);


            Helpers.ApplySetting(720, 1280, 540, 960, Emulator.LDPlayer);

            click.Scale();
            click2.Scale();

            Assert.AreEqual(click.ClickPoint.X, 960 * 19200 / 960);
            Assert.AreEqual(click.ClickPoint.Y, 540 * 10800 / 540);
            Assert.AreEqual(click2.ClickPoint.X, 0);
            Assert.AreEqual(click2.ClickPoint.Y, 0);
        }

        //-----------
        //Fit Mode
        //-----------

        [TestMethod]
        public void NormalScaleInFitMode()
        {
            var click = Helpers.GetClick(1280, 720);
            var click2 = Helpers.GetClick(0, 0);

            Helpers.ApplySetting(1280, 720, 1280, 720, Emulator.LDPlayer, scale: Data.ScaleMode.Fit);

            click.Scale();
            click2.Scale();

            Assert.AreEqual(click.ClickPoint.X, 1280 * 15);
            Assert.AreEqual(click.ClickPoint.Y, 720 * 15);
            Assert.AreEqual(click2.ClickPoint.X, 0);
            Assert.AreEqual(click2.ClickPoint.Y, 0);
        }

        [TestMethod]
        public void NormalScaleInFitModePortrait()
        {
            var click = Helpers.GetClick(720, 1280);
            var click2 = Helpers.GetClick(0, 0);

            Helpers.ApplySetting(720, 1280, 720, 1280, Emulator.LDPlayer, scale: Data.ScaleMode.Fit);

            click.Scale();
            click2.Scale();

            Assert.AreEqual(click.ClickPoint.X, 1280 * 15);
            Assert.AreEqual(click.ClickPoint.Y, 720 * 15);
            Assert.AreEqual(click2.ClickPoint.X, 0);
            Assert.AreEqual(click2.ClickPoint.Y, 0);
        }

        [TestMethod]
        public void UpScaleInFitMode()
        {
            var click = Helpers.GetClick(1280, 720);
            var click2 = Helpers.GetClick(0, 0);

            Helpers.ApplySetting(1280, 720, 3200, 1440, Emulator.LDPlayer, scale: Data.ScaleMode.Fit);

            click.Scale();
            click2.Scale();

            Assert.AreEqual(click.ClickPoint.X, 2880 * 19200 / 3200);
            Assert.AreEqual(click.ClickPoint.Y, 1440 * 10800 / 1440);
            Assert.AreEqual(click2.ClickPoint.X, GlobalData.DeltaX * 19200 / 3200);
            Assert.AreEqual(click2.ClickPoint.Y, 0);
        }

        [TestMethod]
        public void UpScaleInFitModePortrait()
        {
            var click = Helpers.GetClick(720, 1280);
            var click2 = Helpers.GetClick(0, 0);

            Helpers.ApplySetting(720, 1280, 1440, 3200, Emulator.LDPlayer, scale: Data.ScaleMode.Fit);

            click.Scale();
            click2.Scale();

            Assert.AreEqual(click.ClickPoint.X, 19200);
            Assert.AreEqual(click.ClickPoint.Y, 2880 * 10800 / 3200);
            Assert.AreEqual(click2.ClickPoint.X, 0);
            Assert.AreEqual(click2.ClickPoint.Y, GlobalData.DeltaY * 10800 / 3200);
        }

        [TestMethod]
        public void DownScaleInFitMode()
        {
            var click = Helpers.GetClick(2880, 1440);
            var click2 = Helpers.GetClick(320, 0);

            Helpers.ApplySetting(3200, 1440, 1280, 720, Emulator.LDPlayer, scale: Data.ScaleMode.Fit);

            click.Scale();
            click2.Scale();

            Assert.AreEqual(click.ClickPoint.X, 1280 * 15);
            Assert.AreEqual(click.ClickPoint.Y, 720 * 15);
            Assert.AreEqual(click2.ClickPoint.X, 0);
            Assert.AreEqual(click2.ClickPoint.Y, 0);
        }

        [TestMethod]
        public void DownScaleInFitModePortrait()
        {
            var click = Helpers.GetClick(1440, 2880);
            var click2 = Helpers.GetClick(0, 320);

            Helpers.ApplySetting(1440, 3200, 720, 1280, Emulator.LDPlayer, scale: Data.ScaleMode.Fit);

            click.Scale();
            click2.Scale();

            Assert.AreEqual(click.ClickPoint.X, 1280 * 15);
            Assert.AreEqual(click.ClickPoint.Y, 720 * 15);
            Assert.AreEqual(click2.ClickPoint.X, 0);
            Assert.AreEqual(click2.ClickPoint.Y, 0);
        }

        //-----------
        //Zoom Mode
        //-----------

        [TestMethod]
        public void NormalScaleInZoomMode()
        {
            var click = Helpers.GetClick(1280, 720);
            var click2 = Helpers.GetClick(0, 0);

            Helpers.ApplySetting(1280, 720, 1280, 720, Emulator.LDPlayer, scale: Data.ScaleMode.Zoom);

            click.Scale();
            click2.Scale();

            Assert.AreEqual(click.ClickPoint.X, 1280 * 15);
            Assert.AreEqual(click.ClickPoint.Y, 720 * 15);
            Assert.AreEqual(click2.ClickPoint.X, 0);
            Assert.AreEqual(click2.ClickPoint.Y, 0);
        }

        [TestMethod]
        public void NormalScaleInZoomModePortrait()
        {
            var click = Helpers.GetClick(720, 1280);
            var click2 = Helpers.GetClick(0, 0);

            Helpers.ApplySetting(720, 1280, 720, 1280, Emulator.LDPlayer, scale: Data.ScaleMode.Zoom);

            click.Scale();
            click2.Scale();

            Assert.AreEqual(click.ClickPoint.X, 1280 * 15);
            Assert.AreEqual(click.ClickPoint.Y, 720 * 15);
            Assert.AreEqual(click2.ClickPoint.X, 0);
            Assert.AreEqual(click2.ClickPoint.Y, 0);
        }

        [TestMethod]
        public void UpScaleInZoomMode()
        {
            var click = Helpers.GetClick(1280, 720);
            var click2 = Helpers.GetClick(0, 0);

            Helpers.ApplySetting(1280, 720, 3200, 1440, Emulator.LDPlayer, scale: Data.ScaleMode.Zoom);

            click.Scale();
            click2.Scale();

            Assert.AreEqual(click.ClickPoint.X, 3200 * 19200 / 3200);
            Assert.AreEqual(click.ClickPoint.Y, (180 + 1440) * 10800 / 1440);
            Assert.AreEqual(click2.ClickPoint.X, 0);
            Assert.AreEqual(click2.ClickPoint.Y, GlobalData.DeltaY * 10800 / 1440);
        }

        [TestMethod]
        public void UpScaleInZoomModePortrait()
        {
            var click = Helpers.GetClick(720, 1280);
            var click2 = Helpers.GetClick(0, 0);

            Helpers.ApplySetting(720, 1280, 1440, 3200, Emulator.LDPlayer, scale: Data.ScaleMode.Zoom);

            click.Scale();
            click2.Scale();

            Assert.AreEqual(click.ClickPoint.X, (1440 - GlobalData.DeltaX) * 19200 / 1440);
            Assert.AreEqual(click.ClickPoint.Y, 10800);
            Assert.AreEqual(click2.ClickPoint.X, GlobalData.DeltaX * 19200 / 1440);
            Assert.AreEqual(click2.ClickPoint.Y, 0);
        }

        [TestMethod]
        public void DownScaleInZoomMode()
        {
            var click = Helpers.GetClick(3200, 1620);
            var click2 = Helpers.GetClick(0, -180);

            Helpers.ApplySetting(3200, 1440, 1280, 720, Emulator.LDPlayer, scale: Data.ScaleMode.Zoom);

            click.Scale();
            click2.Scale();

            Assert.AreEqual(click.ClickPoint.X, 1280 * 15);
            Assert.AreEqual(click.ClickPoint.Y, 720 * 15);
            Assert.AreEqual(click2.ClickPoint.X, 0);
            Assert.AreEqual(click2.ClickPoint.Y, 0);
        }

        [TestMethod]
        public void DownScaleInZoomModePortrait()
        {
            var click = Helpers.GetClick(1620, 3200);
            var click2 = Helpers.GetClick(-180, 0);

            Helpers.ApplySetting(1440, 3200, 720, 1280, Emulator.LDPlayer, scale: Data.ScaleMode.Zoom);

            click.Scale();
            click2.Scale();

            Assert.AreEqual(click.ClickPoint.X, 1280 * 15);
            Assert.AreEqual(click.ClickPoint.Y, 720 * 15);
            Assert.AreEqual(click2.ClickPoint.X, 0);
            Assert.AreEqual(click2.ClickPoint.Y, 0);
        }
    }
}
