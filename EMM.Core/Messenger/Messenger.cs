using System;
using System.Collections.Generic;

namespace EMM.Core
{
    public class ToolEventArgs : EventArgs
    {
        public ToolEventArgs(ToolMessage message)
        {
            this.ToolMessage = message;
        }

        public ToolEventArgs()
        {

        }

        public ToolMessage ToolMessage { get; set; }
    }

    public class DropEventArgs : EventArgs
    {
        public DropEventArgs()
        {

        }

        public DropEventArgs(string[] filepaths)
        {
            this.FilePaths = filepaths;
        }

        public string[] FilePaths { get; set; }
    }

    public class AppEventArgs : EventArgs
    {
        public AppEventArgs(EventMessage eventMessage)
        {
            this.EventMessage = eventMessage;
        }
        public AppEventArgs()
        {

        }
        public EventMessage EventMessage { get; set; }
    }

    /// <summary>
    /// This class provide event through out the application
    /// </summary>
    public static class Messenger
    {        
        private static event EventHandler<AppEventArgs> AppEvent;

        private static event EventHandler<ToolEventArgs> ToolEvent;

        private static event EventHandler<DropEventArgs> DropEvent;

        public static void Send(object sender, AppEventArgs e)
        {                    
            AppEvent.Invoke(sender, e);
        }
        public static void Send(object sender, ToolEventArgs e)
        {
            ToolEvent?.Invoke(sender, e);
        }
        public static void Send(object sender, DropEventArgs e)
        {
            DropEvent?.Invoke(sender, e);
        }

        public static void Register(EventHandler<AppEventArgs> eventHandler)
        {
            AppEvent -= eventHandler;
            AppEvent += eventHandler;
        }
        public static void Register(EventHandler<ToolEventArgs> eventHandler)
        {
            ToolEvent -= eventHandler;
            ToolEvent += eventHandler;
        }
        public static void Register(EventHandler<DropEventArgs> eventHandler)
        {
            DropEvent -= eventHandler;
            DropEvent += eventHandler;
        }

        public static void UnRegister(EventHandler<AppEventArgs> eventHandler)
        {
            AppEvent -= eventHandler;
        }
        public static void UnRegister(EventHandler<ToolEventArgs> eventHandler)
        {
            ToolEvent -= eventHandler;
        }
        public static void UnRegister(EventHandler<DropEventArgs> eventHandler)
        {
            DropEvent -= eventHandler;
        }
    }  
}
