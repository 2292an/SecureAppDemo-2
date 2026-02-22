namespace SecureAppDemo.Services;

using SecureAppDemo.Dtos;

public interface IAuthorizationService
{
    Task<AuthorizationResponseDto> AuthorizeAsync(AuthorizationRequestDto request);
}
