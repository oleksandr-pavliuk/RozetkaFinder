using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RozetkaFinder.DTOs;
using RozetkaFinder.Services.GoodsServices;
using RozetkaFinder.Services.MarkdownServices;

namespace RozetkaFinder.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly IMarkdownService _markdownService;
        private readonly IGoodsService _goodService;
        public ItemsController(IMarkdownService markdownService, IGoodsService goodService)
        {
            _markdownService = markdownService;
            _goodService = goodService;
        }

        [HttpGet]
        [Authorize]
        public async Task<List<GoodDTO>> GetAllMarkdownsAsync(string naming)
        {
            return await _markdownService.GetMarkdownsAsync(naming);
        }


        [HttpGet]
        [Authorize]
        public async Task<List<GoodDTO>> GetGoodsAsync(string name)
        {
            return await _goodService.GetGoodsByRequestAsync(name);
        }



    }
}
