namespace SecureAppDemo.Models.Response;

public class UserResponse
{
    public required Guid ExternalId { get; set; }
    public required string Username { get; set; }
    public string? Email { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
}