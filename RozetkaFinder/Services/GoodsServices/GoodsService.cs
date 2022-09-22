using AutoMapper;
using RozetkaFinder.DTOs;
using RozetkaFinder.Models;
using RozetkaFinder.Services.JSONServices;
using System.Collections;

namespace RozetkaFinder.Services.GoodsServices
{
    public interface IGoodsService
    {
        Task<List<GoodDTO>> GetGoodsByRequestAsync(string name);

    }
    public class GoodsService: IGoodsService
    {
        private readonly IJsonService _jsonService;
        private readonly IMapper _mapper;   
        public GoodsService(IJsonService jsonService, IMapper mapper)
        {
            _jsonService = jsonService;
            _mapper = mapper;
        }
        public async Task<List<GoodDTO>> GetGoodsByRequestAsync(string name)
        {
            var response = await _jsonService.GetGoodsAsync(name);
            List<GoodDTO> goods = new List<GoodDTO>();

            foreach(var item in response)
            {
                goods.Add(_mapper.Map<GoodDTO>(item));
            }
            return goods;
        }
    }
}
