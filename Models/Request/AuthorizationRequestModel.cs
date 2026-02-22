namespace SecureAppDemo.Models.Request;

using System.ComponentModel.DataAnnotations;

public class AuthorizationRequestModel
{
    [Required]
    [MaxLength(100)]
    public string Username { get; set; }

    [Required]
    [MaxLength(255)]
    public string Password { get; set; }
}
