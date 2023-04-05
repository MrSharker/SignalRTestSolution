using Microsoft.AspNetCore.Http.Connections;
using SignalRTestSolution;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddSignalR(hubOptions =>
    {
        hubOptions.EnableDetailedErrors = true;
        hubOptions.KeepAliveInterval = TimeSpan.FromMinutes(1);
    })
    .AddHubOptions<ChatHub>(options =>
    {
        options.EnableDetailedErrors = false;
        options.KeepAliveInterval = TimeSpan.FromMinutes(5);
    });

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapHub<ChatHub>("/chat",
    options => {
        options.ApplicationMaxBufferSize = 128;
        options.TransportMaxBufferSize = 128;
        options.LongPolling.PollTimeout = TimeSpan.FromMinutes(1);
        options.Transports = HttpTransportType.LongPolling | HttpTransportType.WebSockets;
    });

app.Run();