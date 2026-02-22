using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SecureAppDemo.Data.Entities;

[Table(nameof(Role))]
public class Role
{
    public int Id { get; set; }
    [MaxLength(100)]
    public string Name { get; set; }

    public List<UserRole> UserRoles { get; set; } = [];
}