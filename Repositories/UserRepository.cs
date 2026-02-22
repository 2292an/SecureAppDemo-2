using Microsoft.EntityFrameworkCore;
using SecureAppDemo.Data.Context;
using SecureAppDemo.Data.Entities;
using SecureAppDemo.Dtos;

namespace SecureAppDemo.Repositories;
public class UserRepository : IUserRepository
{
    private readonly DatabaseContext _context;

    public UserRepository(DatabaseContext context)
    {
        _context = context;
    }

    public Task<List<User>> GetUsersAsync(SearchUsersRequestDto search)
    {
        return _context.Users.Where(u =>
            (string.IsNullOrEmpty(search.Email) || u.Email.Contains(search.Email)) &&
            (string.IsNullOrEmpty(search.Name) || u.FirstName.Contains(search.Name) || u.LastName.Contains(search.Name)) &&
            (!search.ExternalId.HasValue || u.ExternalId == search.ExternalId.Value))
            .ToListAsync();
    }

    public Task<User?> GetUserByUsernameAsync(string username)
    {
        return _context.Users.FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task<User> CreateUserAsync(User user, List<Role> roles)
    {
        var newUserRoles = roles.Select(r => new UserRole { User = user, Role = r }).ToList();
        user.Roles.AddRange(newUserRoles);

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return user;
    }
}