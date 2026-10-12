
using Microsoft.EntityFrameworkCore;
using WebApplication1.Channels;
using WebApplication1.Core;
using WebApplication1.Model;

namespace WebApplication1.Endpoints;

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
            AlertsDb db,
            IEnumerable<INotificationChannel> channels) =>
        {
            if (string.IsNullOrWhiteSpace(request.Name) ||
                request.Keywords is not { Count: > 0 } ||
                request.Categories is null)
            {
                return Results.BadRequest(
                    "Name, keywords and categories are required.");
            }

            if (!TryNormalizeChannels(
                    request.Channels, channels, out var normalizedChannels))
            {
                return Results.BadRequest("Invalid or unsupported channel.");
            }

            var alert = new Alert
            {
                UserId = userId,
                Name = request.Name,
                Keywords = request.Keywords,
                Categories = request.Categories,
                Enabled = true,
                Channels = normalizedChannels
                    .Select(c => new AlertChannel { Type = c })
                    .ToList()
            };

            db.Alerts.Add(alert);
            await db.SaveChangesAsync();

            return Results.Created(
                $"/api/users/{userId}/alerts/{alert.Id}", alert);
        });

        users.MapPut("/{id:int}", async (
            string userId,
            int id,
            UpdateAlertRequest request,
            AlertsDb db,
            IEnumerable<INotificationChannel> channels) =>
        {
            if (string.IsNullOrWhiteSpace(request.Name) ||
                request.Keywords is not { Count: > 0 } ||
                request.Categories is null)
            {
                return Results.BadRequest(
                    "Name, keywords and categories are required.");
            }

            if (!TryNormalizeChannels(
                    request.Channels, channels, out var normalizedChannels))
            {
                return Results.BadRequest("Invalid or unsupported channel.");
            }

            var alert = await db.Alerts
                .Include(a => a.Channels)
                .FirstOrDefaultAsync(a =>
                    a.Id == id && a.UserId == userId);

            if (alert is null)
                return Results.NotFound();

            alert.Name = request.Name;
            alert.Keywords = request.Keywords;
            alert.Categories = request.Categories;
            alert.Enabled = request.Enabled;

            db.AlertChannels.RemoveRange(alert.Channels);
            alert.Channels = normalizedChannels
                .Select(c => new AlertChannel { Type = c })
                .ToList();

            await db.SaveChangesAsync();

            return Results.NoContent();
        });

        users.MapDelete("/{id:int}", async (
            string userId,
            int id,
            AlertsDb db) =>
        {
            var alert = await db.Alerts.FirstOrDefaultAsync(a =>
                a.Id == id && a.UserId == userId);

            if (alert is null)
                return Results.NotFound();

            db.Alerts.Remove(alert);
            await db.SaveChangesAsync();

            return Results.NoContent();
        });

        return app;
    }

    private static bool TryNormalizeChannels(
        IEnumerable<string>? requestedChannels,
        IEnumerable<INotificationChannel> registeredChannels,
        out List<string> normalizedChannels)
    {
        normalizedChannels = [];

        if (requestedChannels is null)
            return false;

        var channelByType = registeredChannels.ToDictionary(
            c => c.Type,
            StringComparer.OrdinalIgnoreCase);

        var requested = requestedChannels
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        if (requested.Count == 0 ||
            requested.Any(c =>
                string.IsNullOrWhiteSpace(c) ||
                !channelByType.ContainsKey(c)))
        {
            return false;
        }

        normalizedChannels = requested
            .Select(c => channelByType[c].Type)
            .ToList();

        return true;
    }
}
