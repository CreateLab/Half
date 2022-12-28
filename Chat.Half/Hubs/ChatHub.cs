using Chat.Half.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Chat.Half.Hubs;

[Authorize]
public class ChatHub : Hub
{
  
    
    public async Task SendMessage(MessageDto message)
    {
        
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }
    
    public async Task JoinGroup(string user)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, user);
    }
}