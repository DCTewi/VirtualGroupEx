using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtualGroupEx.Server.Data;

namespace VirtualGroupEx.Server.Services
{
    public class BulleinService
    {
        private readonly ApplicationDbContext db;

        public BulleinService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public List<Bulletin> GetAll()
        {
            return db.Bulletins
                .AsNoTracking()
                .ToList();
        }

        public async Task AddAsync(Bulletin bulletin)
        {
            db.Bulletins.Add(bulletin);
            await db.SaveChangesAsync();
            db.Entry(bulletin).State = EntityState.Detached;
        }

        public async Task UpdateAsync(Bulletin bulletin)
        {
            db.Bulletins.Update(bulletin);
            await db.SaveChangesAsync();
            db.Entry(bulletin).State = EntityState.Detached;
        }

        public async Task RemoveAsync(int id)
        {
            var bulletin = db.Bulletins.FirstOrDefault(b => b.Id == id);
            db.Bulletins
                .Remove(bulletin);

            await db.SaveChangesAsync();
        }
    }
}
