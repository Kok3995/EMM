using System.Diagnostics;

namespace EMM.Core.Tools
{
    public class TimerTool
    {
        public Stopwatch Stopwatch { get; set; } = new Stopwatch();

        public void StartTimer()
        {
            Stopwatch.Start();
        }

        public void StopTimer()
        {
            Stopwatch.Reset();
        }
    }
}
