using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;

namespace SignalRTestSolution.Services
{
    [Authorize]
    public class ChatHub : Hub
    {
        public async Task Send(string message, string userName)
        {
            await Clients.All.SendAsync("Receive", message, userName);
        }

        public async Task SendTo(string message, string to)
        {
            if (Context.UserIdentifier is string userName)
            {
                await Clients.Users(to, userName).SendAsync("ReceiveSecret", message, userName, to);
            }
        }

        public async Task SendPresent(string present, string userName)
        {
            await Clients.Caller.SendAsync("Notify", $"You sent {present} to everyone");
            await Clients.Others.SendAsync("ReceivePresent", $"You get {present} from {userName}");
        }

        public override async Task OnConnectedAsync()
        {

            var context = Context.GetHttpContext();
            if (context is not null)
            {
                if (context.Request.Cookies.ContainsKey("name"))
                {
                    if (context.Request.Cookies.TryGetValue("name", out var userName))
                    {
                        Console.WriteLine($"name = {userName}");
                    }
                }
                await Clients.All.SendAsync("Notify", $"{Context.UserIdentifier} entered into the chat");
                Console.WriteLine($"UserAgent = {context.Request.Headers["User-Agent"]}");
                Console.WriteLine($"RemoteIpAddress = {context.Connection?.RemoteIpAddress?.ToString()}");

                await base.OnConnectedAsync();
            }
        }
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await Clients.All.SendAsync("Notify", $"{Context.UserIdentifier} exits the chat");
            await base.OnDisconnectedAsync(exception);
        }
    }
}
