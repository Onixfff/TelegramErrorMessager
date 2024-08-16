using DataBasePomelo.Interface;
using Microsoft.EntityFrameworkCore;

namespace DataBasePomelo.Services
{
    public class Message : IMessageUpdate
    {
        private readonly ErrorsDbContext _dbContext;
        private readonly IJsonRead _jsonRead;
        private int _lastId;

        public Message(ErrorsDbContext dbContext, IJsonRead jsonRead)
        {
            _dbContext = dbContext;
            _jsonRead = jsonRead;
            _lastId = _jsonRead.GetLastProcessedIdFromJsonAsync().Result;
        }

        public async Task<string> GetLastMessageAsync()
        {
            var result = await _dbContext.Errors
                .OrderByDescending(m => m.Id)
                .Select(e => new { e.Id, e.Message })
                .FirstOrDefaultAsync();

            if (result != null && result.Id > _lastId)
            {
                _lastId = result.Id;
                await _jsonRead.SaveLastProcessedIdToJsonAsync(_lastId);
                return result.Message;
            }

            return string.Empty;
        }

        public async Task<string> GetMessageAsync(int id)
        {
            var message = await _dbContext.Errors
                .Where(e => e.Id == id)
                .Select(e => e.Message)
                .FirstOrDefaultAsync();

            return message ?? string.Empty;
        }
    }
}
