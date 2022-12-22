using Db.Half.Models;
using Microsoft.EntityFrameworkCore;

namespace Room.Half.Services;

class RoomService : IRoomService
{
    private readonly IDbContextFactory<HalfContext> _contextFactory;

    public RoomService(IDbContextFactory<HalfContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    /// <inheritdoc />
    public async Task CreateRoom(string? identityName, CancellationToken token)
    {
        var user = await UserExists(identityName, token);
        if (user != null)
        {
            var context = await _contextFactory.CreateDbContextAsync(token);
            var room = new Db.Half.Models.Room
            {
                OwnerLogin = identityName
            };
            context.Rooms.Add(room);
            await context.SaveChangesAsync(token);
            var userRoom = new UserRoom
            {
                RoomId = room.Id,
                UserId = user.Id
            };
            context.UserRooms.Add(userRoom);
            await context.SaveChangesAsync(token);
        }
    }

    /// <inheritdoc />
    public Task JoinRoom(string? identityName, Guid roomid, CancellationToken token)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public Task DeleteRoom(string? identityName, Guid roomId, CancellationToken token)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public Task<IList<Guid>> GetRooms(string? identityName, CancellationToken token)
    {
        throw new NotImplementedException();
    }

    private async Task<User?> UserExists(string? identityName, CancellationToken token)
    {
        var context = await _contextFactory.CreateDbContextAsync(token);
        var user = await context.Users.FirstOrDefaultAsync(u => u.Login == identityName, token);
        return user;
    }
}