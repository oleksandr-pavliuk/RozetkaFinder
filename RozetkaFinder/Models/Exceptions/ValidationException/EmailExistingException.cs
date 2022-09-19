namespace RozetkaFinder.Models.Exceptions.ValidationException
{
    public class EmailExistingException : Exception
    {
        public string Email { get; set; }
        public EmailExistingException() { }
        public EmailExistingException(string message) : base(message) { }

        public EmailExistingException(string message, Exception inner) : base(message, inner) { }
        public EmailExistingException(string message, string email) : this(message)
        {
            Email = email;
        }
    }
}
