using Microsoft.AspNetCore.Mvc;
using RozetkaFinder.Models;
using RozetkaFinder.Models.GoodObjects;
using RozetkaFinder.Models.User;
using RozetkaFinder.Repository;
using RozetkaFinder.Services.GoodsServices;
using RozetkaFinder.Services.JSONServices;
using RozetkaFinder.Services.Notification;

namespace RozetkaFinder.Services.MonitoringService
{
    public class MonitoringService : BackgroundService
    {
        private readonly ILogger<MonitoringService> _logger;
        private readonly IServiceProvider _provider;
        public MonitoringService(ILogger<MonitoringService> logger, IServiceProvider provider)
        {
            _provider = provider;
            _logger = logger;
        }
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            return base.StartAsync(cancellationToken);
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("From Monitoring: RUNNING {datatime}", DateTime.Now);
           
            while (!stoppingToken.IsCancellationRequested)
            {
                await DoWork(stoppingToken);
            }
        }

        private async Task DoWork(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"From Monitoring: WORKING {DateTime.Now}");
            using (var scope = _provider.CreateScope())
            {
                var _goodService = scope.ServiceProvider.GetRequiredService<IGoodsService>();
                var _goodRepositary = scope.ServiceProvider.GetRequiredService<IRepository<GoodItem>>();
                var _userRepositary = scope.ServiceProvider.GetRequiredService<IRepository<User>>();
                var _notification = scope.ServiceProvider.GetRequiredService<INotificationService>();
                var _jsonService = scope.ServiceProvider.GetRequiredService<IJsonService>();
                var goods = await _goodRepositary.GetAllAsync();

                foreach (var item in goods)
                {
                    if (await _goodService.CheckGoodPrice(item))
                    {
                        _logger.LogInformation("Good is found . . .{id}", item.IdGood);
                        var user = await _userRepositary.ReadAsync(item.UserId);
                        if (user.Notification == "email")
                        {
                            _notification = new EmailNotificationService();
                        }
                        else
                        {
                            _notification = new TelegramNotificationService();
                        }
                        EmailModel emailModel = await _jsonService.GetEmailModelAsync();
                        _notification.Send(item.UserId, emailModel.Email, emailModel.AppPassword, item.Href);
                        await _goodRepositary.DeleteAsync(item);

                    }
                }

                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("From Monitoring: STOPPED {datetime}", DateTime.Now);
            return base.StopAsync(cancellationToken);
        }
    }
}
