using WebApplication1.Model;

namespace WebApplication1.Channels
{

    // Replace these demo implementations with real integrations.
    sealed class EmailChannel(ILogger<EmailChannel> logger) : INotificationChannel
    {
        public string Type => "email";

        public Task SendAsync(Alert alert, WorldEvent worldEvent, CancellationToken cancellationToken)
        {
            logger.LogInformation("Email to user {UserId}: {Title}", alert.UserId, worldEvent.Title);

            return Task.CompletedTask;
        }
    }
}
