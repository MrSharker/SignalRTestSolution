using Microsoft.AspNetCore.SignalR;

namespace SignalRTestSolution
{
    public class ChatHub : Hub
    {
        public async Task Send(string message, string userName)
        {
            await this.Clients.All.SendAsync("Receive", message, userName);
        }

        public async Task SendPresent(string present, string userName)
        {
            await this.Clients.Caller.SendAsync("Notify", $"You sent {present} to everyone");
            await this.Clients.Others.SendAsync("ReceivePresent", $"You get {present} from {userName}");
        }

        public override async Task OnConnectedAsync()
        {
            
            var context = Context.GetHttpContext();
            if (context is not null)
            {
                // получаем кук name
                if (context.Request.Cookies.ContainsKey("name"))
                {
                    if (context.Request.Cookies.TryGetValue("name", out var userName))
                    {
                        Console.WriteLine($"name = {userName}");
                    }
                }
                await Clients.All.SendAsync("Notify", $"{Context.ConnectionId} entered into the chat");
                // получаем юзер-агент
                Console.WriteLine($"UserAgent = {context.Request.Headers["User-Agent"]}");
                // получаем ip
                Console.WriteLine($"RemoteIpAddress = {context.Connection?.RemoteIpAddress?.ToString()}");

                await base.OnConnectedAsync();
            }
        }
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await Clients.All.SendAsync("Notify", $"{Context.ConnectionId} exits the chat");
            await base.OnDisconnectedAsync(exception);
        }
    }
}
