using System;
using System.Windows;
using System.Windows.Input;
using PiControlClient.Extensions;
using PiControlClient.Utility;

namespace PiControlClient.UI.Windows
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            ShowSettingsCommand = new SimpleCommand<Window>(ShowSettingsDialog);
            ExitApplication = new SimpleCommand(() => Application.Current.Shutdown(0));
        }

        public ICommand ShowSettingsCommand { get; }
        public ICommand ExitApplication { get; }
        
        private static void ShowSettingsDialog(Window? parent)
        {
            var winSettings = new SettingsWindow();
            if (parent != null) winSettings.CenterOn(parent);
            winSettings.ShowDialog();
        }
    }
}