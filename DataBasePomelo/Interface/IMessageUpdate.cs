namespace DataBasePomelo.Interface
{
    public interface IMessageUpdate
    {
        public Task<string> GetLastMessageAsync();
        public Task<string> GetMessageAsync(int id);
    }
}
