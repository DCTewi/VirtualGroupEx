using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using VirtualGroupEx.Server.Data;

namespace VirtualGroupEx.Server.Middlewares
{
    public class PostLoginMiddleware
    {
        private static IDictionary<Guid, PostLoginInfo> LoginRequests { get; set; } =
            new ConcurrentDictionary<Guid, PostLoginInfo>();

        public static Guid CreateLoginRequest(PostLoginInfo info)
        {
            Guid key = Guid.NewGuid();

            LoginRequests[key] = info;
            return key;
        }

        private readonly RequestDelegate _next;
        private readonly PostLoginOptions options;
        private readonly ILogger<PostLoginMiddleware> logger;

        public PostLoginMiddleware(RequestDelegate next,
                                   PostLoginOptions options,
                                   ILogger<PostLoginMiddleware> logger)
        {
            _next = next;
            this.logger = logger;
            this.options = options;
        }

        public async Task Invoke(HttpContext httpContext,
                           SignInManager<User> signInManager)
        {
            if (httpContext.Request.Path == options.PostLoginPath)
            {
                if (httpContext.Request.Query.ContainsKey("key"))
                {
                    var key = Guid.Parse(httpContext.Request.Query["key"]);
                    var info = LoginRequests[key];

                    logger.LogInformation(
                        $"Post login request for [{info.UserName}].");

                    await signInManager.PasswordSignInAsync(
                        info.UserName, info.Password, info.RememberMe, lockoutOnFailure: false);

                    LoginRequests.Remove(key);

                    logger.LogInformation(
                        $"Post login completed for [{info.UserName}].");
                }

                httpContext.Response.Redirect("/");
                return;
            }

            if (httpContext.Request.Path == options.LogOutPath)
            {
                logger.LogInformation(
                    $"IP[{httpContext.Request.Host.Value}] User [{httpContext.User.Identity.Name}] logged out");

                await signInManager.SignOutAsync();
                httpContext.Response.Redirect("/");
                return;
            }

            await _next(httpContext);
        }
    }

    public static class PostLoginMiddlewareExtensions
    {
        public static IApplicationBuilder UsePostLoginMiddleware(this IApplicationBuilder builder,
                                                                 PostLoginOptions options)
        {
            return builder.UseMiddleware<PostLoginMiddleware>(options);
        }
    }
}
