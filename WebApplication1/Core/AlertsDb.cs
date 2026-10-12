using Microsoft.EntityFrameworkCore;
using WebApplication1.Model;

namespace WebApplication1.Core
{
    sealed class AlertsDb(DbContextOptions<AlertsDb> options) : DbContext(options)
    {
        public DbSet<Alert> Alerts => Set<Alert>();

        public DbSet<AlertChannel> AlertChannels => Set<AlertChannel>();

        public DbSet<DeliveryAttempt> DeliveryAttempts => Set<DeliveryAttempt>();


        protected override void OnModelCreating(ModelBuilder model)
        {
            model.Entity<Alert>()
                .Property(a => a.Keywords)
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(
                        v, (System.Text.Json.JsonSerializerOptions?)null),
                    v => System.Text.Json.JsonSerializer.Deserialize<List<string>>(
                        v, (System.Text.Json.JsonSerializerOptions?)null) ?? new List<string>());

            model.Entity<Alert>()
                .Property(a => a.Categories)
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(
                        v, (System.Text.Json.JsonSerializerOptions?)null),
                    v => System.Text.Json.JsonSerializer.Deserialize<List<string>>(
                        v, (System.Text.Json.JsonSerializerOptions?)null) ?? new List<string>());

            model.Entity<Alert>()
                .HasMany(a => a.Channels)
                .WithOne()
                .HasForeignKey(c => c.AlertId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
