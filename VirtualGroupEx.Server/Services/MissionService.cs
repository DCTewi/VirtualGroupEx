using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtualGroupEx.Server.Data;

namespace VirtualGroupEx.Server.Services
{
    public class MissionService
    {
        private readonly ApplicationDbContext db;
        private readonly DiscussionService discussionService;

        public MissionService(ApplicationDbContext db, 
                              DiscussionService discussionService)
        {
            this.db = db;
            this.discussionService = discussionService;
        }

        public List<Mission> GetAll()
        {
            return db.Missions
                .AsNoTracking()
                .Include(m => m.Sections)
                .ToList();
        }

        public Mission GetById(int id)
        {
            return db.Missions
                .AsNoTracking()
                .Include(m => m.Sections)
                .FirstOrDefault(m => m.Id == id);
        }

        public async Task RemoveAsync(int id)
        {
            var mission = db.Missions.FirstOrDefault(m => m.Id == id);

            var discussion = db.Discussions.FirstOrDefault(d => d.RelatedMissionId == mission.Id);

            db.Discussions.Remove(discussion);
            db.Missions.Remove(mission);

            await db.SaveChangesAsync();
        }

        public async Task RemoveSectionAsync(int id)
        {
            var section = db.MissionSections.FirstOrDefault(s => s.Id == id);

            db.MissionSections.Remove(section);

            await db.SaveChangesAsync();
        }

        public async Task RemoveColumnAsync(int id)
        {
            var column = db.MissionColumns.FirstOrDefault(col => col.Id == id);

            db.MissionColumns.Remove(column);

            await db.SaveChangesAsync();
        }

        public MissionSection GetSectionIncludingColumns(int sectionId)
        {
            return db.MissionSections
                .AsNoTracking()
                .Include(s => s.MissionColumns)
                .FirstOrDefault(s => s.Id == sectionId);
        }

        public async Task<int> AddMissionAsync(Mission mission)
        {
            var result = db.Missions.Add(mission);
            await db.SaveChangesAsync();
            db.Entry(mission).State = EntityState.Detached;

            Discussion discussion = new()
            {
                Type = DiscussionType.MissionRelated,
                RelatedMissionId = result.Entity.Id,
                Title = mission.Title,
                CreatorUserId = mission.CreatorUserId,
            };

            var disId = await discussionService.AddAsync(discussion);

            mission.Id = result.Entity.Id;
            mission.DiscussionId = disId;
            await UpdateMissionAsync(mission);

            return result.Entity.Id;
        }

        public async Task AddSectionAsync(MissionSection section)
        {
            db.MissionSections.Add(section);
            await db.SaveChangesAsync();
            db.Entry(section).State = EntityState.Detached;
        }

        public async Task<bool> TryAddColumnAsync(int sectionId, MissionColumn col)
        {
            var section = GetSectionIncludingColumns(sectionId);

            if (section == null || !section.ValidateDemands(col.SkillPoint))
            {
                return false;
            }

            col.MissionSectionId = sectionId;

            db.MissionColumns.Add(col);

            await db.SaveChangesAsync();
            db.Entry(col).State = EntityState.Detached;

            return true;
        }

        public async Task UpdateMissionAsync(Mission mission)
        {
            db.Update(mission);

            await db.SaveChangesAsync();
            db.Entry(mission).State = EntityState.Detached;
        }

        public async Task ToggleMissionColumnStatusAsync(int colId)
        {
            var col = db.MissionColumns.FirstOrDefault(col => col.Id == colId);

            col.Status = col.Status switch
            {
                MissionColumnStatus.Proccessing => MissionColumnStatus.Completed,
                _ => MissionColumnStatus.Proccessing
            };

            db.Update(col);

            await db.SaveChangesAsync();
            db.Entry(col).State = EntityState.Detached;
        }

        public async Task UpdateMissionColumnAsync(MissionColumn col)
        {
            db.Update(col);

            await db.SaveChangesAsync();
            db.Entry(col).State = EntityState.Detached;
        }

        public List<MissionColumn> GetColumnsByUserId(string userId)
        {
            return db.MissionColumns
                .AsNoTracking()
                .Where(col => col.UserId == userId)
                .OrderByDescending(col => col.JoinTime)
                .ToList();
        }

        public Mission GetMissionBySectionId(int sectionId)
        {
            var section = db.MissionSections
                .AsNoTracking()
                .FirstOrDefault(s => s.Id == sectionId);

            if (section != null)
            {
                return GetById(section.MissionId);
            }

            return null;
        }

        public int GetColumnsCountByUserId(string userId)
        {
            return db.MissionColumns
                .AsNoTracking()
                .Where(col => col.UserId == userId)
                .Count();
        }

        public int GetColumnsCountLastMonthByUserId(string userId)
        {
            return db.MissionColumns
                .AsNoTracking()
                .Where(col => col.UserId == userId && col.JoinTime.AddDays(30).CompareTo(DateTime.Now) > 0)
                .Count();
        }
    }
}
