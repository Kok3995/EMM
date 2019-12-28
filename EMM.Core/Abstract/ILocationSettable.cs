using System.Windows;

namespace EMM.Core
{
    /// <summary>
    /// Stupid name I know... Class that implement this interface exposed method to set its location
    /// For a click: change location, a swipe will add location
    /// </summary>
    public interface ILocationSettable
    {
        void SetLocation(Point location);
    }
}
