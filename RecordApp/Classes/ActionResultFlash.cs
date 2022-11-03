using System;
using Microsoft.AspNetCore.Mvc;
using static RecordApp.Classes.FlashMessage;

namespace RecordApp.Classes
{
    public static class ActionResultFlash
    {
        public static ActionResult FlashSuccess(this ActionResult result, Controller controller, string message, int timeout = 5000)
        {
            return Flash(result, controller, message, NotificationType.Success, timeout);
        }

        public static ActionResult FlashError(this ActionResult result, Controller controller, string message, int timeout = 5000)
        {
            return Flash(result, controller, message, NotificationType.Error, timeout);
        }

        public static ActionResult Flash(this ActionResult result, Controller controller, string message, NotificationType type, int timeout)
        {
            controller.TempData["flash"] = new FlashMessage
            {
                Message = message,
                Level = type,
                HideTimeout = timeout
            };
            return result;
        }
    }
}
