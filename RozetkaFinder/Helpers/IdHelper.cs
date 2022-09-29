using System.Security.Cryptography;
using System.Text;

namespace RozetkaFinder.Helpers
{
    public interface IIdHelper
    {
        Task<byte[]> ConfigIdHashAsync();
    }
    public class IdHelper : IIdHelper
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
