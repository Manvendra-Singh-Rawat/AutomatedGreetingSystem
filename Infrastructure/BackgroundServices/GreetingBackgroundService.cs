using AutomatedGreetingSystem.Application.Interfaces;

namespace AutomatedGreetingSystem.Infrastructure.GreetingService
{
    public class GreetingBackgroundService : BackgroundService
    {
        private readonly ILogger<GreetingBackgroundService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public GreetingBackgroundService(ILogger<GreetingBackgroundService> logger, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                Console.WriteLine("Checking for greetings...");

                using (var scope = _scopeFactory.CreateScope())
                {
                    var greetingService = scope.ServiceProvider.GetService<IGreetingService>();

                    if (greetingService != null)
                        await greetingService.CheckAndSendGreet();
                    else
                        throw new Exception("greeting service is null");
                }

                await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
            }
        }
    }
}
