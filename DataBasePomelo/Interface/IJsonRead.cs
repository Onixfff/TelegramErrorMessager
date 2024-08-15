namespace DataBasePomelo.Interface
{
    public interface IJsonRead
    {
        public Task<int> GetLastProcessedIdFromJsonAsync();
        public Task SaveLastProcessedIdToJsonAsync(int lastMessageId);
    }
}