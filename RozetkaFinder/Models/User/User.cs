namespace RozetkaFinder.Models.User
{
    public class User
    {
        //---------------- INFO --------------------
        public int Id { get; set; }
        public byte[] IdHash { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public Roles Role { get; set; }
        public string Email { get; set; }
        public string Telegram { get; set; }
        public Notification Notification { get; set; }

        // -------------- Security -----------------
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string RefreshToken { get; set; }
        public DateTime TokenCreated { get; set; }
        public DateTime TokenExpires { get; set; }

    }
}
