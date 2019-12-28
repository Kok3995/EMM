using System;

namespace EMM.Core
{
    public interface IResolutionChange
    {
        /// <summary>
        /// Change the resolution of every point
        /// </summary>
        /// <param name="scaleX">Multiplier for the x coordinate</param>
        /// <param name="scaleY">Multiplier for the y coordinate</param>
        /// <returns>return a new IActionViewModel with the changed point</returns>
        public IActionViewModel ChangeResolution(double scaleX, double scaleY, MidpointRounding roundMode = MidpointRounding.ToEven);
    }
}
