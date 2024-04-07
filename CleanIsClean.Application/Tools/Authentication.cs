using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;


namespace CleanIsClean.Application.Tools
{
    public class Authentication
    {
        public static async Task<string> GetUserIdFromHttpContext(HttpContext httpContext)
        {
            string? authorizationHeader = httpContext.Request.Headers["Authorization"];
            if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
            {
                throw new Exception("Authorization header is missing or invalid");
            }

            string jwtToken = authorizationHeader["Bearer ".Length..];
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.ReadJwtToken(jwtToken);
            var idClaim = token.Claims.FirstOrDefault(claim => claim.Type == "UserId");

            return await Task.FromResult(idClaim?.Value ?? throw new Exception("User ID claim not found in token"));
        }
    }
}
