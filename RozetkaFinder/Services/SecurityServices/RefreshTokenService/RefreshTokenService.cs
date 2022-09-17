using System.Security.Cryptography;

namespace RozetkaFinder.Services.Security.RefreshToken
{
    public interface IRefreshTokenService
    {
        Task<RefreshToken> GenerateRefreshToken();
    }

    public class RefreshTokenService : IRefreshTokenService
    {
        public async Task<RefreshToken> GenerateRefreshToken()
        {
            var refreshToken = new RefreshToken()
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expires = DateTime.Now.AddDays(7),
                Created = DateTime.Now
            };
            return refreshToken;
        }
    }
}
