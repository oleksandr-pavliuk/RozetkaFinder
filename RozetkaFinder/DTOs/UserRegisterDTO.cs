namespace RozetkaFinder.DTOs
{
    public class UserRegisterDTO
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public Roles Role { get; set; }
        public string Email { get; set; }
        public string Telegram { get; set; }
        public string Password { get; set; }
    }
}
