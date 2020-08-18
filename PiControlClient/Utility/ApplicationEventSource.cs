using System;
using System.Diagnostics.Tracing;

namespace PiControlClient.Utility
{
    [EventSource(Name = "Application")]
    internal class ApplicationEventSource : EventSource
    {
        public static readonly ApplicationEventSource Log = new ApplicationEventSource();
        
        private static class Keywords
        {
            public const EventKeywords Tracing = (EventKeywords) (1 << 0);
            public const EventKeywords Lifecycle = (EventKeywords) (1 << 1);
        }
        
        [Event(1, Message = "Application starting.",Level = EventLevel.Informational, Keywords = Keywords.Lifecycle)]
        public void Startup() => WriteEvent(1);
        [Event(2, Message = "Second instance start detected.", Level = EventLevel.Informational, Keywords = Keywords.Lifecycle)]
        public void SecondInstanceStarted() => WriteEvent(2);
        [Event(3, Message = "We are not the first instance, shutting down.", Level = EventLevel.Informational, Keywords = Keywords.Lifecycle)]
        public void NotFirstInstance() => WriteEvent(3);
        [Event(4, Message = "Fatal Exception occured, application is exiting:\n{0}", Level = EventLevel.Critical, Keywords = Keywords.Lifecycle)]
        public void FatalException(Exception ex) => WriteEvent(4, ex.ToString());


        [Event(1000, Level = EventLevel.Informational, Keywords = Keywords.Tracing)]
        public void Info(string message) => WriteEvent(1, message);
        [Event(1001, Message = "Dispose Exception occured:\n{0}", Level = EventLevel.Error, Keywords = Keywords.Tracing)]
        public void DisposeException(Exception ex)
        {
            WriteEvent(1001, ex.ToString());
        }
        
    }
}