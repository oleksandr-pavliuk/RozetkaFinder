using Newtonsoft.Json;
using System.Net;
using RozetkaFinder.Models.GoodObjects;
using AutoMapper;
using RozetkaFinder.DTOs;
using RozetkaFinder.Models;
using System.Reflection.Metadata;
using RozetkaFinder.Helpers.Constants;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

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

        // Method for getting goods from RozetkaAPI by naming.
        public async Task<List<GoodDTO>> GetGoodsAsync(string name)
        {
            string json = GetJsonString("https://search.rozetka.com.ua/ua/search/api/v6/?front-type=xl&country=UA&lang=ua&page=1&text=" + name.Replace(" ", "+"));

            Rootobject obj = JsonConvert.DeserializeObject<Rootobject>(json);

            Good[] goods = obj.data.goods;

            List<GoodDTO> goodItems = new List<GoodDTO>();

            if (goods == null)
                throw new Exception(Constants.goodWasNotFound);

            foreach (var item in goods)
            {
                goodItems.Add(_mapper.Map<GoodDTO>(item));
            }
            return goodItems;
            
        }

        //Method for getting good from RozetkaAPI by id.
        public async Task<SubscribtionGood> GetGoodIDAsync(string id)
        {
            string json = GetJsonString("https://search.rozetka.com.ua/ua/search/api/v6/?front-type=xl&country=UA&lang=ua&text=" + id.Replace(" ", "+"));
            Rootobject obj = JsonConvert.DeserializeObject<Rootobject>(json);

            SubscribtionGood good = new SubscribtionGood()
            {
                IdGood = obj.data.goods[0].id,
                Price = Convert.ToInt32(obj.data.goods[0].price),
                Href = obj.data.goods[0].href
            };

            return good;
        }

        //Method for getting markdowns from RozetkaAPI by naming.
        public async Task<List<GoodDTO>> GetMarkdownGoods(string naming)
        {
            string json = GetJsonString("https://search.rozetka.com.ua/ua/search/api/v6/?front-type=xl&country=UA&lang=ua&page=1&text=уцінка+" + naming.Replace(" ", "+"));
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

        //Method that helps to get count markdown's count by naming.
        public async Task<int> GetMarkdownCount(string naming)
        {
            string json = GetJsonString("https://search.rozetka.com.ua/ua/search/api/v6/?front-type=xl&country=UA&lang=ua&page=1&text=уцінка+" + naming.Replace(" ", "+"));
            Rootobject obj = JsonConvert.DeserializeObject<Rootobject>(json);

            Good[] goods = obj.data.goods;

            if (goods == null)
                throw new Exception(Constants.markdownWasNotFound);


            return goods.Length;
            
        }

        //Method helps to get json from RozetkaAPI by the our request (link).
        private string GetJsonString(string link)
        {
            using (WebClient wc = new WebClient())
            {
                wc.Headers.Add("accept", "application/json, text/plain, */*");
                wc.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/104.0.0.0 Safari/537.36");

                string json = wc.DownloadString(link);
                return json;
            }
        }

        //Method that helps to get JWT salt from file.
        public async Task<JwtSalt> GetJwtSaltAsync()
        {
            JwtSalt jwtSalt;
            using (StreamReader sr = new StreamReader("./JwtSalt.json"))
            {
                jwtSalt = JsonConvert.DeserializeObject<JwtSalt>(await sr.ReadToEndAsync());
            }
            return jwtSalt;
        }

        //Method that helps to get email model salt from file.
        public async Task<EmailModel> GetEmailModelAsync()
        {
            EmailModel emailModel;
            using (StreamReader sr = new StreamReader("./EmailModel.json"))
            {
                emailModel = JsonConvert.DeserializeObject<EmailModel>(await sr.ReadToEndAsync());
            }
            return emailModel;
        }

        //Method that helps to get telegram bot token from file.
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
