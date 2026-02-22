namespace SecureAppDemo.Dtos;

public class AuthorizationResponseDto
{
    public string BearerToken { get; set; }
    public DateTime ExpiresIn { get; set; }
}
