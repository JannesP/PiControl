using System;
using System.Diagnostics.Tracing;

namespace PiControlClient.Logging
{
    [EventSource(Name = "PiControlClient")]
    internal class ApplicationEventSource : EventSource
    {
        public static readonly ApplicationEventSource Log = new ApplicationEventSource();
        public static class Keywords
        {
            public const EventKeywords Tracing = (EventKeywords) (1 << 0);
            public const EventKeywords Lifecycle = (EventKeywords) (1 << 1);
        }
        
        [Event(1, Message = "Application starting.",Level = EventLevel.Informational, Keywords = Keywords.Lifecycle, Channel = EventChannel.Debug)]
        public void Startup() => WriteEvent(1);
        [Event(2, Message = "Second instance start detected.", Level = EventLevel.Informational, Keywords = Keywords.Lifecycle, Channel = EventChannel.Debug)]
        public void SecondInstanceStarted() => WriteEvent(2);
        [Event(3, Message = "We are not the first instance, shutting down.", Level = EventLevel.Informational, Keywords = Keywords.Lifecycle, Channel = EventChannel.Debug)]
        public void NotFirstInstance() => WriteEvent(3);
        
        [NonEvent]
        public void FatalException(Exception ex) => FatalException(ex.ToString());
        [Event(4, Message = "Fatal Exception occured, application is exiting:\n{0}", Level = EventLevel.Critical, Keywords = Keywords.Lifecycle, Channel = EventChannel.Debug)]
        public void FatalException(string ex) => WriteEvent(4, ex);
        
        [Event(1000, Level = EventLevel.Informational, Keywords = Keywords.Tracing, Channel = EventChannel.Debug)]
        public void Info(string message) => WriteEvent(1000, message);
        [NonEvent]
        public void DisposeException(Exception ex) => DisposeException(ex.ToString());
        [Event(1001, Message = "Dispose Exception occured:\n{0}", Level = EventLevel.Error, Keywords = Keywords.Tracing, Channel = EventChannel.Debug)]
        public void DisposeException(string exception) => WriteEvent(1001, exception);
        
    }
}