using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using RozetkaFinder.Services.Notification;

namespace RozetkaFinder.Helpers.NotificationCreateHelper
{
    public class NotificationCreator
    {
        private readonly Dictionary<string, INotificationService> notificationDictionary = new Dictionary<string, INotificationService>()
        {
            ["email"] = new EmailNotificationService(),
            ["telegram"] = new TelegramNotificationService()
        };
        public INotificationService CreateNotificationService(string notification)
        {
            return notificationDictionary[notification];
        }
    }
}
