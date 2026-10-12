
using Microsoft.EntityFrameworkCore;
using WebApplication1.Core;
using WebApplication1.Model;

namespace WebApplication1.Endpoints
{


    public static class AlertEndpoints
    {
        public static IEndpointRouteBuilder MapAlertEndpoints(
            this IEndpointRouteBuilder app)
        {
            var users = app.MapGroup("/api/users/{userId}/alerts");

            users.MapGet("/", async (string userId, AlertsDb db) =>
                await db.Alerts
                    .Include(a => a.Channels)
                    .Where(a => a.UserId == userId)
                    .ToListAsync());

            users.MapPost("/", async (
                string userId,
                CreateAlertRequest request,
                AlertsDb db) =>
            {
                if (string.IsNullOrWhiteSpace(request.Name) ||
                    request.Keywords.Count == 0 ||
                    request.Channels.Count == 0)
                {
                    return Results.BadRequest(
                        "Name, keywords and channels are required.");
                }

                if (request.Channels.Any(c =>
                    c is not ("email" or "slack")))
                {
                    return Results.BadRequest("Unsupported channel.");
                }

                var alert = new Alert
                {
                    UserId = userId,
                    Name = request.Name,
                    Keywords = request.Keywords,
                    Categories = request.Categories,
                    Enabled = true,
                    Channels = request.Channels
                        .Distinct()
                        .Select(c => new AlertChannel { Type = c })
                        .ToList()
                };

                db.Alerts.Add(alert);
                await db.SaveChangesAsync();

                return Results.Created(
                    $"/api/users/{userId}/alerts/{alert.Id}", alert);
            });

            // Ide kerül a meglévő MapPut és MapDelete is.

            return app;
        }
    }
}
