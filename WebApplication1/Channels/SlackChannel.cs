using WebApplication1.Model;

namespace WebApplication1.Channels
{
    sealed class SlackChannel(ILogger<SlackChannel> logger) : INotificationChannel
    {
        public string Type => "slack";

        public Task SendAsync(Alert alert, WorldEvent worldEvent, CancellationToken cancellationToken)
        {
            logger.LogInformation("Slack notification for user {UserId}: {Title}", alert.UserId, worldEvent.Title);

            return Task.CompletedTask;
        }
    }
}
