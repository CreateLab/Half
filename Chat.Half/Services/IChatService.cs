using Chat.Half.Models;

namespace Chat.Half.Services;

public interface IChatService
{
    Task SendMessage(string? identityName, MessageDto message, Guid room, CancellationToken cancellationToken);
    Task<IEnumerable<MessageResult>> ReceiveMessage(string? identityName, Guid room, ulong? lastMessageid,
        CancellationToken cancellationToken);

    Task<IEnumerable<MessageResult>> GetPreviousMessages(string? identityName, Guid room, ulong lastMessageid, CancellationToken cancellationToken);
}