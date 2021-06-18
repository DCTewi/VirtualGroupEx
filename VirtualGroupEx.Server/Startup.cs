using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Localization;
using VirtualGroupEx.Server.Data;
using VirtualGroupEx.Server.Localization;
using VirtualGroupEx.Server.Middlewares;
using VirtualGroupEx.Server.Services;

namespace VirtualGroupEx.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; init; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlite(Configuration.GetConnectionString("SQLite"));
            });

            services.AddIdentity<User, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredUniqueChars = 2;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = $"/login";
                options.LogoutPath = $"/logout";
                options.AccessDeniedPath = $"/login";
            });

            services.AddHttpContextAccessor();
            services.AddControllers();
            services.AddRazorPages();
            services.AddServerSideBlazor();

            services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<User>>();
            services.AddScoped<CurrentUserService>();
            services.AddScoped<UserInfoService>();
            services.AddScoped<RegisterService>();
            services.AddScoped<JSInvokeService>();
            services.AddScoped<AdminService>();
            services.AddScoped<BulleinService>();
            services.AddScoped<DiscussionService>();
            services.AddScoped<RoutineService>();
            services.AddScoped<MissionService>();
            services.AddScoped<SearchService>();
            services.AddScoped<UploadService>();

            services.AddSingleton<PreferenceService>();
            services.AddSingleton<IStringLocalizerFactory, JsonStringLocalizerFactory>();
            services.AddSingleton<IStringLocalizer, JsonStringLocalizer>();

#if DEBUG
            services.AddDatabaseDeveloperPageExceptionFilter();
#endif
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                context.Database.EnsureCreated();
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/error");
                app.UseHsts();

                app.UseForwardedHeaders(new ForwardedHeadersOptions
                {
                    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
                });
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UsePostLoginMiddleware(new PostLoginOptions
            {
                PostLoginPath = "/postLogin",
                LogOutPath = "/logout"
            });
            app.UseAccessProtectionMiddleware(new AccessProtectionOptions
            {
                AuthPath = "/login",
                LogoutPath = "/logout",
                Whitelist =
                {
                    "/login", "/postLogin", "/register", "/logout", "/locale", "/error"
                }
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
