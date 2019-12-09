using System;
using System.Windows.Input;
using System.Windows.Threading;

namespace EMM.Core.Tools
{
    public class TimerToolViewModel : BaseViewModel
    {
        public TimerToolViewModel(MouseHook mouseHook, TimerTool timerTool)
        {
            this.mouseHook = mouseHook;
            this.timerTool = timerTool;
            this.dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 10);
            dispatcherTimer.Tick += OnTimerTicked;

            InitializeCommands();

            Messenger.Register((sender, e) =>
            {
                if (e.TimerMessage != TimerMessage.StartTimer)
                    return;

                this.timerTool.StartTimer();
                dispatcherTimer.Start();
            });

            Messenger.Register((sender, e) =>
            {
                if (e.TimerMessage != TimerMessage.StopTimer)
                    return;

                this.timerTool.StopTimer();
                dispatcherTimer.Stop();
            });
        }

        private void OnTimerTicked(object sender, EventArgs e)
        {
            OnPropertyChanged("Time");
        }

        private MouseHook mouseHook;
        private DispatcherTimer dispatcherTimer;
        private TimerTool timerTool;

        public bool IsTimerStart { get; set; }
        public bool IsTimerToolOpen { get; set; }
        public long Time => timerTool.Stopwatch.ElapsedMilliseconds;

        public ICommand OpenTimerToolCommand { get; set; }
        public ICommand StartMouseHookCommand { get; set; }
        public ICommand UnHookMouseCommand { get; set; }

        private void InitializeCommands()
        {
            OpenTimerToolCommand = new RelayCommand(p =>
            {
                if (!IsTimerToolOpen)
                {
                    Messenger.Send(this, new TimerEventArgs(TimerMessage.OpenTimer));
                    IsTimerToolOpen = true;
                    StartMouseHookCommand.Execute(null);
                }
                else
                {
                    Messenger.Send(this, new TimerEventArgs(TimerMessage.CloseTimer));
                    IsTimerToolOpen = false;
                    UnHookMouseCommand.Execute(null);
                }
            });

            StartMouseHookCommand = new RelayCommand(p =>
            {
                IsTimerStart = true;
                mouseHook.StartHook();
            }, p => IsTimerStart == false);

            UnHookMouseCommand = new RelayCommand(p =>
            {
                IsTimerStart = false;               
                mouseHook.StopHook();
            }, p => IsTimerStart == true);
        }
    }
}
