using Newtonsoft.Json;
using System.Net;
using RozetkaFinder.Models.GoodObjects;
using AutoMapper;

namespace RozetkaFinder.Services.JSONServices
{
    public interface IJsonService
    {
        Task<List<GoodItem>> GetGoodsAsync(string name);
    }
    public class JsonService : IJsonService
    {
        private readonly IMapper _mapper;
        public JsonService(IMapper mapper)
        {
            _mapper = mapper;
        }
        public async Task<List<GoodItem>> GetGoodsAsync(string name)
        {
            using (WebClient wc = new WebClient())
            {
                wc.Headers.Add("accept", "application/json, text/plain, */*");
                wc.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/104.0.0.0 Safari/537.36");

                string uri = "https://search.rozetka.com.ua/ua/search/api/v6/?front-type=xl&country=UA&lang=ua&page=1&text=" + name.Replace(" ", "+");
                string json = wc.DownloadString(uri);
                Rootobject obj = JsonConvert.DeserializeObject<Rootobject>(json);

                Good[] goods = obj.data.goods;
                List<GoodItem> goodItems = new List<GoodItem>();
                foreach(var item in goods)
                {
                    goodItems.Add(_mapper.Map<GoodItem>(item));
                }
                return goodItems; 
            }
        }
    }
}
