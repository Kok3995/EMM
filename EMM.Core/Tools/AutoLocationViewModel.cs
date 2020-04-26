using EMM.Core.ViewModels;
using System;
using System.Windows;
using System.Windows.Input;

namespace EMM.Core.Tools
{
    public class AutoLocationViewModel : BaseViewModel
    {
        public AutoLocationViewModel(MouseHook mouseHook, AutoLocation autoLocation, MacroManagerViewModel macroManager)
        {
            this.autoLocation = autoLocation;
            this.mouseHook = mouseHook;
            this.macroManager = macroManager;

            InitializeCommands();
        }

        private AutoLocation autoLocation;
        private MouseHook mouseHook;
        private MacroManagerViewModel macroManager;
        private IntPtr targetWindowHandle = IntPtr.Zero;

        public ICommand ToggleCaptureLocationCommand { get; set; }

        public bool IsCaptureStarted { get; set; }

        private void InitializeCommands()
        {
            ToggleCaptureLocationCommand = new RelayCommand(p =>
            {
                if (!IsCaptureStarted) {
                    this.mouseHook.StartHook(MouseHookCallBack);
                    IsCaptureStarted = true;
                }
                else
                {
                    this.mouseHook.StopHook();
                    IsCaptureStarted = false;
                }

                this.targetWindowHandle = IntPtr.Zero;
            });
        }

        private IntPtr MouseHookCallBack(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && (MouseMessages)wParam == MouseMessages.WM_RBUTTONDOWN)
            {
                if (this.targetWindowHandle == IntPtr.Zero)
                    this.targetWindowHandle = this.autoLocation.GetForegroundWindowHandler(); //Set the target window

                if (this.targetWindowHandle == this.autoLocation.GetForegroundWindowHandler()) //Only handle the target window
                {
                    var relativePos = this.autoLocation.GetClientCoordinates(this.targetWindowHandle);
                    var clientRect = this.autoLocation.GetClientRect(this.targetWindowHandle);
                    var scaleX = this.macroManager.GetCurrentMacro().OriginalX / clientRect.Width;
                    var scaleY = this.macroManager.GetCurrentMacro().OriginalY / clientRect.Height;

                    SetLocationRecursive(this.macroManager.GetCurrentSelectedActionViewModel(), relativePos.X, scaleX, relativePos.Y, scaleY);
                }
            }

            return MouseHook.CallNextHookEx(mouseHook.GetCurrentHookID(), nCode, wParam, lParam);
        }

        private void SetLocationRecursive(IActionViewModel actionViewModel, double x, double scaleX, double y, double scaleY)
        {
            if (actionViewModel is ILocationSettable selectedAction)
            {
                selectedAction.SetLocation(new Point(Math.Round(x * scaleX, MidpointRounding.AwayFromZero), Math.Round(y * scaleY, MidpointRounding.AwayFromZero)));
            }
            else if (actionViewModel is CommonViewModel commonViewModel)
            {
                SetLocationRecursive(commonViewModel.SelectedItem, x, scaleX, y, scaleY);
            }
        }
    }
}
