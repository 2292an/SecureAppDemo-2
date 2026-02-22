using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SecureAppDemo.Data.Entities;

[Table(nameof(User))]
public class User
{
    public int Id { get; set; }
    public Guid ExternalId { get; set; } = Guid.NewGuid();
    [MaxLength(50)]
    public required string Username { get; set; }
    [MaxLength(200)]
    public string? Email { get; set; }
    [MaxLength(100)]
    public required string FirstName { get; set; }
    [MaxLength(100)]
    public required string LastName { get; set; }
    public required string PasswordHash { get; set; }
    public List<UserRole> Roles { get; set; } = [];
}