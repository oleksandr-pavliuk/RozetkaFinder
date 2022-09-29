namespace RozetkaFinder.Models.Exceptions
{
    public class AuthenticationException : Exception
    {
        public string UserEmail { get; set; }
        public AuthenticationException() { }
        public AuthenticationException(string message) : base(message) { }

        public AuthenticationException(string message, Exception inner) : base(message, inner) { }
        public AuthenticationException(string message, string userEmail) : this(message)
        {
            UserEmail = userEmail;
        }
    }
}
