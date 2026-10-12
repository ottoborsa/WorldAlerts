
using Microsoft.EntityFrameworkCore;
using WebApplication1.Core;


namespace WebApplication1.Endpoints
{


    public static class AdminEndpoints
    {
        public static IEndpointRouteBuilder MapAdminEndpoints(
            this IEndpointRouteBuilder app)
        {
            var admin = app.MapGroup("/api/admin");

            admin.MapGet("/alerts", async (AlertsDb db) =>
                await db.Alerts
                    .Include(a => a.Channels)
                    .OrderBy(a => a.Id)
                    .ToListAsync());

            admin.MapGet("/deliveries", async (AlertsDb db) =>
                await db.DeliveryAttempts
                    .OrderByDescending(d => d.CreatedAt)
                    .Take(200)
                    .ToListAsync());

            return app;
        }
    }
}
