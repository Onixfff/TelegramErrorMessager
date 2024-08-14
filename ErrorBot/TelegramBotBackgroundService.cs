using ErrorBot.Options;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Requests;
using Telegram.Bot.Types;

namespace ErrorBot
{

    public class TelegramBotBackgroundService : BackgroundService
    {
        private readonly ILogger<TelegramBotBackgroundService> _logger;
        private readonly TelegramOptions _telegramOptions;

        public TelegramBotBackgroundService(ILogger<TelegramBotBackgroundService> logger,
            IOptionsMonitor<TelegramOptions> telegramOptions)
        {
            _logger = logger;
            _telegramOptions = telegramOptions.CurrentValue;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var botClient = new TelegramBotClient(_telegramOptions.Token);

            ReceiverOptions receiverOptions = new()
            {
                AllowedUpdates = []
            };

            while (!stoppingToken.IsCancellationRequested)
            {
                await botClient.ReceiveAsync(
                    updateHandler: HandlerUpdateAsync,
                    pollingErrorHandler: HandlerPollingErrorAsync,
                    receiverOptions: receiverOptions,
                    cancellationToken: stoppingToken);
            }
        }

        async Task HandlerUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Message is not { } message)
                return;
            if (message.Text is not { } messageText)
                return;

            var chatId = message.Chat.Id;

            Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");

            var sendMessageRequest = new SendMessageRequest(chatId, messageText);

            Message sentMessage = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "Я не могу ответить на это сообщение:\n" + messageText,
                replyToMessageId: message.MessageId,
                cancellationToken: cancellationToken);

        }

        Task HandlerPollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }
    }
}
