using RozetkaFinder.Helpers.Constants;
using RozetkaFinder.Services.JSONServices;
using RozetkaFinder.Services.UserServices;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace RozetkaFinder.Services.TelegramServices
{
    public interface ITelegramService
    {
        void Send(string link, long chatId);
        void Start();
    }
    public class TelegramService : ITelegramService
    {
        private readonly IServiceProvider _provider;
        private string token;
        private ITelegramBotClient bot;
        private Dictionary<int, string> updates = new Dictionary<int, string>()
        {
            [0] = "/start",
            [1] = "@gmail.com"
         };

        public TelegramService(IServiceProvider provider)
        {
            _provider = provider;

            using (var scope = _provider.CreateScope())
            {
                var _jsonService = scope.ServiceProvider.GetRequiredService<IJsonService>();
                token = _jsonService.GetTelegramTokenAsync().Result.Token;
            }
               
           
            bot = new TelegramBotClient(token); 
        }

        public void Start()
        {
            var cts = new CancellationTokenSource();
            var cancellationToken = cts.Token;
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = { },
            };
            bot.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                receiverOptions,
                cancellationToken
            );
        }

        private async Task HandleErrorAsync(ITelegramBotClient botClient, Exception ex, CancellationToken cancellationToken)
        {
            throw ex;
        }

        private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            
            using (var scope = _provider.CreateScope())
            {
                var _userService = scope.ServiceProvider.GetRequiredService<IUserService>();
                switch (update.Message.Text)
                {
                    case "/start":
                        await bot.SendTextMessageAsync(update.Message.Chat.Id, "Write me your email please . . .");
                        break;


                    case string s when s.Contains(updates[1]):
                        RozetkaFinder.Models.User.User user = _userService.GetUser(update.Message.Text);
                        if (user == null) await bot.SendTextMessageAsync(update.Message.Chat.Id, Constants.gettingEmailBot);
                        else
                        {
                            user.TelegramChatId = update.Message.Chat.Id;
                            _userService.Update(user);
                            await bot.SendTextMessageAsync(update.Message.Chat.Id, Constants.mainTextBot);
                        }
                        break;
                    default: 
                        await bot.SendTextMessageAsync(update.Message.Chat.Id, Constants.defaultBot);
                        break;
                }
            }
        }

        public async void Send(string link, long chatId)
        {
            await bot.SendTextMessageAsync(chatId,Constants.visitBot);
            await bot.SendTextMessageAsync(chatId, link);
        }
    }
}
