using DataBasePomelo.Interface;
using DataBasePomelo.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace DataBasePomelo.Controlls
{
    public class Message : IMessageUpdate
    {
        private readonly ErrorsDbContext _dbContext;
        private int _lastId;
        private readonly string _jsonFilePath = "lastProcessedMessage.json";
        private static readonly SemaphoreSlim _fileSemaphore = new SemaphoreSlim(1, 1); // Механизм синхронизации

        public Message(ErrorsDbContext dbContext)
        {
            _dbContext = dbContext;
            _lastId = await GetLastProcessedIdFromJsonAsync(_jsonFilePath);

        }

        public static async Task<Message> CreateAsync(ErrorsDbContext dbContext)
        {
            var message = new Message(dbContext);
            message._lastId = await message.GetLastProcessedIdFromJsonAsync(message._jsonFilePath);
            return message;
        }

        public async Task<string> GetLastMessageAsync()
        {
            var message = await _dbContext.Errors
                .OrderByDescending(m => m.Id) // Или другой критерий для определения "последнего" сообщения
                .Select(e => e.Message)
                .FirstOrDefaultAsync();

            return message ?? string.Empty;
        }

        public async Task<string> GetMessageAsync(int id)
        {
            var message = await _dbContext.Errors
                .Where(e => e.Id == id)
                .Select(e => e.Message) // Возвращаем поле Message
                .FirstOrDefaultAsync();

            return message ?? string.Empty;
        }

        private async Task<int> GetLastProcessedIdFromJsonAsync(string filePath)
        {
            await _fileSemaphore.WaitAsync(); // Ожидаем разрешение на доступ к ресурсу
            try
            {
                if (File.Exists(filePath))
                {
                    var json = await File.ReadAllTextAsync(filePath);
                    var data = JsonSerializer.Deserialize<LastProcessedMessageData>(json);

                    if (data?.LastMessageId >= 0)
                    {
                        return data?.LastMessageId ?? 0; ;
                    }
                }
                return 0;
            }
            finally
            {
                _fileSemaphore.Release(); // Освобождаем ресурс
            }
        }

        private async Task SaveLastProcessedIdToJsonAsync(int lastMessageId, string filePath)
        {
            await _fileSemaphore.WaitAsync(); // Ожидаем разрешение на доступ к ресурсу
            try
            {
                var data = new LastProcessedMessageData { LastMessageId = lastMessageId };
                var json = JsonSerializer.Serialize(data);
                await File.WriteAllTextAsync(filePath, json);
            }
            finally
            {
                _fileSemaphore.Release(); // Освобождаем ресурс
            }
        }
    }
}
