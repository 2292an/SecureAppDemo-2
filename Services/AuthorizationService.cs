namespace SecureAppDemo.Services;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using SecureAppDemo.Data.Entities;
using SecureAppDemo.Dtos;
using SecureAppDemo.Repositories;

public class AuthorizationService : IAuthorizationService
{
    private readonly IAuthorizationRepository authorizationRepository;
    private readonly IConfiguration configuration;

    public AuthorizationService(IAuthorizationRepository authorizationRepository, IConfiguration configuration)
    {
        this.authorizationRepository = authorizationRepository;
        this.configuration = configuration;
    }

    public async Task<AuthorizationResponseDto> AuthorizeAsync(AuthorizationRequestDto request)
    {
        var user = await authorizationRepository.AuthorizeAsync(request.Username, request.Password)
            .ConfigureAwait(false);

        if (user == null)
        {
            return null;
        }

        var token = GenerateJwtToken(user);

        return new AuthorizationResponseDto
        {
            BearerToken = token,
            ExpiresIn = DateTime.UtcNow.AddHours(1)
        };
    }

    private string GenerateJwtToken(User user)
    {
        var jwtSettings = configuration.GetSection("Jwt");
        var secret = jwtSettings["Secret"];
        var issuer = jwtSettings["Issuer"];
        var audience = jwtSettings["Audience"];
        var expirationMinutes = int.Parse(jwtSettings["ExpirationMinutes"] ?? "60");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.Username),
            new("externalId", user.ExternalId.ToString()),
        };

        claims.AddRange([.. user.Roles.Select(r => new Claim(ClaimTypes.Role, r.Role.Name))]);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
