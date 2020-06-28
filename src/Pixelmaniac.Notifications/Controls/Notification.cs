using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Pixelmaniac.Notifications.Controls
{
    [TemplatePart(Name = "PART_CloseButton", Type = typeof(Button))]
    public class Notification : ContentControl
    {
        #region Fields

        private static readonly TimeSpan ClosingAnimationDuration = TimeSpan.FromMilliseconds(200);

        #endregion

        #region Ctor

        static Notification()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Notification), new FrameworkPropertyMetadata(typeof(Notification)));
        }

        #endregion

        #region Events

        public static readonly RoutedEvent NotificationClosingEvent = EventManager.RegisterRoutedEvent(
            nameof(NotificationClosing), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(Notification));

        public static readonly RoutedEvent NotificationClosedEvent = EventManager.RegisterRoutedEvent(
            nameof(NotificationClosed), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(Notification));

        public event RoutedEventHandler NotificationClosing
        {
            add => AddHandler(NotificationClosingEvent, value);
            remove => RemoveHandler(NotificationClosingEvent, value);
        }

        public event RoutedEventHandler NotificationClosed
        {
            add => AddHandler(NotificationClosedEvent, value);
            remove => RemoveHandler(NotificationClosedEvent, value);
        }

        #endregion

        #region Properties

        public bool IsClosing { get; set; }

        public static readonly DependencyProperty CloseButtonStyleProperty = DependencyProperty.Register(
            nameof(CloseButtonStyle), typeof(Style), typeof(Notification), new PropertyMetadata(default(Style)));

        public Style CloseButtonStyle
        {
            get => (Style) GetValue(CloseButtonStyleProperty);
            set => SetValue(CloseButtonStyleProperty, value);
        }

        public static readonly DependencyProperty CloseOnClickProperty =
            DependencyProperty.RegisterAttached("CloseOnClick", typeof(bool), typeof(Notification), new FrameworkPropertyMetadata(false, OnCloseOnClickChanged));

        public static bool GetCloseOnClick(DependencyObject obj)
        {
            return (bool)obj.GetValue(CloseOnClickProperty);
        }

        public static void SetCloseOnClick(DependencyObject obj, bool value)
        {
            obj.SetValue(CloseOnClickProperty, value);
        }

        #endregion

        #region Public Methods

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (GetTemplateChild("PART_CloseButton") is Button closeButton)
                closeButton.Click += OnCloseButtonClick;
        }

        public async void Close()
        {
            if (IsClosing)
                return;

            IsClosing = true;

            RaiseEvent(new RoutedEventArgs(NotificationClosingEvent));
            await Task.Delay(ClosingAnimationDuration);
            RaiseEvent(new RoutedEventArgs(NotificationClosedEvent));
        }

        #endregion

        #region Private Methods

        private static void OnCloseOnClickChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (!(obj is Button button))
                return;

            var value = (bool)e.NewValue;

            if (value)
            {
                button.Click += (sender, args) =>
                {
                    var notification = button.FindVisualParent<Notification>();
                    notification?.Close();
                };
            }
        }

        private void OnCloseButtonClick(object sender, RoutedEventArgs e)
        {
            if (!(sender is Button button))
                return;

            button.Click -= OnCloseButtonClick;
            Close();
        }

        #endregion
    }
}
