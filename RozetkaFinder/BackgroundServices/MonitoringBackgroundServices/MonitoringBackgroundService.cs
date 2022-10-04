using RozetkaFinder.Helpers.NotificationCreateHelper;
using RozetkaFinder.Models;
using RozetkaFinder.Services.GoodsServices;
using RozetkaFinder.Services.JSONServices;
using RozetkaFinder.Services.Notification;
using RozetkaFinder.Services.UserServices;

namespace RozetkaFinder.Services.MonitoringService
{
    public class MonitoringBackgroundService : BackgroundService
    {
        private readonly ILogger<MonitoringBackgroundService> _logger;
        private readonly IServiceProvider _provider;
        public MonitoringBackgroundService(ILogger<MonitoringBackgroundService> logger, IServiceProvider provider)
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
                var _userService = scope.ServiceProvider.GetRequiredService<IUserService>();
                var _notification = scope.ServiceProvider.GetRequiredService<INotificationService>();
                var _jsonService = scope.ServiceProvider.GetRequiredService<IJsonService>();
                var goods = await _goodService.GetAllGoods();

                foreach (var item in goods)
                {
                    if (await _goodService.CheckGoodPrice(item))
                    {
                        _logger.LogInformation("Good is found . . .{id}", item.IdGood);

                        var user = _userService.GetUser(item.UserId);

                        _notification = new NotifaicationCreator().CreateNotificationService(user.Notification.ToString());

                        EmailModel emailModel = await _jsonService.GetEmailModelAsync();
                        _notification.Send(item.UserId, emailModel.Email, emailModel.AppPassword, item.Href);
                        _goodService.DeleteGoodAsync(item);

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
