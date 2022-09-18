using System.Security.Cryptography;
using System.Text;

namespace RozetkaFinder.Services.PasswordServices
{
    public interface IPasswordService
    {
        Task<(byte[], byte[])> CreatePasswordHashAsync(string password);
        Task<bool> AuthenticationPasswordHashAsync(string password, byte[] passwordHash, byte[] passwordSalt);
    }
    public class PasswordService : IPasswordService
    {
        public async Task<(byte[], byte[])> CreatePasswordHashAsync(string password)
        {
            using (var hmac = new HMACSHA512())
            {
                byte[] passwordSalt = hmac.Key;
                byte[] passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return (passwordHash, passwordSalt);
            }
        }

        public async Task<bool> AuthenticationPasswordHashAsync(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var hash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return hash.SequenceEqual(passwordHash);
            }
        }

    }
    
}
