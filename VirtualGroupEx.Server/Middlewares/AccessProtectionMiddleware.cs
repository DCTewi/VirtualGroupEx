using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using VirtualGroupEx.Server.Data;
using VirtualGroupEx.Server.Services;

namespace VirtualGroupEx.Server.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class AccessProtectionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<AccessProtectionMiddleware> logger;
        private readonly AccessProtectionOptions options;

        public AccessProtectionMiddleware(RequestDelegate next,
                                          AccessProtectionOptions options,
                                          ILogger<AccessProtectionMiddleware> logger)
        {
            _next = next;
            this.logger = logger;
            this.options = options;
        }

        public Task Invoke(HttpContext httpContext,
                           SignInManager<User> signInManager,
                           CurrentUserService currentUser)
        {
            var path = httpContext.Request.Path.Value;
            if (path.StartsWith("/_blazor") || path.EndsWith(".css") || path.EndsWith(".js"))
            {
                logger.LogTrace(
                    $"IP[{httpContext.Connection.RemoteIpAddress?.ToString()}] try to access [{path}]");
            }
            else
            {
                logger.LogInformation(
                    $"IP[{httpContext.Connection.RemoteIpAddress?.ToString()}] try to access [{path}]");

                var isSigned = signInManager.IsSignedIn(httpContext.User);

                if (isSigned && currentUser.User == null)
                {
                    httpContext.Response.Redirect(options.LogoutPath);
                    return Task.CompletedTask;
                }

                if (!isSigned || !currentUser.User.Available)
                {
                    if (!options.Whitelist.Contains(path))
                    {
                        logger.LogInformation(
                            $"IP[{httpContext.Connection.RemoteIpAddress?.ToString()}] User[{httpContext?.User?.Identity?.Name}] access denied for [{path}]");

                        httpContext.Response.Redirect(options.AuthPath);

                        return Task.CompletedTask;
                    }
                }
            }

            return _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class AccessProtectionMiddlewareExtensions
    {
        public static IApplicationBuilder UseAccessProtectionMiddleware(this IApplicationBuilder builder,
                                                                        AccessProtectionOptions options)
        {
            return builder.UseMiddleware<AccessProtectionMiddleware>(options);
        }
    }
}
