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
            var message = await _dbContext.Errors.OrderByDescending(m => m.Id).Select(e => e.Message).FirstOrDefaultAsync();

            var newMessageId = await _dbContext.Errors.OrderByDescending(m => m.Id).Select(m => m.Id).FirstOrDefaultAsync();
            if (newMessageId > _lastId)
            {
                _lastId = newMessageId;
                await _jsonRead.SaveLastProcessedIdToJsonAsync(_lastId);
            }

            return message ?? string.Empty;
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
