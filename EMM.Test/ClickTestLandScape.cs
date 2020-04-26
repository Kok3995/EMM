using System;
using Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EMM.Test
{
    [TestClass]
    public class ClickTestLandScape
    {
        [TestMethod]
        public void NormalScaleInStretchMode()
        {
            var click = Helpers.GetClick(1280, 720);
            var click2 = Helpers.GetClick(0, 0);

            Helpers.ApplySetting(1280, 720, 1280, 720);

            click.Scale();
            click2.Scale();

            Assert.AreEqual(click.ClickPoint.X, 1280);
            Assert.AreEqual(click.ClickPoint.Y, 720);
            Assert.AreEqual(click2.ClickPoint.X, 0);
            Assert.AreEqual(click2.ClickPoint.Y, 0);
        }

        [TestMethod]
        public void UpScaleInStretchMode()
        {
            var click = Helpers.GetClick(1280, 720);
            var click2 = Helpers.GetClick(0, 0);

            Helpers.ApplySetting(1280, 720, 1920, 1080);

            click.Scale();
            click2.Scale();

            Assert.AreEqual(click.ClickPoint.X, 1920);
            Assert.AreEqual(click.ClickPoint.Y, 1080);
            Assert.AreEqual(click2.ClickPoint.X, 0);
            Assert.AreEqual(click2.ClickPoint.Y, 0);
        }

        [TestMethod]
        public void DownScaleInStretchMode()
        {
            var click = Helpers.GetClick(1280, 720);
            var click2 = Helpers.GetClick(0, 0);


            Helpers.ApplySetting(1280, 720, 960, 540);

            click.Scale();
            click2.Scale();

            Assert.AreEqual(click.ClickPoint.X, 960);
            Assert.AreEqual(click.ClickPoint.Y, 540);
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

            Helpers.ApplySetting(1280, 720, 1280, 720, scale: Data.ScaleMode.Fit);

            click.Scale();
            click2.Scale();

            Assert.AreEqual(click.ClickPoint.X, 1280);
            Assert.AreEqual(click.ClickPoint.Y, 720);
            Assert.AreEqual(click2.ClickPoint.X, 0);
            Assert.AreEqual(click2.ClickPoint.Y, 0);
        }

        [TestMethod]
        public void UpScaleInFitMode()
        {
            var click = Helpers.GetClick(1280, 720);
            var click2 = Helpers.GetClick(0, 0);

            Helpers.ApplySetting(1280, 720, 3200, 1440, scale: Data.ScaleMode.Fit);

            click.Scale();
            click2.Scale();

            Assert.AreEqual(click.ClickPoint.X, 2880);
            Assert.AreEqual(click.ClickPoint.Y, 1440);
            Assert.AreEqual(click2.ClickPoint.X, GlobalData.DeltaX);
            Assert.AreEqual(click2.ClickPoint.Y, 0);
        }

        [TestMethod]
        public void DownScaleInFitMode()
        {
            var click = Helpers.GetClick(2880, 1440);
            var click2 = Helpers.GetClick(320, 0);

            Helpers.ApplySetting(3200, 1440, 1280, 720, scale: Data.ScaleMode.Fit);

            click.Scale();
            click2.Scale();

            Assert.AreEqual(click.ClickPoint.X, 1280);
            Assert.AreEqual(click.ClickPoint.Y, 720);
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

            Helpers.ApplySetting(1280, 720, 1280, 720, scale: Data.ScaleMode.Zoom);

            click.Scale();
            click2.Scale();

            Assert.AreEqual(click.ClickPoint.X, 1280);
            Assert.AreEqual(click.ClickPoint.Y, 720);
            Assert.AreEqual(click2.ClickPoint.X, 0);
            Assert.AreEqual(click2.ClickPoint.Y, 0);
        }

        [TestMethod]
        public void UpScaleInZoomMode()
        {
            var click = Helpers.GetClick(1280, 720);
            var click2 = Helpers.GetClick(0, 0);

            Helpers.ApplySetting(1280, 720, 3200, 1440, scale: Data.ScaleMode.Zoom);

            click.Scale();
            click2.Scale();

            Assert.AreEqual(click.ClickPoint.X, 3200);
            Assert.AreEqual(click.ClickPoint.Y, 1620);
            Assert.AreEqual(click2.ClickPoint.X, 0);
            Assert.AreEqual(click2.ClickPoint.Y, GlobalData.DeltaY);
        }

        [TestMethod]
        public void DownScaleInZoomMode()
        {
            var click = Helpers.GetClick(3200, 1620);
            var click2 = Helpers.GetClick(0, -180);

            Helpers.ApplySetting(3200, 1440, 1280, 720, scale: Data.ScaleMode.Zoom);

            click.Scale();
            click2.Scale();

            Assert.AreEqual(click.ClickPoint.X, 1280);
            Assert.AreEqual(click.ClickPoint.Y, 720);
            Assert.AreEqual(click2.ClickPoint.X, 0);
            Assert.AreEqual(click2.ClickPoint.Y, 0);
        }
    }
}
