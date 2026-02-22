using System.ComponentModel.DataAnnotations;

namespace SecureAppDemo.Models.Request;

public class CreateUserRequestModel
{
    [Required]
    [MaxLength(50)]
    public required string Username { get; set; }

    [Required]
    [EmailAddress]
    [MaxLength(200)]
    public required string Email { get; set; }

    [Required]
    [MaxLength(100)]
    public required string FirstName { get; set; }

    [Required]
    [MaxLength(100)]
    public required string LastName { get; set; }

    [Required]
    [MinLength(8)]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$")]
    public required string Password { get; set; }

    [Required]
    [MinLength(1)]
    public required List<string> RoleNames { get; set; }
}
