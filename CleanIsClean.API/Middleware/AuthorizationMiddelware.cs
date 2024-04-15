using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Text.Json;

namespace CleanIsClean.API.Middleware
{
    public class AuthorizationMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        private readonly RequestDelegate _next = next;
        private readonly IConfiguration _configuration = configuration;
        private readonly List<string> _bypassEndpoints = configuration.GetSection("BypassAuthorizationEndpoints").Get<List<string>>() ?? new List<string>();

        public async Task Invoke(HttpContext context)
        {
            var requestPath = context.Request.Path.Value?.ToLowerInvariant();

            // Check if the requested path matches any of the bypass endpoints
            foreach (var endpoint in _bypassEndpoints)
            {
                if (string.Equals(requestPath, endpoint, StringComparison.InvariantCultureIgnoreCase))
                {
                    // Allow the request to proceed to the next middleware or endpoint
                    await _next(context);
                    return;
                }
            }

            // Check for the Authorization header
            if (!context.Request.Headers.ContainsKey("Authorization"))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await HandleRequestError(HttpStatusCode.Unauthorized, context, "You don't have permission to access this feature.");
                return;
            }

            // Extract the JWT token from the Authorization header
            var authorizationHeader = context.Request.Headers.Authorization.ToString();
            var jwtToken = authorizationHeader.Replace("Bearer ", "");
            JwtSecurityToken? token = null;
            // Parse the JWT token 
            // and check if it is valid and not expired
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                token = tokenHandler.ReadJwtToken(jwtToken);

            }
            catch (Exception)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await HandleRequestError(HttpStatusCode.Unauthorized, context, "Invalid token. Please provide a valid token.");
                return;
            }

            // Check if the token is expired
            if (token.ValidTo < DateTime.UtcNow)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await HandleRequestError(HttpStatusCode.Unauthorized, context, "The token has expired. Please obtain a new token.");
                return;
            }

            // Allow the request to proceed to the next middleware or endpoint
            await _next(context);
        }
        private static async Task HandleRequestError(HttpStatusCode code, HttpContext context, string message)
        {
            var result = JsonSerializer.Serialize(new { error = message });
            context.Response.ContentType = "application/json";
            context.Response.Headers.Append("Access-Control-Allow-Origin", "*");
            context.Response.StatusCode = (int)code;
            await context.Response.WriteAsync(result);
        }
    }
}
