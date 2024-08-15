using DataBasePomelo.Interface;
using DataBasePomelo.Models;
using System.Text.Json;

namespace DataBasePomelo.Services
{
    public class JsonRead : IJsonRead
    {
        private readonly string _jsonFilePath = "lastProcessedMessage.json";
        private static readonly SemaphoreSlim _fileSemaphore = new SemaphoreSlim(1, 1); // Механизм синхронизации

        public async Task<int> GetLastProcessedIdFromJsonAsync()
        {
            await _fileSemaphore.WaitAsync(); // Ожидаем разрешение на доступ к ресурсу
            try
            {
                if (File.Exists(_jsonFilePath))
                {
                    var json = await File.ReadAllTextAsync(_jsonFilePath);
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

        public async Task SaveLastProcessedIdToJsonAsync(int lastMessageId)
        {
            await _fileSemaphore.WaitAsync(); // Ожидаем разрешение на доступ к ресурсу
            try
            {
                var data = new LastProcessedMessageData { LastMessageId = lastMessageId };
                var json = JsonSerializer.Serialize(data);
                await File.WriteAllTextAsync(_jsonFilePath, json);
            }
            finally
            {
                _fileSemaphore.Release(); // Освобождаем ресурс
            }
        }
    }
}
