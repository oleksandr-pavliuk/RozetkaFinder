using RozetkaFinder.Helpers.NotificationCreateHelper;
using RozetkaFinder.Services;
using RozetkaFinder.Services.GoodsServices;
using RozetkaFinder.Services.JSONServices;
using RozetkaFinder.Services.MarkdownServices;
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
                var _markdownService = scope.ServiceProvider.GetRequiredService<IMarkdownService>();
                var _userService = scope.ServiceProvider.GetRequiredService<IUserService>();
                var _jsonService = scope.ServiceProvider.GetRequiredService<IJsonService>();
                var _notCreator = scope.ServiceProvider.GetRequiredService<INotificationCreator>();
                var goods = await _goodService.GetAllGoodsAsync();
                var markdowns = await _markdownService.GetAllMarkdownsAsync();

                foreach (var item in goods)
                {
                    if (await _goodService.CheckGoodPriceAsync(item))
                    {
                        _logger.LogInformation("Good is found . . .{id}", item.IdGood);

                        var user = _userService.GetUser(item.UserEmail);

                        INotificationService _notification = _notCreator.CreateNotificationService(user.Notification.ToString());


                        _notification.Send(item.UserEmail, item.Href);
                        _goodService.DeleteGoodAsync(item);

                    }
                }

                foreach(var item in markdowns)
                {
                    if(await _markdownService.CheckMarkdownCountAsync(item))
                    {
                        _logger.LogInformation("Good is found . . .{id}", item.Naming);

                        var user = _userService.GetUser(item.UserEmail);

                        INotificationService _notification = _notCreator.CreateNotificationService(user.Notification.ToString());


                        _notification.Send(item.UserEmail, item.Href);
                        _markdownService.DeleteMarkdownAsync(item);
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
