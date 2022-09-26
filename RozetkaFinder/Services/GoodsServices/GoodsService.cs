using AutoMapper;
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
        Task<bool> GetGoodIDAsync(string id, string user);
        Task<bool> CheckGoodPrice(GoodItem good);

    }
    public class GoodsService : IGoodsService
    {
        private readonly IJsonService _jsonService;
        private readonly IMapper _mapper;
        private readonly IRepository<GoodItem> _repository; 
        public GoodsService(IJsonService jsonService, IMapper mapper, IRepository<GoodItem> repository)
        {
            _repository = repository;
            _jsonService = jsonService;
            _mapper = mapper;
        }
        public async Task<List<GoodDTO>> GetGoodsByRequestAsync(string name)
        {
            return await _jsonService.GetGoodsAsync(name);
        }
        public async Task<bool> GetGoodIDAsync(string id, string user)
        {

            var good = await _jsonService.GetGoodIDAsync(id);
            good.UserId = user;
            try
            {
                await _repository.CreateAsync(good);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> CheckGoodPrice(GoodItem good)
        {
            var goodNew = await _jsonService.GetGoodIDAsync(Convert.ToString(good.IdGood));
            if (goodNew.Price < good.Price)
                return true;
            return false;
        }
    }
}
