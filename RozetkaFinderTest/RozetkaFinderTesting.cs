using Xunit;
using RozetkaFinder;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Hosting;
using RozetkaFinder.Controllers;
using RozetkaFinder.Services.UserServices;
using RozetkaFinder.Services.TelegramServices;
using RozetkaFinder.Services.GoodsServices;
using RozetkaFinder.Services.MarkdownServices;
using Microsoft.AspNetCore.Mvc;
using RozetkaFinder.Models.User;
using RozetkaFinder.DTOs;
using Moq;
using RozetkaFinderTest.MockData;
using FluentAssertions;
using RozetkaFinder.Repository;
using Microsoft.EntityFrameworkCore;

namespace RozetkaFinderTest
{
    public class RozetkaFinderTesting
    {
        
        Mock<IGoodsService> _goodService = new Mock<IGoodsService>();
        Mock<IUserService> _userService = new Mock<IUserService>();
        Mock<ITelegramService> _telegramService = new Mock<ITelegramService>();
        Mock<IMarkdownService> _markdownService = new Mock<IMarkdownService>();
        protected readonly ApplicationContext _context;
        public RozetkaFinderTesting()
        {
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new ApplicationContext(options);

            _context.Database.EnsureCreated();
        }

        [Fact]
        public void GetAllGoodsAsyncTest_ShouldReturnList()
        {
            string naming = "Asus Zenbook";
            _goodService.Setup(_ => _.GetGoodsByRequestAsync(naming)).ReturnsAsync(GoodDTOMock.GetGoodDTOs);
            var sut = new UserController(_userService.Object, _goodService.Object, _telegramService.Object, _markdownService.Object);

            var result = sut.GetGoodsAsync(naming);

            result.Should().NotBeOfType(typeof(GoodDTO));
        }
        [Fact]
        public void RegisterTest_ShouldSave()
        {
            User user = 
            _userService.Setup(_ => _.Registration(User user))
        }
    }
}