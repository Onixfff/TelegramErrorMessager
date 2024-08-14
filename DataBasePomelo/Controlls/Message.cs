using DataBasePomelo.Interface;
using Microsoft.EntityFrameworkCore;

namespace DataBasePomelo.Controlls
{
    public class Message : IMessageUpdate
    {
        private readonly ErrorsDbContext _dbContext;

        public Message(ErrorsDbContext dbContext)
        {
            _dbContext = dbContext;
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
    }
}
