using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;

namespace PiControlClient.Utility
{
    internal static class ApplicationManagementUtil
    {
        private static string RegistryAutostartKey => @"Software\Microsoft\Windows\CurrentVersion\Run";
        
        /// <summary>
        /// Restarts the current executable. You can use this if you used the WpfUtil Mutex to handle multiple instances.
        /// </summary>
        public static void Restart()
        {
            Application.Current.Exit += (o, args) =>
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = FullAssemblyPath,
                    WorkingDirectory = Process.GetCurrentProcess().StartInfo.WorkingDirectory
                });
            };
            Application.Current.Shutdown();
        }
        
        /// <summary>
        /// Adds the program to the autostart in the registry. You should probably use some command line args to prevent the gui from opening.
        /// If the regkeyname is not set it will use the executable name without the extension.
        /// </summary>
        /// <param name="additionalArgs">additional command line args to be used</param>
        /// <param name="regKeyName">If null it will use the executable name. Note that removeFromAutostart always uses the last regKeyName that was used.</param>
        /// <returns>if the key was set successfully</returns>
        public static bool AddToAutostart(string? additionalArgs = null, string? regKeyName = null)
        {
            regKeyName ??= AssemblyNameWithoutExtension;
            string args;
            if (additionalArgs == null)
            {
                args = "";
            }
            else
            {
                args = " " + additionalArgs;
            }
            
            try
            {
                using (RegistryKey? key = Registry.CurrentUser.OpenSubKey(RegistryAutostartKey, true))
                {
                    if (key != null)
                    {
                        key.SetValue(regKeyName, $"\"{FullAssemblyPath}\"{args}");
                        return true;
                    }
                    else
                    {
                        Trace.TraceError("Autostart key couldn't be opened!");
                    }
                }
            }
            catch (SecurityException ex)
            {
                Trace.TraceError("Error AddToAutostart:\n{0}", ex.GetType().Name + ex.Message);
            }
            return false;
        }

        /// <summary>
        /// Removes the program from autostart, deleting the registry key. It will use the last key name that was used to set in this instance.
        /// </summary>
        /// <returns>if the key was removed successfully</returns>
        public static bool RemoveFromAutostart(string? regKeyName = null)
        {
            regKeyName ??= AssemblyNameWithoutExtension;
            try
            {
                using (RegistryKey? key = Registry.CurrentUser.OpenSubKey(RegistryAutostartKey, true))
                {
                    if (key != null)
                    {
                        if (key.GetValue(regKeyName) != null)
                        {
                            key.DeleteValue(regKeyName);
                        }
                        return true;
                    }
                    else
                    {
                        Trace.TraceError("Autostart key couldn't be opened!");
                    }
                }
            }
            catch (SecurityException ex)
            {
                Trace.TraceError("Error RemoveFromAutostart:\n{0}", ex.GetType().Name + ex.Message);
            }
            return false;
        }
        
        /// <summary>
        /// Required because the Assembly.Location doesn't handle all special characters (like '#' for example).
        /// </summary>
        public static string FullAssemblyPath
        {
            get
            {
                Assembly? entryAssembly = Assembly.GetEntryAssembly();
                Debug.Assert(entryAssembly != null, nameof(entryAssembly) + " != null");
                string? codeBasePseudoUrl = entryAssembly.CodeBase;
                const string filePrefix3 = @"file:///";
                Debug.Assert(codeBasePseudoUrl != null, nameof(codeBasePseudoUrl) + " != null");
                if (codeBasePseudoUrl.StartsWith(filePrefix3))
                {
                    string sPath = codeBasePseudoUrl.Substring(filePrefix3.Length);
                    return sPath.Replace('/', '\\');
                }
                return entryAssembly.Location;
            }
        }
        
        /// <summary>
        /// Retrieves the entry assembly name (usually the executed .exe file) of the current application. 
        /// </summary>
        public static string AssemblyNameWithoutExtension => Path.GetFileNameWithoutExtension(FullAssemblyPath);
    }
}