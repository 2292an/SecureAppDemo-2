using SecureAppDemo.Data.Entities;
using SecureAppDemo.Dtos;
using SecureAppDemo.Repositories;

namespace SecureAppDemo.Services;
public interface IUserService
{
    Task<List<User>> GetUsersAsync(SearchUsersRequestDto search);
    Task<User?> GetUserByUsernameAsync(string username);
    Task<User> CreateUserAsync(CreateUserDto dto, List<string> roleNames);
}