namespace SecureAppDemo.Repositories;

using Microsoft.EntityFrameworkCore;
using SecureAppDemo.Data.Context;
using SecureAppDemo.Data.Entities;

public class AuthorizationRepository : IAuthorizationRepository
{
    private readonly DatabaseContext databaseContext;

    public AuthorizationRepository(DatabaseContext databaseContext)
    {
        this.databaseContext = databaseContext;
    }

    public async Task<User> AuthorizeAsync(string username, string password)
    {
        var user = await databaseContext.Users
        .Include(u => u.Roles)
        .ThenInclude(ur => ur.Role)
        .FirstOrDefaultAsync(u => u.Username == username)
        .ConfigureAwait(false);

        if (user == null)
        {
            return null;
        }

        if (!VerifyPassword(password, user.PasswordHash))
        {
            return null;
        }

        return user;
    }

    private bool VerifyPassword(string password, string hash)
    {
        return BCrypt.Net.BCrypt.Verify(password, hash); //encriptado el password y lo compara con el hash guardado en la base de datos
    }
}
