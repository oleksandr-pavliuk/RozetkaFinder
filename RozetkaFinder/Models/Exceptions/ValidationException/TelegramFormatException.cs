namespace RozetkaFinder.Models.Exceptions.ValidationException
{
    public class TelegramFormatException : Exception
    {
        public string Telegram { get; set; }
        public TelegramFormatException() { }
        public TelegramFormatException(string message) : base(message) { }

        public TelegramFormatException(string message, Exception inner) : base(message, inner) { }
        public TelegramFormatException(string message, string telegram) : this(message)
        {
            Telegram = telegram;
        }
    }
}
