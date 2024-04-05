using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CleanIsClean.Domain.Interfaces;
using CleanIsClean.Infrastructure.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace CleanIsClean.Application.Services;

public class AuthenticationService(IRepository<User> userRepository, IConfiguration configuration)
{
    private readonly IRepository<User> _userRepository = userRepository;
    private readonly IConfiguration _configuration = configuration;
    public async Task<string?> Login(string email, string password)
    {
        IEnumerable<User?> users = await _userRepository.Get(p => p.Email == email && p.Password == password);
        var user = users.FirstOrDefault();

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
                //new Claim(ClaimTypes.Role, "Admin"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            ]),
            Expires = DateTime.UtcNow.AddMinutes(5),
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
}