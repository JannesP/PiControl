using System;

namespace PiControlClient.Resources
{
    internal static class ResourcePaths
    {
        private static string BaseResourcePath => "PiControlClient.Resources.";
        private static string Concat(string fileName) => BaseResourcePath + fileName;
        
        public static string TrayIcon => Concat("tray.ico");
    }
}