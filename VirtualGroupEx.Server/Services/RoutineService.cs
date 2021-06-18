using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtualGroupEx.Server.Data;

namespace VirtualGroupEx.Server.Services
{
    public class RoutineService
    {
        private readonly ApplicationDbContext db;
        private readonly CurrentUserService currentUser;

        public RoutineService(ApplicationDbContext db,
                              CurrentUserService currentUser)
        {
            this.db = db;
            this.currentUser = currentUser;
        }

        public List<Routine> GetAll()
        {
            return db.Routines
                .AsNoTracking()
                .ToList();
        }

        public async Task AddAsync(Routine routine)
        {
            routine.DateTime = DateTime.Now;
            routine.UserId = currentUser.User.Id;

            db.Routines.Add(routine);
            await db.SaveChangesAsync();
            db.Entry(routine).State = EntityState.Detached;
        }

        public async Task UpdateAsync(Routine routine)
        {
            if (currentUser.User.Id == routine.UserId || currentUser.User.IsOperator)
            {
                db.Routines.Update(routine);
                await db.SaveChangesAsync();
                db.Entry(routine).State = EntityState.Detached;
            }
        }

        public async Task RemoveAsync(int id)
        {
            var routine = db.Routines
                .AsNoTracking()
                .FirstOrDefault(r => r.Id == id);

            if (currentUser.User.Id == routine.UserId || currentUser.User.IsOperator)
            {
                db.Routines.Remove(routine);

                await db.SaveChangesAsync();
            }
        }

        public List<Routine> GetByUserId(string userId)
        {
            return db.Routines
                .AsNoTracking()
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.DateTime)
                .ToList();
        }
    }
}
