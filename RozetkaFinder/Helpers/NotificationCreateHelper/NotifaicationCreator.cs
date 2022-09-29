using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using RozetkaFinder.Services.Notification;

namespace RozetkaFinder.Helpers.NotificationCreateHelper
{
    public class NotifaicationCreator
    {
        public INotificationService CreateNotificationService(int notifacation)
        {
            if (notifacation == ((int)Notification.email))
            {
                return new EmailNotificationService();
            }
            else
                return new TelegramNotificationService();
        }
    }
}
