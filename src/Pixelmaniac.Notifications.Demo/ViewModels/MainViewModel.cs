using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Caliburn.Micro;
using Pixelmaniac.Notifications.Controls;

namespace Pixelmaniac.Notifications.Demo.ViewModels
{
    public sealed class MainViewModel : Screen
    {
        private readonly NotificationManager _notificationManager;

        private string _title;
        private string _message;
        private string _attributionText;
        private bool _useIcon = true;
        private bool _useLargeIcon = true;
        private bool _useVectorIcon = true;
        private bool _inAppPlacement = true;
        private string _appIdentity;
        private bool _useCustomNotification;

        // Designer

        public MainViewModel() : this(IoC.Get<NotificationManager>()) { }

        public MainViewModel(NotificationManager notificationManager)
        {
            _notificationManager = notificationManager;

            Title = "Pixelmaniac";
            Message = "This is a test message to inform you about something quite important.";
            AppIdentity = "App";
            AttributionText = "Via PXMC";
        }

        public override string DisplayName { get; set; } = "Notifications";

        public string Title
        {
            get => _title;
            set
            {
                if (value == _title) return;
                _title = value;
                NotifyOfPropertyChange();
            }
        }

        public string Message
        {
            get => _message;
            set
            {
                if (value == _message) return;
                _message = value;
                NotifyOfPropertyChange();
            }
        }

        public string AttributionText
        {
            get => _attributionText;
            set
            {
                if (value == _attributionText) return;
                _attributionText = value;
                NotifyOfPropertyChange();
            }
        }

        public string AppIdentity
        {
            get => _appIdentity;
            set
            {
                if (value == _appIdentity) return;
                _appIdentity = value;
                NotifyOfPropertyChange();
            }
        }

        public bool UseIcon
        {
            get => _useIcon;
            set
            {
                if (value == _useIcon) return;
                _useIcon = value;
                NotifyOfPropertyChange();
            }
        }

        public bool UseVectorIcon
        {
            get => _useVectorIcon;
            set
            {
                if (value == _useVectorIcon) return;
                _useVectorIcon = value;
                NotifyOfPropertyChange();
            }
        }

        public bool UseLargeIcon
        {
            get => _useLargeIcon;
            set
            {
                if (value == _useLargeIcon) return;
                _useLargeIcon = value;
                NotifyOfPropertyChange();
            }
        }

        public bool UseCustomNotification
        {
            get => _useCustomNotification;
            set
            {
                if (value == _useCustomNotification) return;
                _useCustomNotification = value;
                NotifyOfPropertyChange();
            }
        }

        public bool InAppPlacement
        {
            get => _inAppPlacement;
            set
            {
                if (value == _inAppPlacement) return;
                _inAppPlacement = value;
                NotifyOfPropertyChange();
            }
        }

        //

        public void ShowNotification()
        {
            if (UseCustomNotification)
                ShowCustomNotification();
            else
                ShowDefaultNotification();
        }

        public void ShowDefaultNotification()
        {
            _notificationManager.Options.InAppNotificationPlacement = InAppPlacement;

            var content = new NotificationContent
            {
                Title = Title,
                Message = Message,
                AttributionText = AttributionText,
                UseLargeIcon = UseLargeIcon
            };

            if (!string.IsNullOrEmpty(AppIdentity))
                content.AppIdentity = AppIdentity;

            if (UseIcon)
            {
                if (UseVectorIcon)
                {
                    content.VectorIcon =
                        Application.Current.TryFindResource("Geometry.Icon.16") as
                            StreamGeometry;

                    content.VectorIconBrush = (SolidColorBrush)new BrushConverter().ConvertFrom("#da552f");
                }
                else
                    content.Icon =
                        new BitmapImage(
                            new Uri($"pack://application:,,,/Assets/icon-{(UseLargeIcon ? "48" : "16")}.png"));
            }

            _notificationManager.Notify(content); // (content, onClose: () => _notificationManager.Notify("", "Closed"))
        }

        public void ShowCustomNotification()
        {
            _notificationManager.Options.InAppNotificationPlacement = InAppPlacement;

            var vm = new NotificationViewModel(_notificationManager) { Title = Title, Message = Message };
            _notificationManager.Notify(vm);
        }

        public void ShowWindowWithNotifications()
        {
            if (!(GetView() is Window currentWindow))
                return;

            var width = currentWindow.ActualWidth / 2;
            var height = currentWindow.ActualHeight / 2;

            var wnd = new Window
            {
                Topmost = true,
                Width = width,
                Height = height,
                ShowInTaskbar = false,
                Content = new NotificationArea()
            };

            wnd.Show();
        }
    }
}
