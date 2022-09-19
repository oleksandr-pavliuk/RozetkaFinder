namespace RozetkaFinder.Models.Exceptions.ValidationException
{
    public class PasswordFormatException : Exception
    {
        public string Password { get; set; }
        public PasswordFormatException() { }
        public PasswordFormatException(string message) : base(message) { }

        public PasswordFormatException(string message, Exception inner) : base(message, inner) { }
        public PasswordFormatException(string message, string password) : this(message)
        {
            Password = password;
        }
    }
}
