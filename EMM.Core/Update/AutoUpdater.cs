using EMM.Core.ViewModels;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Windows;
using System.Windows.Input;

namespace EMM.Core.Update
{
    public class AutoUpdater : IAutoUpdater
    {
        #region Ctor

        public AutoUpdater(IMessageBoxService messageBoxService)
        {
            this.messageBoxService = messageBoxService;

            InitializeCommands();
        }

        #endregion

        #region Private members

        private IMessageBoxService messageBoxService;

        private UpdateInfo updateInfo;

        private static string updateInfoUrl = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "Setting", "Url"));

        private static string updaterName = "EMMUpdater.exe";

        private string updaterExePath = Path.Combine(Environment.CurrentDirectory, updaterName);

        #endregion

        #region Public Properties

        public bool IsStartUp { get; set; } = true;

        #endregion

        #region Commands

        public ICommand CheckForUpdateCommand { get; set; }

        private void InitializeCommands()
        {
            CheckForUpdateCommand = new RelayCommand(p =>
            {
                this.IsStartUp = false;
                this.CheckForUpdate();
            });
        }

        #endregion

        #region Interface Implement

        /// <summary>
        /// Check for update
        /// </summary>
        /// <returns></returns>
        public async void CheckForUpdate()
        {
            var updateInfoString = string.Empty;
            using (WebClient wc = new WebClient())
            {
                wc.CachePolicy = new System.Net.Cache.RequestCachePolicy(System.Net.Cache.RequestCacheLevel.BypassCache);
                try
                {
                    updateInfoString = await wc.DownloadStringTaskAsync(new System.Uri(updateInfoUrl));

                    var currentVersion = Assembly.GetEntryAssembly().GetName().Version;

                    updateInfo = TryToParseUpdateInfo(updateInfoString);

                    if (updateInfo?.Version?.CompareTo(currentVersion) > 0)
                    {
                        if (UpdateConfirmation() == MessageResult.Yes)
                        {
                            Update();
                        }
                    }
                    else if (!IsStartUp)
                    {
                        this.messageBoxService.ShowMessageBox("You're already running the latest version", "Update", MessageButton.OK, MessageImage.Information);
                    }
                }
                catch
                {
                    if (IsStartUp == false)
                        this.messageBoxService.ShowMessageBox("Fail to get update infomation. Please check your internet connection!");
                }
            }
        }

        public void Update()
        {
            if (!File.Exists(updaterExePath))
            {
                this.messageBoxService.ShowMessageBox("Can not find EMMUpdater.exe! You can re-download the latest version here: \n");
                return;
            }

            using (Process process = new Process())
            {
                var processInfo = new ProcessStartInfo(updaterName);
                processInfo.WorkingDirectory = Environment.CurrentDirectory;
                processInfo.Arguments = updateInfo.Url;
                process.StartInfo = processInfo;
                process.Start();
            }

            Application.Current.Shutdown();
        }

        #endregion

        #region Helpers

        private MessageResult UpdateConfirmation()
        {
            return messageBoxService.ShowMessageBox("There's a new version available. Do you want to update?", "Update", MessageButton.YesNo, MessageImage.Information);
        }

        private UpdateInfo TryToParseUpdateInfo(string updateInfoString)
        {
            try
            {
                return JsonConvert.DeserializeObject<UpdateInfo>(updateInfoString);
            }
            catch
            {
                return null;
            }
        } 

        #endregion

    }
}
