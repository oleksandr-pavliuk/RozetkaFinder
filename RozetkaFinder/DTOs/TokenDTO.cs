using RozetkaFinder.Services.Security.RefreshToken;

namespace RozetkaFinder.DTOs
{
    public class TokenDTO
    {
        public string Refresh { get; set; }
        public string JwtToken { get; set; }
    }
}
