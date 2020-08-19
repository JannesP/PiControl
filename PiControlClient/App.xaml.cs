using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Threading;
using PiControlClient.Extensions;
using PiControlClient.Logging;
using PiControlClient.Resources;
using PiControlClient.UI.Windows;
using PiControlClient.Utility;

namespace PiControlClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    // ReSharper disable once RedundantExtendsListEntry
    public partial class App : Application
    {
        private TrayIconManager? _trayIconManager;
        private SingleProcessManager? _singleProcessManager;
        
        protected override void OnStartup(StartupEventArgs e)
        {
            AppDomain.CurrentDomain.UnhandledException += OnCurrentDomainUnhandledException;
            this.DispatcherUnhandledException += OnDispatcherUnhandledException;
            
            var _ = ConsoleEventListener.Instance;
            ApplicationEventSource.Log.Startup();
            this.ShutdownMode = ShutdownMode.OnExplicitShutdown;
            _singleProcessManager = new SingleProcessManager("com_jannes_peters_PiControlWpfClient");
            if (!_singleProcessManager.IsFirstInstance)
            {
                ApplicationEventSource.Log.NotFirstInstance();
                this.Shutdown(0);
                return;
            }
            _singleProcessManager.SecondInstanceStarted += OnSecondInstanceStarted;
            var createUi = true;
            foreach (string arg in e.Args)
            {
                switch (arg)
                {
                    case "-tray":
                        createUi = false;
                        break;
                    default:
                        Trace.TraceWarning($"Got invalid argument '{arg}', ignoring ...");
                        break;
                }
            }

            _trayIconManager = new TrayIconManager(ResourcePaths.TrayIcon) {IconVisible = true};
            _trayIconManager.ItemExitClick += (sender, eventArgs) => this.Shutdown(0);
            _trayIconManager.DoubleClick += (sender, eventArgs) => this.ShowCreateMainWindow<MainWindow>();
            if (createUi)
            {
                this.ShowCreateMainWindow<MainWindow>();
            }
            base.OnStartup(e);
        }

        private void OnSecondInstanceStarted(object? sender, EventArgs e)
        {
            ApplicationEventSource.Log.SecondInstanceStarted();
            this.Dispatcher.Invoke(this.ShowCreateMainWindow<MainWindow>);
        }

        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            ApplicationEventSource.Log.FatalException(e.Exception);
            Cleanup();
        }
        
        private void OnCurrentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = (Exception)e.ExceptionObject;
            ApplicationEventSource.Log.FatalException(ex);
            Cleanup();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            Cleanup();
        }
        
        private void Cleanup()
        {
            _trayIconManager?.SafeDispose();
            _singleProcessManager?.SafeDispose();
            ApplicationEventSource.Log.SafeDispose();
            ConsoleEventListener.Instance.SafeDispose();
        }
        
    }
}