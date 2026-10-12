using WebApplication1.Model;

namespace WebApplication1.Channels
{
    interface INotificationChannel
    {
        string Type { get; }

        Task SendAsync(Alert alert, WorldEvent worldEvent, CancellationToken cancellationToken);
    }
}
