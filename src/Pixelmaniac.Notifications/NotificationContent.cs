using System.Reflection;
using System.Windows.Media;

namespace Pixelmaniac.Notifications
{
    public class NotificationContent
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public string AppIdentity { get; set; } = Assembly.GetEntryAssembly()?.GetName().Name;
        public string AttributionText { get; set; }

        public ImageSource Icon { get; set; }
        public StreamGeometry VectorIcon { get; set; }
        public Brush VectorIconBrush { get; set; }
        public bool UseLargeIcon { get; set; }
    }
}
