using DataBasePomelo.Interface;

namespace ErrorBot.Serices
{
    public class MessagePollingBackgroundService : BackgroundService
    {
        private readonly IMessageUpdate _messageUpdate;
        private readonly EventAggregator _eventAggregator;
        private readonly ILogger<MessagePollingBackgroundService> _logger;

        public MessagePollingBackgroundService(
            IMessageUpdate messageUpdate,
            EventAggregator eventAggregator,
            ILogger<MessagePollingBackgroundService> logger)
        {
            _messageUpdate = messageUpdate;
            _eventAggregator = eventAggregator;
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
                        await _eventAggregator.PublishMessage(lastMessage, stoppingToken);
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
