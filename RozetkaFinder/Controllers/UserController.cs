using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Org.BouncyCastle.Asn1.Mozilla;
using Org.BouncyCastle.Crypto;
using RozetkaFinder.DTOs;
using RozetkaFinder.Models.User;
using RozetkaFinder.Services.GoodsServices;
using RozetkaFinder.Services.MarkdownServices;
using RozetkaFinder.Services.TelegramServices;
using RozetkaFinder.Services.UserServices;

namespace RozetkaFinder.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITelegramService _telegramService;
        private readonly IGoodsService _goodService;
        private readonly IMarkdownService _markdownService;
        public UserController(IUserService userService, IGoodsService goodService, ITelegramService telegramService, IMarkdownService markdownService)
        {
            _telegramService = telegramService;
            _goodService = goodService;
            _userService = userService;
            _markdownService = markdownService;
            _telegramService.Start();
        }

        [HttpPost("register")]
        public async Task<TokenDTO> RegisterAsync(UserRegisterDTO request) =>
            await _userService.Registration(request);

        [HttpGet("all")]
        [Authorize(Roles = "admin")]
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
        public string GetTelegramLink()
        {
            return "t.me/roz_not_bot";
        }

        [HttpGet]
        [Authorize]
        public async Task<List<GoodDTO>> GetGoods(string name)
        {
            return await _goodService.GetGoodsByRequestAsync(name);
        }


        [HttpPost]
        [Authorize]
        public async Task<bool> SubscribeGood(string id)
        {
            var user = HttpContext.User.Claims.Where(i => i.Value.Contains('@')).FirstOrDefault(i => i.Value.Contains('@')).Value;
            return await _goodService.SubscribeGoodAsync(id, user);
        }
        [HttpGet]
        [Authorize]
        public async Task<List<GoodDTO>> GetAllMarkdowns(string naming)
        {
            return await _markdownService.GetMarkdownsAsync(naming);
        }

        [HttpPost]
        [Authorize]
        public async Task<bool> SubscribeMarkdown(string naming)
        {
            return await _markdownService.SubscribeMarkdownAsync(naming, HttpContext.User.Claims.Where(i => i.Value.Contains('@')).FirstOrDefault(i => i.Value.Contains('@')).Value);
        }

        [HttpPost]
        [Authorize]
        public async Task<string> ChangePassword(string oldPasswod, string newPassword)
        {
            return await _userService.ChangePassword(HttpContext.User.Claims.Where(i => i.Value.Contains('@')).FirstOrDefault(i => i.Value.Contains('@')).Value, oldPasswod, newPassword);
        }

        [HttpPost]
        [Authorize]
        public async Task<string> ChangeNotificationSetting()
        {
            return await _userService.ChangeNotificationSetting(HttpContext.User.Claims.Where(i => i.Value.Contains('@')).FirstOrDefault(i => i.Value.Contains('@')).Value);
        }
    }
}

