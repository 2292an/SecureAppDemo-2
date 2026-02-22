namespace SecureAppDemo.Dtos;

public class SearchUsersRequestDto
{
    public string? Email { get; set; }
    public Guid? ExternalId { get; set; }
    public string? Name { get; set; }
}