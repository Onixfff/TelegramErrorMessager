using DataBasePomelo.Interface;

namespace TelegramErrorMessager.Controlls
{
    public class PutMessage
    {
        private readonly CancellationToken _cancellationToken;
        private readonly IMessageUpdate _messageUpdate;

        public PutMessage(CancellationToken cancellationToken,
            IMessageUpdate messageUpdate)
        {
            _cancellationToken = cancellationToken;
            _messageUpdate = messageUpdate;
        }

        public async Task<bool> WhileDo()
        {
            while (true)
            {
                // Проверяем, был ли запрошен сигнал отмены
                if (_cancellationToken.IsCancellationRequested)
                {
                    return false; // Выход из цикла и метода, если отмена была запрошена
                }

                // Вызов метода через инстанс _messageUpdate
                string lastMessage = await _messageUpdate.GetLastMessageAsync();

                // Обработка полученного сообщения (пример)
                if (!string.IsNullOrEmpty(lastMessage))
                {
                    Console.WriteLine($"Last message: {lastMessage}");
                }

                // Задержка перед следующим запросом, чтобы не делать запросы слишком часто
                await Task.Delay(1000, _cancellationToken);
            }
        }
        
    }
}
