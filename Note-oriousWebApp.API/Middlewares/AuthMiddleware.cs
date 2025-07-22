using Note_oriousWebApp.API.Helpers;
using System.Data;
using System.Security.Claims;

namespace Note_oriousWebApp.API.Middlewares
{
    public class AuthMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly TokenHelper _tokenHelper;

        public AuthMiddleware(RequestDelegate next, TokenHelper tokenHelper)
        {
            _next = next;
            _tokenHelper = tokenHelper;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Skip Middleware for Public Endpoints
            var path = context.Request.Path.Value?.ToString();
            var method = context.Request.Method; // "GET", "POST", "PUT", "DELETE"

            if ((path == "/api/Auth" && method == "POST") || (path == "/api/Users" && method == "POST"))
            {
                await _next(context);
                return;
            }

            var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer", "").Trim();

            if(string.IsNullOrEmpty(token))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Token is Missing!");
                return;
            }

            try
            {
                var principal = _tokenHelper.VerifyAccessToken(token);
                context.User = principal;

                var role = principal.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Role)?.Value;

                if(string.IsNullOrEmpty(role) || role != "User")
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    await context.Response.WriteAsync("Forbidden: Unathorized User!");
                    return;
                }
            }
            catch
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Invalid Credentials!");
                return;

            }

            await _next(context);

        }

    }
}
