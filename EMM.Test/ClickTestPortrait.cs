using System;
using Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EMM.Test
{
    [TestClass]
    public class ClickTestPortrait
    {
        [TestMethod]
        public void NormalScaleInStretchMode()
        {
            var click = Helpers.GetClick(720, 1280);
            var click2 = Helpers.GetClick(0, 0);

            Helpers.ApplySetting(720, 1280, 720, 1280);

            click.Scale();
            click2.Scale();

            Assert.AreEqual(click.ClickPoint.X, 720);
            Assert.AreEqual(click.ClickPoint.Y, 1280);
            Assert.AreEqual(click2.ClickPoint.X, 0);
            Assert.AreEqual(click2.ClickPoint.Y, 0);
        }

        [TestMethod]
        public void UpScaleInStretchMode()
        {
            var click = Helpers.GetClick(720, 1280);
            var click2 = Helpers.GetClick(0, 0);

            Helpers.ApplySetting(720, 1280, 1440, 3200);

            click.Scale();
            click2.Scale();

            Assert.AreEqual(click.ClickPoint.X, 1440);
            Assert.AreEqual(click.ClickPoint.Y, 3200);
            Assert.AreEqual(click2.ClickPoint.X, 0);
            Assert.AreEqual(click2.ClickPoint.Y, 0);
        }

        [TestMethod]
        public void DownScaleInStretchMode()
        {
            var click = Helpers.GetClick(1440, 3200);
            var click2 = Helpers.GetClick(0, 0);


            Helpers.ApplySetting(1440, 3200, 720, 1280);

            click.Scale();
            click2.Scale();

            Assert.AreEqual(click.ClickPoint.X, 720);
            Assert.AreEqual(click.ClickPoint.Y, 1280);
            Assert.AreEqual(click2.ClickPoint.X, 0);
            Assert.AreEqual(click2.ClickPoint.Y, 0);
        }

        //-----------
        //Fit Mode
        //-----------

        [TestMethod]
        public void NormalScaleInFitMode()
        {
            var click = Helpers.GetClick(720, 1280);
            var click2 = Helpers.GetClick(0, 0);

            Helpers.ApplySetting(720, 1280, 720, 1280, scale: Data.ScaleMode.Fit);

            click.Scale();
            click2.Scale();

            Assert.AreEqual(click.ClickPoint.X, 720);
            Assert.AreEqual(click.ClickPoint.Y, 1280);
            Assert.AreEqual(click2.ClickPoint.X, 0);
            Assert.AreEqual(click2.ClickPoint.Y, 0);
        }

        [TestMethod]
        public void UpScaleInFitMode()
        {
            var click = Helpers.GetClick(720, 1280);
            var click2 = Helpers.GetClick(0, 0);

            Helpers.ApplySetting(720, 1280, 1440, 3200, scale: Data.ScaleMode.Fit);

            click.Scale();
            click2.Scale();

            Assert.AreEqual(click.ClickPoint.X, 1440);
            Assert.AreEqual(click.ClickPoint.Y, 2880);
            Assert.AreEqual(click2.ClickPoint.X, 0);
            Assert.AreEqual(click2.ClickPoint.Y, GlobalData.DeltaY);
        }

        [TestMethod]
        public void DownScaleInFitMode()
        {
            var click = Helpers.GetClick(1440, 2880);
            var click2 = Helpers.GetClick(0, 320);

            Helpers.ApplySetting(1440, 3200, 720, 1280, scale: Data.ScaleMode.Fit);

            click.Scale();
            click2.Scale();

            Assert.AreEqual(click.ClickPoint.X, 720);
            Assert.AreEqual(click.ClickPoint.Y, 1280);
            Assert.AreEqual(click2.ClickPoint.X, 0);
            Assert.AreEqual(click2.ClickPoint.Y, 0);
        }

        //-----------
        //Zoom Mode
        //-----------

        [TestMethod]
        public void NormalScaleInZoomMode()
        {
            var click = Helpers.GetClick(720, 1280);
            var click2 = Helpers.GetClick(0, 0);

            Helpers.ApplySetting(720, 1280, 720, 1280, scale: Data.ScaleMode.Zoom);

            click.Scale();
            click2.Scale();

            Assert.AreEqual(click.ClickPoint.X, 720);
            Assert.AreEqual(click.ClickPoint.Y, 1280);
            Assert.AreEqual(click2.ClickPoint.X, 0);
            Assert.AreEqual(click2.ClickPoint.Y, 0);
        }

        [TestMethod]
        public void UpScaleInZoomMode()
        {
            var click = Helpers.GetClick(720, 1280);
            var click2 = Helpers.GetClick(0, 0);

            Helpers.ApplySetting(720, 1280, 1440, 3200, scale: Data.ScaleMode.Zoom);

            click.Scale();
            click2.Scale();

            Assert.AreEqual(click.ClickPoint.X, 1620);
            Assert.AreEqual(click.ClickPoint.Y, 3200);
            Assert.AreEqual(click2.ClickPoint.X, GlobalData.DeltaX);
            Assert.AreEqual(click2.ClickPoint.Y, 0);
        }

        [TestMethod]
        public void DownScaleInZoomMode()
        {
            var click = Helpers.GetClick(1620, 3200);
            var click2 = Helpers.GetClick(-180, 0);

            Helpers.ApplySetting(1440, 3200, 720, 1280, scale: Data.ScaleMode.Zoom);

            click.Scale();
            click2.Scale();

            Assert.AreEqual(click.ClickPoint.X, 720);
            Assert.AreEqual(click.ClickPoint.Y, 1280);
            Assert.AreEqual(click2.ClickPoint.X, 0);
            Assert.AreEqual(click2.ClickPoint.Y, 0);
        }
    }
}
