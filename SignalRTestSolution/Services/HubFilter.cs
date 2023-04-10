using Microsoft.AspNetCore.SignalR;

namespace SignalRTestSolution.Services
{
    public class HubFilter: IHubFilter
    {
        public async ValueTask<object?> InvokeMethodAsync(
            HubInvocationContext invocationContext,
            Func<HubInvocationContext, ValueTask<object?>> next)
        {
            Console.WriteLine($"Call the {invocationContext.HubMethodName} method by {invocationContext.Hub.Context.UserIdentifier}");
            return await next(invocationContext);
        }

        public Task OnConnectedAsync(HubLifetimeContext context, Func<HubLifetimeContext, Task> next)
        {
            Console.WriteLine("Call the OnConnectedAsync method from HubFilter");
            var contextHub = context.Hub.Context.GetHttpContext();
            if (context is not null)
            {
                if (contextHub.Request.Cookies.ContainsKey("name"))
                {
                    if (contextHub.Request.Cookies.TryGetValue("name", out var userName))
                    {
                        Console.WriteLine($"name = {userName}");
                    }
                }
                Console.WriteLine($"UserAgent = {contextHub.Request.Headers["User-Agent"]}");
                Console.WriteLine($"RemoteIpAddress = {contextHub.Connection?.RemoteIpAddress?.ToString()}");
            }
            return next(context);
        }

        public Task OnDisconnectedAsync(
            HubLifetimeContext context, Exception? exception, Func<HubLifetimeContext, Exception, Task> next)
        {
            Console.WriteLine("Call the OnDisconnectedAsync method from HubFilter");
            return next(context, exception!);
        }
    }
}
