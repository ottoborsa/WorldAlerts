
using Microsoft.EntityFrameworkCore;
using WebApplication1.Channels;
using WebApplication1.Core;
using WebApplication1.Endpoints;



var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AlertsDb>(options => options.UseSqlite("Data Source=alerts.db"));

builder.Services.AddScoped<AlertService>();
builder.Services.AddScoped<INotificationChannel, EmailChannel>();
builder.Services.AddScoped<INotificationChannel, SlackChannel>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AlertsDb>();
    db.Database.EnsureCreated();
}


app.MapAlertEndpoints();
app.MapEventEndpoints();
app.MapAdminEndpoints();


app.Run();


