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
                    Messenger.Send(this, new ToolEventArgs(ToolMessage.OpenTimer));
                    IsTimerToolOpen = true;
                    StartMouseHookCommand.Execute(null);
                }
                else
                {
                    Messenger.Send(this, new ToolEventArgs(ToolMessage.CloseTimer));
                    IsTimerToolOpen = false;
                    UnHookMouseCommand.Execute(null);
                }
            });

            StartMouseHookCommand = new RelayCommand(p =>
            {
                IsTimerStart = true;
                mouseHook.StartHook(LeftClickHookCallBack);
            }, p => IsTimerStart == false);

            UnHookMouseCommand = new RelayCommand(p =>
            {
                IsTimerStart = false;               
                mouseHook.StopHook();
            }, p => IsTimerStart == true);
        }

        private IntPtr LeftClickHookCallBack(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && (MouseMessages)wParam == MouseMessages.WM_LBUTTONDOWN)
            {
                this.timerTool.StartTimer();
                dispatcherTimer.Start();
            }

            if (nCode >= 0 && (MouseMessages)wParam == MouseMessages.WM_LBUTTONUP)
            {
                this.timerTool.StopTimer();
                dispatcherTimer.Stop();
            }

            return MouseHook.CallNextHookEx(mouseHook.GetCurrentHookID(), nCode, wParam, lParam);
        }
    }
}
