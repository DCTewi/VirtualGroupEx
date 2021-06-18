using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using VirtualGroupEx.Server.Data;

namespace VirtualGroupEx.Server.Services
{
    public class CurrentUserService
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly UserManager<User> userManager;
        private readonly ApplicationDbContext db;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor, 
                                  UserManager<User> userManager, 
                                  ApplicationDbContext db)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.userManager = userManager;
            this.db = db;
        }

        private User _user;
        public User User
        {
            get
            {
                if (_user == null)
                {
                    var username = Principal?.Identity.Name;

                    if (string.IsNullOrEmpty(username))
                    {
                        return null;
                    }

                    return _user = db.Users
                        .AsNoTracking()
                        .FirstOrDefault(u => u.UserName == username);
                }
                else return _user;
            }
        }

        public ClaimsPrincipal Principal => httpContextAccessor?.HttpContext?.User;

        public string IPAddress => httpContextAccessor.HttpContext.Request.Host.Value;

        public async Task ChangeNickNameAsync(string nickname)
        {
            var user = await db.Users
                .FirstOrDefaultAsync(u => u.UserName == Principal.Identity.Name);

            if (user != null)
            {
                user.NickName = nickname;
                db.Users.Update(user);
            }

            await db.SaveChangesAsync();
        }

        public async Task<IdentityResult> ChangePasswordAsync(string oldPassword, string newPassword)
        {
            var user = await userManager.GetUserAsync(Principal);
            return await userManager.ChangePasswordAsync(user, oldPassword, newPassword);
        }
    }
}
