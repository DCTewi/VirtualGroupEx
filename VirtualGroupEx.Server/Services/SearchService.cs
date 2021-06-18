using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtualGroupEx.Server.Data;

namespace VirtualGroupEx.Server.Services
{
    public class SearchService
    {
        private readonly ApplicationDbContext db;

        public SearchService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public List<SearchResult> Search(string keyword)
        {
            List<SearchResult> result = new();

            keyword = keyword.Trim();

            result.AddRange(db.Users.AsNoTracking()
                .Where(u => u.NickName.Contains(keyword))
                .Select(u => new SearchResult { Type = SearchResultType.User, Result = u }));

            result.AddRange(db.Missions.AsNoTracking().Include(m => m.Sections)
                .Where(m => m.OriginInfo.Contains(keyword) || m.Title.Contains(keyword))
                .OrderBy(m => m.Status)
                .ThenByDescending(m => m.Priority)
                .ThenByDescending(m => m.Type)
                .Select(m => new SearchResult { Type = SearchResultType.Mission, Result = m }));

            result.AddRange(db.Discussions.AsNoTracking()
                .Where(d => d.Title.Contains(keyword)).Include(d => d.DiscussionPosts)
                .OrderByDescending(d => d.UpdateTime)
                .Select(d => new SearchResult { Type = SearchResultType.Discussion, Result = d }));

            result.AddRange(db.Routines.AsNoTracking()
                .Where(r => r.RoutineContent.Contains(keyword) || r.RoutineType.Contains(keyword))
                .OrderByDescending(r => r.DateTime)
                .Select(r => new SearchResult { Type = SearchResultType.Routine, Result = r }));

            return result;
        }
    }
}
