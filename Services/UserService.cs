using SecureAppDemo.Data.Context;
using SecureAppDemo.Data.Entities;
using SecureAppDemo.Dtos;
using SecureAppDemo.Repositories;
using Microsoft.EntityFrameworkCore;
using SecureAppDemo.Exceptions;

namespace SecureAppDemo.Services;
public class UserService : IUserService
{
    private readonly IUserRepository repository;
    private readonly DatabaseContext context;

    public UserService(IUserRepository repository, DatabaseContext context)
    {
        this.repository = repository;
        this.context = context;
    }

    public Task<List<User>> GetUsersAsync(SearchUsersRequestDto search)
    {
        return repository.GetUsersAsync(search);
    }

    public Task<User?> GetUserByUsernameAsync(string username)
    {
        return repository.GetUserByUsernameAsync(username);
    }

    public async Task<User> CreateUserAsync(CreateUserDto dto, List<string> roleNames)
    {
        var user = new User
        {
            Username = dto.Username,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            ExternalId = Guid.NewGuid(),
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
        };

        // Validate that all roles exist
        var roles = await context.Roles
            .Where(r => roleNames.Contains(r.Name))
            .ToListAsync();

        if (roles.Count != roleNames.Count)
        {
            throw new BadRequestResponseException("One or more roles do not exist");
        }

         var existingUser = await repository.GetUserByUsernameAsync(user.Username).ConfigureAwait(false);
        if (existingUser != null)
        {
            throw new BadRequestResponseException("Username already exists");
        }

        return await repository.CreateUserAsync(user, roles);
    }
}