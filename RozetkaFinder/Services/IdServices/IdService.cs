using System.Security.Cryptography;
using System.Text;

namespace RozetkaFinder.Services.IdServices
{
    public interface IIdService
    {
        public byte[] ConfigIdHash();
    }
    public class IdService : IIdService
    {
        public byte[] ConfigIdHash()
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
