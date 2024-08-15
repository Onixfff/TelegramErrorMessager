using ErrorBot.Interface;
using Telegram.Bot;

namespace ErrorBot.Serices
{
    public class MessageNotifierService : IMessageNotifier
    {
        private readonly TelegramBotClient _botClient;
        private readonly List<long> _userChatIds;

        public MessageNotifierService(TelegramBotClient botClient, List<long> userChatIds)
        {
            _botClient = botClient;
            _userChatIds = userChatIds;
        }

        public async Task NotifyAsync(string message, CancellationToken cancellationToken)
        {
            foreach (var chatId in _userChatIds)
            {
                await _botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: message,
                    cancellationToken: cancellationToken);
            }
        }
    }
}
