using System;
using System.Linq;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Threading;
using Pixelmaniac.Notifications.Controls;

namespace Pixelmaniac.Notifications
{
    public class NotificationManager
    {
        #region Fields

        private static Window _overlayWindow;
        private readonly Dispatcher _dispatcher;

        private readonly TimeSpan _defaultExpirationTime = TimeSpan.FromSeconds(10);

        #endregion

        #region Ctor

        public NotificationManager()
        {
            _dispatcher = Application.Current?.Dispatcher ?? Dispatcher.CurrentDispatcher;
            Options = new NotificationManagerOptions();
        }

        #endregion

        #region Properties

        public NotificationManagerOptions Options { get; }

        #endregion

        #region Public Methods

        public void Notify(string message, string title = "", string attributionText = "", bool showIcon = false, TimeSpan? expirationTime = null)
        {
            var content = new NotificationContent { Message = message, AttributionText = attributionText };
            content.Title = !string.IsNullOrEmpty(title) ? title : content.AppIdentity;

            if (showIcon)
                content.VectorIcon = Application.Current.TryFindResource("Geometry.Notification.16") as StreamGeometry;

            Notify(content);
        }

        public void Notify(object content, TimeSpan? expirationTime = null, Action onClick = null, Action onClose = null)
        {
            if (!_dispatcher.CheckAccess())
            {
                _dispatcher.BeginInvoke(new Action(() => Notify(content, expirationTime, onClick, onClose)));
                return;
            }

            expirationTime ??= _defaultExpirationTime;

            if (!Options.InAppNotificationPlacement || !(Application.Current.Windows.OfType<Window>().FirstOrDefault(x => x.IsActive) is Window activeWnd) || !(activeWnd.FindVisualChild<NotificationArea>() is NotificationArea area))
                ShowNotificationInOverlayWindow(content, expirationTime.Value, onClick, onClose);
            else
                area.ShowNotification(content, (TimeSpan) expirationTime, onClick, onClose);
        }

        #endregion

        #region Private Methods

        private static void ShowNotificationInOverlayWindow(object content, TimeSpan expirationTime, Action onClick = null, Action onClose = null)
        {
            if (Application.Current.ShutdownMode == ShutdownMode.OnLastWindowClose)
                throw new Exception(
                    "Application \"ShutdownMode.OnLastWindowClose\" default setting will prevent your application from closing completely after notifications are displayed. " +
                    "NotificationManager creates an invisible window that may prevent your application from closing completely. " +
                    "To avoid this problem, change Application.Current.ShutdownMode setting or set Options.InAppNotificationPlacement to true to show notifications inside application window.");

            _overlayWindow ??= CreateOverlayWindow();
            _overlayWindow.Show();

            ((NotificationArea)_overlayWindow.Content).ShowNotification(content, expirationTime, onClick, onClose);
        }

        private static Window CreateOverlayWindow()
        {
            var wnd = new Window
            {
                WindowStyle = WindowStyle.None,
                ShowInTaskbar = false,
                AllowsTransparency = true,
                Background = Brushes.Transparent,
                Topmost = true,
                ShowActivated = false,
                Focusable = false,
                Left = SystemParameters.WorkArea.Left,
                Top = SystemParameters.WorkArea.Top,
                Width = SystemParameters.WorkArea.Width,
                Height = SystemParameters.WorkArea.Height,

                Content = new NotificationArea()
            };

            wnd.Loaded += (sender, args) =>
            {
                IntPtr hwnd = new WindowInteropHelper((Window)sender).Handle;

                int extendedStyle = NativeMethods.GetWindowLong(hwnd, NativeMethods.GWL_EXSTYLE);
                _ = NativeMethods.SetWindowLong(hwnd, NativeMethods.GWL_EXSTYLE, extendedStyle | NativeMethods.WS_EX_TOOLWINDOW);
            };

            return wnd;
        }

        #endregion
    }
}
