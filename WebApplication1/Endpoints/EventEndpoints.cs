using WebApplication1.Core;
using WebApplication1.Model;

namespace WebApplication1.Endpoints
{

    public static class EventEndpoints
    {
        public static IEndpointRouteBuilder MapEventEndpoints(
            this IEndpointRouteBuilder app)
        {
            app.MapPost("/api/events", async (
                WorldEvent worldEvent,
                AlertService service) =>
            {
                var count = await service.ProcessAsync(worldEvent);

                return Results.Ok(new
                {
                    notificationsAttempted = count
                });
            });

            return app;
        }
    }
}
