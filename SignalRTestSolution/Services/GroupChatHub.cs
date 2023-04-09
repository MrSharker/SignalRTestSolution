using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace SignalRTestSolution.Services
{
    [Authorize]
    public class GroupChatHub : Hub
    {
        public async Task Enter(string groupName)
        {
            if (Context.UserIdentifier is string userName)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
                await Clients.All.SendAsync("Notify", $"{userName} enter to the group {groupName}");
            }
        }

        public async Task Send(string message, string groupName)
        {
            if (Context.UserIdentifier is string userName)
                await Clients.Group(groupName).SendAsync("ReceiveGroup", message, userName, groupName);
        }
    }
}
