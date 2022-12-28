namespace Chat.Half.Models;

public class MessageDto
{
    public Guid RoomId { get; set; }
    public string Text { get; set; }
}