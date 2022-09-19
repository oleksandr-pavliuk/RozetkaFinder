namespace RozetkaFinder.Models.Exceptions.ValidationException
{
    public class EmailFormatException : Exception
    {
        public string Email { get; set; }
        public EmailFormatException() { }
        public EmailFormatException(string message) : base(message) { }

        public EmailFormatException(string message, Exception inner) : base(message, inner) { }
        public EmailFormatException(string message, string email) : this(message)
        {
            Email = email;
        }

    }
}
