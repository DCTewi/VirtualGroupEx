using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtualGroupEx.Server.Data;

namespace VirtualGroupEx.Server.Services
{
    public class AdminService
    {
        private readonly ApplicationDbContext db;
        private readonly CurrentUserService currentUser;
        private readonly UserManager<User> userManager;

        public AdminService(ApplicationDbContext db, 
                            CurrentUserService currentUser, 
                            UserManager<User> userManager)
        {
            this.db = db;
            this.currentUser = currentUser;
            this.userManager = userManager;
        }

        public List<User> GetAllUsers()
        {
            return db.Users.AsNoTracking()
                .ToList();
        }

        public async Task UpdateSkillPointAsync(string userId, HashSet<SkillPoint> sps)
        {
            if (currentUser.User.IsOperator && sps != null)
            {
                var user = db.Users.FirstOrDefault(u => u.Id == userId);
                user.SkillPoints = sps;
                db.Users.Update(user);

                await db.SaveChangesAsync();
            }
        }

        public async Task BanAsync(string userId, bool unban = false)
        {
            if (currentUser.User.IsOperator && currentUser.User.Id != userId)
            {
                var user = db.Users.FirstOrDefault(u => u.Id == userId);

                // Now you can't unregister a used qid
                // 
                //if (unban)
                //{
                //    if (!db.RegisteredQids.AsNoTracking().Any(q => q.Qid == user.Qid))
                //    {
                //        // Qid was unregistered, you need reregister first
                //        return;
                //    }
                //}

                if (user.IsOperator) return;

                user.Available = unban;
                db.Users.Update(user);

                await db.SaveChangesAsync();
            }
        }

        public async Task RegisterQidAsync(long qid)
        {
            if (currentUser.User.IsOperator)
            {
                if (!db.RegisteredQids.AsNoTracking().Any(q => q.Qid == qid))
                {
                    db.RegisteredQids.Add(new RegisteredQid
                    {
                        Qid = qid,
                        Used = false
                    });

                    await db.SaveChangesAsync();
                }
            }
        }

        public async Task UnregisterQidAsync(long qid)
        {
            if (currentUser.User.IsOperator)
            {
                var rqid = db.RegisteredQids
                    .FirstOrDefault(q => q.Qid == qid);

                if (rqid != null && !rqid.Used)
                {
                    db.RegisteredQids.Remove(rqid);
                }

                await db.SaveChangesAsync();
            }
        }

        public List<RegisteredQid> GetAllRegisteredQid()
        {
            return db.RegisteredQids
                .AsNoTracking()
                .ToList();
        }

        public async Task SetOperatorAsync(string userId, bool toTrue = true)
        {
            if (currentUser.User.IsAdministrator)
            {
                var user = db.Users.FirstOrDefault(u => u.Id == userId);
                user.IsOperator = toTrue;
                db.Users.Update(user);

                await db.SaveChangesAsync();
            }
        }

        public async Task<bool> TryDeleteUserAsync(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);

            var qid = db.RegisteredQids.FirstOrDefault(q => q.Qid == user.Qid);
            qid.Used = false;
            db.RegisteredQids.Update(qid);

            await db.SaveChangesAsync();
            db.Entry(qid).State = EntityState.Detached;

            var result = await userManager.DeleteAsync(user);

            return result.Succeeded;
        }

        public async Task<bool> ResetUserPasswordAsync(string userId, string password)
        {
            var user = await userManager.FindByIdAsync(userId);

            var token = await userManager.GeneratePasswordResetTokenAsync(user);

            var result = await userManager.ResetPasswordAsync(user, token, password);

            return result.Succeeded;
        }
    }
}
