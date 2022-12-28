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
        var context = await _contextFactory.CreateDbContextAsync(token);
        var user = await UserExists(identityName, context, token);
        if (user != null)
        {
            var room = new Db.Half.Models.Room
            {
                OwnerLogin = identityName,
            };
            context.Rooms.Add(room);
            var ur = new UserRoom
            {
                User = user,
                Room = room
            };
            context.UserRooms.Add(ur);

            await context.SaveChangesAsync(token);
        }
    }

    /// <inheritdoc />
    public async Task JoinRoom(string? identityName, Guid roomid, CancellationToken token)
    {
        var context = await _contextFactory.CreateDbContextAsync(token);
        var user = await UserExists(identityName, context, token);
        if (user != null)
        {
            var room = await context.Rooms.FirstOrDefaultAsync(x => x.Id == roomid, token);
            if (room != null)
            {
                var ur = new UserRoom
                {
                    User = user,
                    Room = room
                };
                context.UserRooms.Add(ur);
                await context.SaveChangesAsync(token);
            }
        }
    }

    /// <inheritdoc />
    public Task DeleteRoom(string? identityName, Guid roomId, CancellationToken token)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public async Task<IList<Guid>> GetRooms(string? identityName, CancellationToken token)
    {
        var context = await _contextFactory.CreateDbContextAsync(token);
        var user = await UserExists(identityName, context, token);
        if (user != null)
        {
            var rooms = await context.UserRooms.Where(x => x.User == user).Select(x => x.Room.Id).ToListAsync(token);
            return rooms;
        }

        return null;
    }

    private async Task<User?> UserExists(string? identityName, CancellationToken token)
    {
        var context = await _contextFactory.CreateDbContextAsync(token);
        var user = await context.Users.FirstOrDefaultAsync(u => u.Login == identityName, token);
        return user;
    }

    private async Task<User?> UserExists(string? identityName, HalfContext context, CancellationToken token)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.Login == identityName, token);
        return user;
    }
}