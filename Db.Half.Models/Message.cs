using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Db.Half.Models;

public class Message
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public ulong Id { get; set; }

    public Room Room { get; set; }
    
    public User Owner { get; set; }
    
    public string Text { get; set; }
    
    public DateTime Created { get; set; }
}