using RozetkaFinder.Services.TelegramServices;
using RozetkaFinder.Services.UserServices;

namespace RozetkaFinder.Services.Notification
{
    public class TelegramNotificationService : INotificationService
    {
        private readonly ITelegramService _telegramService;
        private readonly IUserService _userService;
        public TelegramNotificationService(ITelegramService telegramService, IUserService userService)
        {
            _userService = userService;
            _telegramService = telegramService;
        }

        public void Send(string emailTo, string link)
        {
            var telegramTo = _userService.GetUser(emailTo).TelegramChatId;
            _telegramService.Send(link, telegramTo);
        }
    }
}
