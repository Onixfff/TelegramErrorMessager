namespace ErrorBot.Interface
{
    public interface IMessageNotifier
    {
        Task NotifyAsync(string message, CancellationToken cancellationToken);
    }
}
