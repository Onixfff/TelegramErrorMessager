using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.Bot;

namespace Telegram
{

    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;


        public Worker(ILogger<Worker> logger, IOptions<TelegramOptions> telegramOptions)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var botClient = new TelegramBotClient();

            while (!stoppingToken.IsCancellationRequested) 
            {
                await botClient.ReceiveAsync();

                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                }

                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
