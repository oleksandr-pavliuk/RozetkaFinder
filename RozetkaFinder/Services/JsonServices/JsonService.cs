using RozetkaFinder.Models;
using System.Net;

namespace RozetkaFinder.Services.JSONServices
{
    public interface IJsonService
    {
        Task<List<Good>> GetGoodsAsync(string name);
    }
    public class JsonService : IJsonService
    {
        public async Task<List<Good>> GetGoodsAsync(string name)
        {
            List<Good> goods = new List<Good>()
            {
                new Good() {Title ="Zenbook 14 UM", Id = 332135320, Link = "https://rozetka.com.ua/asus-90nb0sm1-m007m0/p332135320/", Price = 29999 },
                new Good() {Title = "Zenbook 14 UX", Id =  351148491, Link = "https://rozetka.com.ua/asus-90nb0si1-m00a60/p351148491/", Price = 46999}
            };

            return goods;
        }
    }
}
