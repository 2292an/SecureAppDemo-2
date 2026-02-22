using System.ComponentModel.DataAnnotations.Schema;

namespace SecureAppDemo.Data.Entities;

[Table(nameof(UserRole))]
public class UserRole
{
    public int UserId { get; set; }
    public required User User { get; set; }

    public int RoleId { get; set; }
    public required Role Role { get; set; }
}