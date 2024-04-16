using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using CleanIsClean.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using CleanIsClean.Domain.ViewModels;
namespace CleanIsClean.Application.Services;

public class AuthenticationService(IRepository<User> userRepository,IRepository<UserRole> userRoleRepository, IConfiguration configuration) : IAuthenticationService
{
    private readonly IRepository<User> _userRepository = userRepository;
    private readonly IRepository<UserRole> _userRoleRepository = userRoleRepository;
    private readonly IConfiguration _configuration = configuration;
    public async Task<string?> Login(LoginView loginView)
    {
        IEnumerable<User?> users = await _userRepository.Get(p => p.Email == loginView.Email && p.Password == loginView.Password);
        User? user = users.FirstOrDefault();

        if (user == null) return null;
        
        // Take the userRole if it exists
        IEnumerable<UserRole?> userRoles = await _userRoleRepository.Get(p=> p.UserId == user.Id, includeProperties: "Role");

        if (user == null) return null;
        // JWT token generation
        var issuer = _configuration["Jwt:Issuer"];
        var audience = _configuration["Jwt:Audience"];
        byte[] key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"] ?? string.Empty);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
            [
                new Claim("Id", Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("UserId", user.Id.ToString()),
                new Claim(ClaimTypes.Role, userRoles.FirstOrDefault()?.Role?.RoleName ?? ""),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            ]),
            Expires = DateTime.UtcNow.AddMinutes(3),
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = new SigningCredentials(
        new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwtToken = tokenHandler.WriteToken(token);

        return jwtToken;
    }
    public static string GenerateRefreshToken()
    {
        // Generate a random refresh token using a cryptographic random number generator
        byte[] randomNumber = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
        }
        return Convert.ToBase64String(randomNumber);
    }
    
}