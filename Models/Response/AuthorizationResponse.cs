namespace SecureAppDemo.Models.Response;

public class AuthorizationResponse
{
    public string BearerToken { get; set; }
    public DateTime ExpiresIn { get; set; }
}
