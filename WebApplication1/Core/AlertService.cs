
using Microsoft.EntityFrameworkCore;
using WebApplication1.Channels;
using WebApplication1.Model;

namespace WebApplication1.Core
{

    sealed class AlertService(
        AlertsDb db,
        IEnumerable<INotificationChannel> channels,
        ILogger<AlertService> logger)
    {
        public async Task<int> ProcessAsync(
            WorldEvent worldEvent,
            CancellationToken cancellationToken = default)
        {
            var alerts = await db.Alerts
                .Include(a => a.Channels)
                .Where(a => a.Enabled)
                .ToListAsync(cancellationToken);

            var attempted = 0;

            foreach (var alert in alerts.Where(a => Matches(a, worldEvent)))
            {
                foreach (var configuredChannel in alert.Channels)
                {
                    var delivery = new DeliveryAttempt
                    {
                        AlertId = alert.Id,
                        Channel = configuredChannel.Type,
                        EventTitle = worldEvent.Title,
                        Status = "Pending"
                    };

                    db.DeliveryAttempts.Add(delivery);
                    await db.SaveChangesAsync(cancellationToken);

                    try
                    {
                        var channel = channels.FirstOrDefault(c =>
                            string.Equals(
                                c.Type,
                                configuredChannel.Type,
                                StringComparison.OrdinalIgnoreCase));

                        if (channel is null)
                        {
                            throw new InvalidOperationException(
                                $"Channel '{configuredChannel.Type}' is not registered.");
                        }

                        await channel.SendAsync(
                            alert, worldEvent, cancellationToken);

                        delivery.Status = "Succeeded";
                    }
                    catch (Exception ex)
                    {
                        delivery.Status = "Failed";
                        delivery.Error = ex.Message;

                        logger.LogError(
                            ex,
                            "Delivery failed for alert {AlertId}",
                            alert.Id);
                    }

                    await db.SaveChangesAsync(cancellationToken);
                    attempted++;
                }
            }

            return attempted;
        }

        private static bool Matches(Alert alert, WorldEvent e)
        {
            var text = $"{e.Title} {e.Description}";

            var keywordMatch = alert.Keywords.Any(k =>
                text.Contains(k, StringComparison.OrdinalIgnoreCase));

            var categoryMatch = alert.Categories.Count == 0 ||
                alert.Categories.Contains(
                    e.Category, StringComparer.OrdinalIgnoreCase);

            return keywordMatch && categoryMatch;
        }
    }
}