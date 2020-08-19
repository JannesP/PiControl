using System;
using System.Windows;

namespace PiControlClient.Extensions
{
    public static class WindowExtensions
    {
        public static T CenterOn<T>(this T dialog, Window centerOn) where T : Window
        {
            double left = centerOn.Left + ((centerOn.ActualWidth / 2) - (dialog.Width / 2));
            double top = centerOn.Top + ((centerOn.ActualHeight / 2) - (dialog.Height / 2));

            if (left < SystemParameters.VirtualScreenLeft) left = SystemParameters.VirtualScreenLeft;
            if (top < SystemParameters.VirtualScreenTop) top = SystemParameters.VirtualScreenTop;
            double screenRight = SystemParameters.VirtualScreenLeft + SystemParameters.VirtualScreenWidth;
            if ((left + dialog.Width) > screenRight)
            {
                left = screenRight - dialog.Width;
            }
            double screenBottom = SystemParameters.VirtualScreenTop + SystemParameters.VirtualScreenHeight;
            if ((top + dialog.Height) > screenBottom)
            {
                left = screenBottom - dialog.Height;
            }
            
            dialog.Top = top;
            dialog.Left = left;
            return dialog;
        }
    }
}