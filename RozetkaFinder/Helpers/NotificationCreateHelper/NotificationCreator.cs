using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using RozetkaFinder.Services.JSONServices;
using RozetkaFinder.Services.Notification;
using RozetkaFinder.Services.TelegramServices;
using RozetkaFinder.Services.UserServices;

namespace RozetkaFinder.Helpers.NotificationCreateHelper
{
    public interface INotificationCreator
    {
        INotificationService CreateNotificationService(string notification);
    }
    public class NotificationCreator : INotificationCreator
    {
        private readonly IServiceProvider _provider;

        public NotificationCreator(IServiceProvider provider)
        {
            _provider = provider;
        }

        private readonly Dictionary<string, Func<IServiceProvider, INotificationService>> notificationDictionary = new Dictionary<string, Func<IServiceProvider, INotificationService>>()
        {
            ["email"] = (IServiceProvider provider) => provider.GetRequiredService<EmailNotificationService>(),
            ["telegram"] = (IServiceProvider provider) => provider.GetRequiredService<TelegramNotificationService>()
        };
        public INotificationService CreateNotificationService(string notification)
        {
            return notificationDictionary[notification](_provider);
        }

        

    }
}
