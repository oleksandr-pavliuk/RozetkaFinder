using AutoMapper;
using AutoMapper.Configuration.Conventions;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Org.BouncyCastle.Crypto;
using RozetkaFinder.DTOs;
using RozetkaFinder.Models;
using RozetkaFinder.Models.GoodObjects;
using RozetkaFinder.Repository;
using RozetkaFinder.Services.JSONServices;
using System.Collections;

namespace RozetkaFinder.Services.GoodsServices
{
    public interface IGoodsService
    {
        Task<List<GoodDTO>> GetGoodsByRequestAsync(string name);
        Task<bool> SubscribeGoodAsync(string id, string user);
        Task<bool> CheckGoodPriceAsync(SubscribtionGood good);

        void DeleteGoodAsync(SubscribtionGood good);
        Task<IEnumerable<SubscribtionGood>> GetAllGoodsAsync();
    }
    public class GoodsService : IGoodsService
    {
        private readonly IJsonService _jsonService;
        private readonly IRepository<SubscribtionGood> _repositoryGoods;
        private readonly IRepository<SubscriptionMarkdown> _repositoryMarkdowns;
        public GoodsService(IJsonService jsonService, IMapper mapper, IRepository<SubscribtionGood> repositoryGoods, IRepository<SubscriptionMarkdown> repositoryMarkdowns)
        {
            _repositoryGoods = repositoryGoods;
            _repositoryMarkdowns = repositoryMarkdowns;
            _jsonService = jsonService;
        }

        //Method for getting good from RozetkaAPI by naming.
        public async Task<List<GoodDTO>> GetGoodsByRequestAsync(string name)
        {
            return await _jsonService.GetGoodsAsync(name);
        }

        //Method for subscribing good by id (add in data base).
        public async Task<bool> SubscribeGoodAsync(string id, string email)
        {

            var good = await _jsonService.GetGoodIDAsync(id);
            good.UserEmail = email;
            try
            {
                await _repositoryGoods.CreateAsync(good);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        //Method for checking price from data base and RozetkaAPI.
        public async Task<bool> CheckGoodPriceAsync(SubscribtionGood good)
        {
            var goodNew = await _jsonService.GetGoodIDAsync(Convert.ToString(good.IdGood));
            if (goodNew.Price < good.Price)
                return true;
            return false;
        }

        //Method for deleting good which was used.
        public async void DeleteGoodAsync(SubscribtionGood good)
        {
            await _repositoryGoods.DeleteAsync(good);
        }

        //Method for getting all goods from data base.
        public async Task<IEnumerable<SubscribtionGood>> GetAllGoodsAsync()
        {
            return await _repositoryGoods.GetAllAsync();
        }



        
    }
}
