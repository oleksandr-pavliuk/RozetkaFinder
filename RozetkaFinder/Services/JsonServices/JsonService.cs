using Newtonsoft.Json;
using System.Net;
using RozetkaFinder.Models.GoodObjects;
using AutoMapper;
using RozetkaFinder.DTOs;
using RozetkaFinder.Models;
using System.IO;
using System.Reflection.Metadata.Ecma335;
using Org.BouncyCastle.Asn1.Mozilla;
using System.Runtime.InteropServices;

namespace RozetkaFinder.Services.JSONServices
{
    public interface IJsonService
    {
        Task<List<GoodDTO>> GetGoodsAsync(string name);
        Task<GoodItem> GetGoodIDAsync(string id);
        Task<JwtSalt> GetJwtSaltAsync();
        Task<EmailModel> GetEmailModelAsync();
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
                foreach(var item in goods)
                {
                    goodItems.Add(_mapper.Map<GoodDTO>(item));
                }
                return goodItems; 
            }
        }

        public async Task<GoodItem> GetGoodIDAsync(string id)
        {
            using (WebClient wc = new WebClient())
            {
                wc.Headers.Add("accept", "application/json, text/plain, */*");
                wc.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/104.0.0.0 Safari/537.36");

                string uri = "https://search.rozetka.com.ua/ua/search/api/v6/?front-type=xl&country=UA&lang=ua&text=" + id.Replace(" ", "+");
                string json = wc.DownloadString(uri);
                Rootobject obj = JsonConvert.DeserializeObject<Rootobject>(json);

                GoodItem good = new GoodItem() 
                {
                    IdGood = obj.data.goods[0].id,
                    Price = Convert.ToInt32(obj.data.goods[0].price),
                    Href = obj.data.goods[0].href
                };
               
                return good;
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
        
    }
}
