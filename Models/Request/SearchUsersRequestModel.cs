namespace SecureAppDemo.Models.Request;

using System.ComponentModel.DataAnnotations;

public class SearchUsersRequestModel
{
    [MaxLength(200)]
    public string? Email { get; set; }
    public Guid? ExternalId { get; set; }
    [MaxLength(100)]
    public string? Name { get; set; }
}