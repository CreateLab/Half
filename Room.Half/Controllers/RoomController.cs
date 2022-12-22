using Microsoft.AspNetCore.Authorization;
using Room.Half.Services;

namespace Room.Half.Controllers;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[ApiController]
[Route("[controller]/[action]")]
public class RoomController : ControllerBase
{
    private readonly IRoomService _roomService;

    /// <inheritdoc />
    public RoomController(IRoomService roomService)
    {
        _roomService = roomService;
    }
    
    [HttpGet]
    public Task CreateRoom(CancellationToken token)
    {
        var identityName = User.Identity!.Name;
        return _roomService.CreateRoom(identityName,token);
    }
   
    [HttpGet]
    public Task JoinRoom(Guid roomid,CancellationToken token)
    {
        var identityName = User.Identity!.Name;
        return _roomService.JoinRoom(identityName, roomid,token);
    }
    
    [HttpGet]
    public Task DeleteRoom(Guid roomId,CancellationToken token)
    {
        var identityName = User.Identity!.Name;
        return _roomService.DeleteRoom(identityName,roomId,token);
    }
    
    [HttpGet]
    public Task<IList<Guid>> GetRooms(CancellationToken token)
    {
        var identityName = User.Identity!.Name;
        return _roomService.GetRooms(identityName,token);
    }
    
    
}