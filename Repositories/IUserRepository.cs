using Microsoft.EntityFrameworkCore;
using SecureAppDemo.Data.Context;
using SecureAppDemo.Data.Entities;
using SecureAppDemo.Dtos;

namespace SecureAppDemo.Repositories;
public interface IUserRepository
{
    Task<List<User>> GetUsersAsync(SearchUsersRequestDto search);
    Task<User?> GetUserByUsernameAsync(string username);
    Task<User> CreateUserAsync(User user, List<Role> roles);
}