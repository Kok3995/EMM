using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EMM.Core
{
    /// <summary>
    /// A ldplayer operation
    /// </summary>
    public class LDPlayerOperation
    {
        public LDPlayerOperation(int timing, int x, int y, LDPLayerState state)
        {
            this.Timing = timing;
            this.Points = new [] { new LDPoint { X = x, Y = y, State = (int)state } };
        }

        [JsonProperty("timing")]
        public int Timing { get; set; }

        [JsonProperty("operationId")]
        public string OperationId { get; set; } = "PutMultiTouch";

        [JsonProperty("points")]
        public LDPoint[] Points { get; set; }
    }

    /// <summary>
    /// ldplayer point
    /// </summary>
    public class LDPoint
    {
        [JsonProperty("id")]
        public int Id { get; set; } = 1;

        [JsonProperty("x")]
        public int X { get; set; }

        [JsonProperty("y")]
        public int Y { get; set; }

        [JsonProperty("state")]
        public int State { get; set; }
    }

    public enum LDPLayerState
    {
        MouseDown = 1,
        MouseUp = 0
    }
}
