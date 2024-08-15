using DataBasePomelo.Interface;
using ErrorBot.Interface;

namespace ErrorBot.Serices
{
    public class MessagePollingBackgroundSerice : BackgroundService
    {
        private readonly IMessageUpdate _messageUpdate;
        private readonly TelegramBotBackgroundService _telegramService;
        private readonly ILogger<MessagePollingBackgroundSerice> _logger;

        public MessagePollingBackgroundSerice(
            IMessageUpdate messageUpdate,
            TelegramBotBackgroundService telegramService,
            ILogger<MessagePollingBackgroundSerice> logger)
        {
            _messageUpdate = messageUpdate;
            _telegramService = telegramService;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var lastMessage = await _messageUpdate.GetLastMessageAsync();
                    _logger.LogInformation($"Last message: {lastMessage}");

                    if (!string.IsNullOrEmpty(lastMessage))
                    {
                        await _telegramService.SendMessageToAllUsersAsync(lastMessage, stoppingToken);
                    }

                    await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while polling the database.");
                }
            }
        }
    }
}
