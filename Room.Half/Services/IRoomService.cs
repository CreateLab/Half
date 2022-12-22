namespace Room.Half.Services;

public interface IRoomService
{
    Task CreateRoom(string? identityName, CancellationToken token);
    Task JoinRoom(string? identityName, Guid roomid, CancellationToken token);
    Task DeleteRoom(string? identityName, Guid roomId, CancellationToken token);
    Task<IList<Guid>> GetRooms(string? identityName, CancellationToken token);
}