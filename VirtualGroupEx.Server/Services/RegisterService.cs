using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtualGroupEx.Server.Data;

namespace VirtualGroupEx.Server.Services
{
    public class RegisterService
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<User> userManager;
        private readonly ILogger<RegisterService> logger;

        public RegisterService(ApplicationDbContext db,
                               UserManager<User> userManager,
                               ILogger<RegisterService> logger)
        {
            this.db = db;
            this.userManager = userManager;
            this.logger = logger;
        }

        public async Task<IdentityResult> TryRegisterAsync(User user, string password)
        {
            var foldedUser = new User
            {
                UserName = user.UserName,
                NickName = user.NickName,
                Qid = user.Qid,
                SkillPoints = new HashSet<SkillPoint>()
            };

            if (!db.Users.AsNoTracking().Any())
            {
                logger.LogInformation(
                    $"First register request, add to operator and administrator");

                foldedUser.IsAdministrator = true;
                foldedUser.IsOperator = true;
                await CheckQidAsync(foldedUser.Qid, first: true);
            }
            else if (!await CheckQidAsync(foldedUser.Qid))
            {
                logger.LogInformation(
                    $"Qid[{foldedUser.Qid}] not registered");
                return IdentityResult.Failed(new IdentityError { Description = "Qid exist or not registered" });
            }

            var result = await userManager.CreateAsync(foldedUser, password);
            if (result.Succeeded)
            {
                var qid = db.RegisteredQids
                    .FirstOrDefault(q => q.Qid == foldedUser.Qid);
                qid.Used = true;

                db.RegisteredQids.Update(qid);
                await db.SaveChangesAsync();

                logger.LogInformation(
                    $"User[{foldedUser.UserName}] register success");

                return IdentityResult.Success;
            }
            else
            {
                logger.LogInformation(
                    $"User[{foldedUser.UserName}] register failed");

                return result;
            }
        }

        private async Task<bool> CheckQidAsync(long qq, bool first = false)
        {
            if (first)
            {
                db.RegisteredQids
                    .Add(new RegisteredQid { Qid = qq });
                await db.SaveChangesAsync();
                return true;
            }
            else
            {
                var qid = db.RegisteredQids
                    .AsNoTracking()
                    .FirstOrDefault(q => q.Qid == qq);

                if (qid == null || qid.Used)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
    }
}
