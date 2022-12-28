using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Db.Half.Models;

public class Room
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    
    public string OwnerLogin { get; set; }

    public IList<UserRoom> Users { get; set; }
    
    public IList<Message> Messages { get; set; }
}