using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

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

            // Check for the Authorization header for other paths
            if (!context.Request.Headers.ContainsKey("Authorization"))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("You are not authorized to access this resource. Please provide a valid Authorization.");
                return;
            }

            // Allow the request to proceed to the next middleware or endpoint
            await _next(context);
        }
    }
}
