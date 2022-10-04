using RozetkaFinder.Services.UserServices;
using Telegram.Bots;

namespace RozetkaFinder.Services.TelegramBotServices
{
    public interface ITelegramBotService
    {
        void Send(long chat, string link);
    }
    public class TelegramBotService 
    {
        private readonly IUserService _userService;


        public TelegramBotService(IConfiguration configuration, IUserService userService)
        {
            _userService = userService;

        }

       

    }
}
