using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Org.BouncyCastle.Asn1.Mozilla;
using Org.BouncyCastle.Crypto;
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
        public UserController(IUserService userService)
        {
            _userService = userService;
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
        [Authorize]
        public async Task<List<GoodDTO>> GetGoods(string name)
        {
            return await _userService.SearchGoods(name);
        }
          
        [HttpPost]
        [Authorize]
        public async Task<bool> SubscribeGood(string id)
        {
            var user = HttpContext.User.Claims.Where(i => i.Value.Contains('@')).FirstOrDefault(i => i.Value.Contains('@')).Value;
            return await _userService.SubscribeGood(id, user);
        }
        [HttpPost]
        [Authorize]
        public async Task<string> ChangePassword(string oldPasswod, string newPassword)
        {
            return await _userService.ChangePassword(HttpContext.User.Claims.Where(i => i.Value.Contains('@')).FirstOrDefault(i => i.Value.Contains('@')).Value, oldPasswod, newPassword);
        }
    }
}

