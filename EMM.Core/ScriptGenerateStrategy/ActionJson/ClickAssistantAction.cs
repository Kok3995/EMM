using Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMM.Core
{
    public class ClickAssistantAction
    {
        public ClickAssistantAction(ClickAssistantType type, int delay, int duration, int cycle, double x, double y, List<ClickAssistantPath> path = null)
        {
            this.Type = type;
            this.Delay = delay;
            this.Duration = duration;
            this.Cycle = cycle;
            this.X1 = (int)x;
            this.Y1 = (int)y;
            this.Path = path;
        }

        [JsonProperty("type")]
        public ClickAssistantType Type { get; set; }

        [JsonProperty("type")]
        public int Delay { get; set; }

        [JsonProperty("duration")]
        public int Duration { get; set; }

        [JsonProperty("a_d")]
        public bool a_d { get; set; }

        [JsonProperty("cycle")]
        public int Cycle { get; set; }

        [JsonProperty("x1")]
        public int X1 { get; set; }

        [JsonProperty("y1")]
        public int Y1 { get; set; }

        public List<ClickAssistantPath> Path { get; set; }
    }

    public enum ClickAssistantType
    {
        Click = 4,
        Swipe = 7
    }

    public class ClickAssistantPath
    {
        public ClickAssistantPath(int state, double x, double y)
        {
            this.State = state;
            this.X1 = (int)x;
            this.Y1 = (int)y;
        }

        [JsonProperty("t")]
        public int State { get; set; }

        [JsonProperty("x1")]
        public int X1 { get; set; }

        [JsonProperty("y1")]
        public int Y1 { get; set; }
    }
}
