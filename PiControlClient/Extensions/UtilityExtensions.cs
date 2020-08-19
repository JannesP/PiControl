using System;
using PiControlClient.Logging;
using PiControlClient.Utility;

namespace PiControlClient.Extensions
{
    internal static class UtilityExtensions
    {
        public static void SafeDispose(this IDisposable disposable)
        {
            try
            {
                disposable?.Dispose();
            }
            catch (Exception ex)
            {
                ApplicationEventSource.Log.DisposeException(ex);
            }
        }
    }
}