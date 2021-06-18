using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtualGroupEx.Server.Data;

namespace VirtualGroupEx.Server.Services
{
    public class UserInfoService
    {
        private readonly ApplicationDbContext db;

        public static readonly User UnknownUser = new()
        {
            Available = false,
            NickName = "-",
            UserName = "-",
        };

        public UserInfoService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public User GetUserById(string userId)
        {
            var result = db.Users
                .AsNoTracking()
                .FirstOrDefault(u => u.Id == userId);

            return result ?? UnknownUser;
        }

        public User GetUserByQid(long qid)
        {
            var result = db.Users
                .AsNoTracking()
                .FirstOrDefault(u => u.Qid == qid);

            return result ?? UnknownUser;
        }

        public List<User> GetAll()
        {
            return db.Users
                .AsNoTracking()
                .ToList();
        }
    }
}
