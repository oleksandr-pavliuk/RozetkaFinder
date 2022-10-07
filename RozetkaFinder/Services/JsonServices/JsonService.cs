using Newtonsoft.Json;
using System.Net;
using RozetkaFinder.Models.GoodObjects;
using AutoMapper;
using RozetkaFinder.DTOs;
using RozetkaFinder.Models;
using System.Reflection.Metadata;
using RozetkaFinder.Helpers.Constants;

namespace RozetkaFinder.Services.JSONServices
{
    public interface IJsonService
    {
        Task<List<GoodDTO>> GetGoodsAsync(string name);
        Task<SubscribtionGood> GetGoodIDAsync(string id);
        Task<JwtSalt> GetJwtSaltAsync();
        Task<EmailModel> GetEmailModelAsync();
        Task<TelegramToken> GetTelegramTokenAsync();
        Task<List<GoodDTO>> GetMarkdownGoods(string naming);
        Task<int> GetMarkdownCount(string naming);
    }
    public class JsonService : IJsonService
    {
        private readonly IMapper _mapper;
        public JsonService(IMapper mapper)
        {
            _mapper = mapper;
        }
        public async Task<List<GoodDTO>> GetGoodsAsync(string name)
        {
            using (WebClient wc = new WebClient())
            {
                wc.Headers.Add("accept", "application/json, text/plain, */*");
                wc.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/104.0.0.0 Safari/537.36");

                string uri = "https://search.rozetka.com.ua/ua/search/api/v6/?front-type=xl&country=UA&lang=ua&page=1&text=" + name.Replace(" ", "+");
                string json = wc.DownloadString(uri);
                Rootobject obj = JsonConvert.DeserializeObject<Rootobject>(json);

                Good[] goods = obj.data.goods;
                
                List<GoodDTO> goodItems = new List<GoodDTO>();

                if (goods == null)
                    throw new Exception(Constants.goodWasNotFound);

                foreach(var item in goods)
                {
                    goodItems.Add(_mapper.Map<GoodDTO>(item));
                }
                return goodItems; 
            }
        }

        public async Task<SubscribtionGood> GetGoodIDAsync(string id)
        {
            using (WebClient wc = new WebClient())
            {
                wc.Headers.Add("accept", "application/json, text/plain, */*");
                wc.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/104.0.0.0 Safari/537.36");

                string uri = "https://search.rozetka.com.ua/ua/search/api/v6/?front-type=xl&country=UA&lang=ua&text=" + id.Replace(" ", "+");
                string json = wc.DownloadString(uri);
                Rootobject obj = JsonConvert.DeserializeObject<Rootobject>(json);

                SubscribtionGood good = new SubscribtionGood() 
                {
                    IdGood = obj.data.goods[0].id,
                    Price = Convert.ToInt32(obj.data.goods[0].price),
                    Href = obj.data.goods[0].href
                };
               
                return good;
            }
        }

        public async Task<List<GoodDTO>> GetMarkdownGoods(string naming)
        {
            using (WebClient wc = new WebClient())
            {
                wc.Headers.Add("accept", "application/json, text/plain, */*");
                wc.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/104.0.0.0 Safari/537.36");

                string uri = "https://search.rozetka.com.ua/ua/search/api/v6/?front-type=xl&country=UA&lang=ua&page=1&text=уцінка+" + naming.Replace(" ", "+");
                string json = wc.DownloadString(uri);
                Rootobject obj = JsonConvert.DeserializeObject<Rootobject>(json);

                Good[] goods = obj.data.goods;

                List<GoodDTO> goodItems = new List<GoodDTO>();

                if (goods == null)
                    throw new Exception(Constants.markdownWasNotFound);

                foreach (var item in goods)
                {
                    goodItems.Add(_mapper.Map<GoodDTO>(item));
                }
                return goodItems;

            }
        }

        public async Task<int> GetMarkdownCount(string naming)
        {
            using (WebClient wc = new WebClient())
            {
                wc.Headers.Add("accept", "application/json, text/plain, */*");
                wc.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/104.0.0.0 Safari/537.36");

                string uri = "https://search.rozetka.com.ua/ua/search/api/v6/?front-type=xl&country=UA&lang=ua&page=1&text=уцінка+" + naming.Replace(" ", "+");
                string json = wc.DownloadString(uri);
                Rootobject obj = JsonConvert.DeserializeObject<Rootobject>(json);

                Good[] goods = obj.data.goods;

                if (goods == null)
                    throw new Exception(Constants.markdownWasNotFound);

                
                return goods.Length;
            }
        }

        public async Task<JwtSalt> GetJwtSaltAsync()
        {
            JwtSalt jwtSalt;
            using (StreamReader sr = new StreamReader("./JwtSalt.json"))
            {
                jwtSalt = JsonConvert.DeserializeObject<JwtSalt>(await sr.ReadToEndAsync());
            }
            return jwtSalt;
        }

        public async Task<EmailModel> GetEmailModelAsync()
        {
            EmailModel emailModel;
            using (StreamReader sr = new StreamReader("./EmailModel.json"))
            {
                emailModel = JsonConvert.DeserializeObject<EmailModel>(await sr.ReadToEndAsync());
            }
            return emailModel;
        }
        public async Task<TelegramToken> GetTelegramTokenAsync()
        {
            TelegramToken token;
            using (StreamReader sr = new StreamReader("./TelegramToken.json"))
            {
                token = JsonConvert.DeserializeObject<TelegramToken>(await sr.ReadToEndAsync());
            }
            return token;
        }



    }
}
