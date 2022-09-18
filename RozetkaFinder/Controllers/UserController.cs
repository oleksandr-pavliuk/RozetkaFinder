using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RozetkaFinder.DTOs;
using RozetkaFinder.Models.User;
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
        public async Task<ActionResult<TokenDTO>> RegisterAsync(UserRegisterDTO request) => await _userService.Create(request);

        [HttpPost("all")]
        [Authorize(Roles="admin")]
        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _userService.GetAll();
        }
    }
}

