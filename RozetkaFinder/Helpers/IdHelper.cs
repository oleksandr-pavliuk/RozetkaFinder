using System.Security.Cryptography;
using System.Text;

namespace RozetkaFinder.Helpers
{
    public interface IIdHelper
    {
        byte[] ConfigIdHashAsync();
    }
    public class IdHelper : IIdHelper
    {
        public byte[] ConfigIdHashAsync()
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
