namespace SecureAppDemo.Repositories;

using SecureAppDemo.Data.Entities;

public interface IAuthorizationRepository
{
    Task<User> AuthorizeAsync(string username, string password);
}
