using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;
using Telegram.Bot.Polling;

namespace TelegramBotNotification
{
    public class BotService
    {
        
        string token = "";

        ITelegramBotClient bot;

        public BotService()
        {
            var jsonDes = new JsonService();
            token = jsonDes.Token.Token;
            bot = new TelegramBotClient(token);
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
            Console.ReadLine();
            bot.CloseAsync();
        } 
        async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var chatId = update.Message.Chat.Id;
            
            if (update.Message.Text == "/start")
                await botClient.SendTextMessageAsync(chatId, "Hello");

        }
        async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
        }
    }
}
