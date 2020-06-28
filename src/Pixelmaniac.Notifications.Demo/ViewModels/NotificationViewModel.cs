using System.Threading.Tasks;
using Caliburn.Micro;

namespace Pixelmaniac.Notifications.Demo.ViewModels
{
    internal sealed class NotificationViewModel : PropertyChangedBase
    {
        private readonly NotificationManager _notificationManager;
        private string _title;
        private string _message;

        // Designer

        public NotificationViewModel() : this(IoC.Get<NotificationManager>()) { }

        public NotificationViewModel(NotificationManager notificationManager)
        {
            _notificationManager = notificationManager;
        }

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

        public async void Ok()
        {
            await Task.Delay(500);
            _notificationManager.Notify(string.Empty, "OK button was clicked");
        }

        public async void Cancel()
        {
            await Task.Delay(500);
            _notificationManager.Notify(string.Empty, "Cancel button was clicked");
        }
    }
}
