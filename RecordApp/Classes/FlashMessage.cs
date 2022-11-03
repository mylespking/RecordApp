using System;
using System.Collections.Generic;

namespace RecordApp.Classes
{
    public class FlashMessage
    {
        private static readonly IDictionary<NotificationType, string> _alertType = new Dictionary<NotificationType, string>
        {
            [NotificationType.Error] = "alert-danger",
            [NotificationType.Success] = "alert-success"
        };

        public string Message { get; set; }

        // Sets the level of the message
        public NotificationType Level { get; set; }

        public string Css => _alertType[Level];

        // Number
        public int HideTimeout { get; set; } = 5000;

        public enum NotificationType
        {
            Success,
            Error
        }
    }
}
