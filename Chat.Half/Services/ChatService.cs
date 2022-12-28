using Chat.Half.Models;
using Db.Half.Models;
using Microsoft.EntityFrameworkCore;

namespace Chat.Half.Services;

class ChatService : IChatService
{
    private readonly IDbContextFactory<HalfContext> _contextFactory;

    public ChatService(IDbContextFactory<HalfContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    /// <inheritdoc />
    public async Task SendMessage(string? identityName, MessageDto messageDto, Guid room,
        CancellationToken cancellationToken)
    {
        await using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
        var user = await context.Users.FirstOrDefaultAsync(u => u.Login == identityName,
            cancellationToken: cancellationToken);
        if (user == null)
        {
            throw new InvalidOperationException("User not found");
        }

        var chatRoom = await context.Rooms.FirstOrDefaultAsync(r => r.Id == room, cancellationToken: cancellationToken);
        if (chatRoom == null)
        {
            throw new InvalidOperationException("Chat room not found");
        }

        if (!await context.UserRooms.AnyAsync(x => x.Room.Id == room && x.User.Login == identityName,
                cancellationToken: cancellationToken))
        {
            throw new InvalidOperationException("No room access");
        }

        var message = new Message
        {
            Room = chatRoom,
            Owner = user,
            Text = messageDto.Text,
            Created = DateTime.UtcNow.ToUniversalTime()
        };

        context.Messages.Add(message);
        await context.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<MessageResult>> ReceiveMessage(string? identityName, Guid room, ulong? lastMessageId,
        CancellationToken cancellationToken)
    {
        await using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
        if (lastMessageId == null)
        {
            return await context.Messages
                .Where(m => m.Room.Id == room)
                .OrderBy(m => m.Created)
                .TakeLast(100)
                .Select(m => new MessageResult
                {
                    Text = m.Text,
                    Owner = m.Owner.Name,
                    Created = m.Created
                })
                .ToListAsync(cancellationToken);
        }

        return await context.Messages
            .Where(m => m.Room.Id == room)
            .SkipWhile(m => m.Id != lastMessageId)
            .TakeLast(100)
            .Select(m => new MessageResult
            {
                Text = m.Text,
                Owner = m.Owner.Name,
                Created = m.Created
            })
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<MessageResult>> GetPreviousMessages(string? identityName, Guid room, ulong lastMessageid, CancellationToken cancellationToken)
    {
        await using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
        return await context.Messages
            .Where(m => m.Room.Id == room)
            .TakeWhile(m => m.Id != lastMessageid)
            .OrderByDescending(m => m.Created)
            .Take(100)
            .Select(m => new MessageResult
            {
                Text = m.Text,
                Owner = m.Owner.Name,
                Created = m.Created
            })
            .ToListAsync(cancellationToken);
    }
}