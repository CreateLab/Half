using Chat.Half.Models;
using Chat.Half.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Half.Controllers;

[Authorize]
[ApiController]
[Route("[controller]/[action]")]
public class ChatController : ControllerBase
{
    private readonly IChatService _chatService;

    public ChatController(IChatService chatService)
    {
        _chatService = chatService;
    }

    [HttpPost]
    public Task SendMessage(MessageDto message,[FromQuery] Guid room, CancellationToken cancellationToken)
    {
        var identityName = User.Identity!.Name;
        return _chatService.SendMessage(identityName,message, room, cancellationToken);
    }

    [HttpGet]
    public Task<IEnumerable<MessageResult>> ReceiveMessage(Guid room, ulong? lastMessageid = null,
        CancellationToken cancellationToken = default)
    {
        var identityName = User.Identity!.Name;
        return _chatService.ReceiveMessage(identityName,room, lastMessageid, cancellationToken);
    }
    
    public Task<IEnumerable<MessageResult>> GetPreviousMessages(Guid room, ulong lastMessageid,
        CancellationToken cancellationToken = default)
    {
        var identityName = User.Identity!.Name;
        return _chatService.GetPreviousMessages(identityName,room, lastMessageid, cancellationToken);
    }
}