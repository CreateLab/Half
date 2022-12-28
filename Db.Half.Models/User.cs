using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Db.Half.Models;

public class User
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public string Name { get; set; }
    public string PasswordHash { get; set; }
    public string Login { get; set; }
    
    public IList<UserRoom> Rooms { get; set; }
    
    public IList<Message> Messages { get; set; }
}