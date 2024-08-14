namespace DataBasePomelo.Interface
{
    public interface IMessageUpdate
    {
        Task<string> GetLastMessageAsync();
        Task<string> GetMessageAsync(int id);
    }
}
