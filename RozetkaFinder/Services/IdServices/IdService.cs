using System.Security.Cryptography;
using System.Text;

namespace RozetkaFinder.Services.IdServices
{
    public interface IIdService
    {
        Task<byte[]> ConfigIdHashAsync();
    }
    public class IdService : IIdService
    {
        public async Task<byte[]> ConfigIdHashAsync()
        {
            byte[] hashId;
            string time = Convert.ToString(DateTime.Now);
            using (var hmac = new HMACSHA512())
            {
                hashId = hmac.ComputeHash(Encoding.UTF8.GetBytes(time));
            }
            return hashId;
        }
    }
}
