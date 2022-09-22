using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;
using RozetkaFinder.DTOs;
using RozetkaFinder.Models.User;
using RozetkaFinder.Services.GoodsServices;
using RozetkaFinder.Services.UserServices;

namespace RozetkaFinder.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IGoodsService _goodsService;
        public UserController(IUserService userService, IGoodsService goodsService)
        {
            _userService = userService;
            _goodsService = goodsService;
        }

        [HttpPost("register")]
        public async Task<TokenDTO> RegisterAsync(UserRegisterDTO request) => 
            await _userService.Create(request);

        [HttpGet("all")]
        [Authorize(Roles="admin")]
        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _userService.GetAll();
        }

        [HttpPost]
        public async Task<TokenDTO> LoginAsync(UserInDTO request)
        {
            return await _userService.Login(request);
        }

        [HttpGet]
        [Authorize(Roles="admin, customer")]
        public async Task<List<GoodDTO>> GetGoods(string name)
        {
            var user = HttpContext.User.Claims.Where(i => i.Value.Contains('@')).FirstOrDefault();
            Console.Write(user.Value);
            return await _goodsService.GetGoodsByRequestAsync(name);
        }
            

    }
}

