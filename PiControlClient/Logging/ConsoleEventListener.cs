using System;
using System.Diagnostics.Tracing;

namespace PiControlClient.Logging
{
    public class ConsoleEventListener : EventListener
    {
        public static readonly ConsoleEventListener Instance = new ConsoleEventListener();
        
        protected override void OnEventSourceCreated(EventSource eventSource)
        {
            Console.WriteLine($"{eventSource.Guid} | {eventSource.Name}");
            if (eventSource.Name == "PiControlClient")
            {
                EnableEvents(eventSource, EventLevel.Informational);
            }
        }

        protected override void OnEventWritten(EventWrittenEventArgs eventData)
        {
            Console.WriteLine($"Event Fired: {eventData.Message}");
        }
    }
}