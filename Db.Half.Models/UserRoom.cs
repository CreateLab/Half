namespace Db.Half.Models;

public class UserRoom
{
    public Guid UserId { get; set; }
    public Guid RoomId { get; set; }
    public Room Room { get; set; }
    public User User { get; set; }
}