using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Pixelmaniac.Notifications.Controls
{
    [TemplatePart(Name = "PART_Items", Type = typeof(Panel))]
    public class NotificationArea : Control
    {
        #region Fields

        private IList _notificationItems;

        #endregion

        #region Ctor

        public NotificationArea()
        {
            if (!DesignerProperties.GetIsInDesignMode(this))
                Panel.SetZIndex(this, 1);
        }

        static NotificationArea()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NotificationArea),
                new FrameworkPropertyMetadata(typeof(NotificationArea)));
        }

        #endregion

        #region Properties

        public static readonly DependencyProperty PositionProperty =
            DependencyProperty.Register(nameof(Position), typeof(NotificationPosition), typeof(NotificationArea), new PropertyMetadata(NotificationPosition.BottomRight));

        public static readonly DependencyProperty MaxNotificationsCountProperty =
            DependencyProperty.Register(nameof(MaxNotificationsCount), typeof(int), typeof(NotificationArea), new PropertyMetadata(int.MaxValue));

        public NotificationPosition Position
        {
            get => (NotificationPosition)GetValue(PositionProperty);
            set => SetValue(PositionProperty, value);
        }

        public int MaxNotificationsCount
        {
            get => (int)GetValue(MaxNotificationsCountProperty);
            set => SetValue(MaxNotificationsCountProperty, value);
        }

        #endregion

        #region Public Methods

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (GetTemplateChild("PART_Items") is Panel panel)
                _notificationItems = panel.Children;
        }

        public async void ShowNotification(object content, TimeSpan expirationTime, Action onClickAction, Action onCloseAction)
        {
            var notification = new Notification
            {
                Content = content
            };

            notification.MouseLeftButtonDown += (sender, args) =>
            {
                if (onClickAction != null)
                {
                    onClickAction.Invoke();
                    (sender as Notification)?.Close();
                }
            };

            notification.NotificationClosed += (sender, args) => onCloseAction?.Invoke();
            notification.NotificationClosed += OnNotificationClosed;

            if (!IsLoaded)
                return;

            var w = Window.GetWindow(this);
            var x = w == null ? null : PresentationSource.FromVisual(w);

            if (x == null)
                return;

            lock (_notificationItems)
            {
                _notificationItems.Add(notification);

                if (_notificationItems.OfType<Notification>().Count(i => !i.IsClosing) > MaxNotificationsCount)
                    _notificationItems.OfType<Notification>().First(i => !i.IsClosing).Close();
            }

            if (expirationTime == TimeSpan.MaxValue)
                return;

            await Task.Delay(expirationTime);

            notification.Close();
        }

        #endregion

        #region Private Methods

        private void OnNotificationClosed(object sender, RoutedEventArgs e)
        {
            if (sender is Notification notification)
                _notificationItems.Remove(notification);
        }

        #endregion
    }
}
