namespace Chat.Half.Models;

public class MessageResult
{
    public string Text { get; set; }
    public string User { get; set; }
    public string Owner { get; set; }
    public DateTime Created { get; set; }
}