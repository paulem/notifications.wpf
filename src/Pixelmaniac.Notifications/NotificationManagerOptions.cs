using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pixelmaniac.Notifications
{
    public class NotificationManagerOptions
    {
        internal NotificationManagerOptions() { }

        public bool InAppNotificationPlacement { get; set; } = true;
        public bool SuppressAppShutdownModeVerification { get; set; }
    }
}
