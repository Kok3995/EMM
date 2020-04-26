using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EMM.Core
{
    /// <summary>
    /// A bluestack event
    /// </summary>
    public class BlueStackEvent
    {
        public BlueStackEvent(int time, double x, double y, BSEventType eventType)
        {
            this.Timestamp = time;
            this.X = x;
            this.Y = y;
            this.EventType = eventType;
        }

        public int Timestamp { get; set; }

        public double X { get; set; }

        public double Y { get; set; }

        public int Delta { get; set; }

        public BSEventType EventType { get; set; }
    }

    /// <summary>
    /// BlueStacks EventType
    /// </summary>
    public enum BSEventType
    {
        MouseDown,
        MouseUp,
        MouseMove
    }
}
