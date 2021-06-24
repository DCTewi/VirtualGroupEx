using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtualGroupEx.Server.Data;

namespace VirtualGroupEx.Server.Services
{
    public class DiscussionService
    {
        private readonly ApplicationDbContext db;

        public DiscussionService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public Discussion GetById(int id)
        {
            var dis = db.Discussions
                .AsNoTracking()
                .Include(d => d.DiscussionPosts)
                .FirstOrDefault(d => d.Id == id);
            return dis;
        }

        public List<Discussion> GetAll()
        {
            var diss = db.Discussions
                .AsNoTracking()
                .Include(d => d.DiscussionPosts)
                .ToList();
            return diss;
        }

        public async Task<int> AddAsync(Discussion dis)
        {
            dis.CreationTime = DateTime.Now;
            dis.UpdateTime = dis.CreationTime;
            var result = db.Discussions.Add(dis);

            await db.SaveChangesAsync();
            db.Entry(dis).State = EntityState.Detached;
            return result.Entity.Id;
        }

        public async Task RemoveAsync(int id)
        {
            db.Discussions
                .Remove(db.Discussions.FirstOrDefault(d => d.Id == id));

            await db.SaveChangesAsync();
        }

        public async Task UpdateTitleById(int id, string newTitle)
        {
            var dis = db.Discussions
                .FirstOrDefault(d => d.Id == id);

            if (dis != null)
            {
                dis.Title = newTitle;
                dis.UpdateTime = DateTime.Now;
                db.Discussions.Update(dis);

                await db.SaveChangesAsync();
                db.Entry(dis).State = EntityState.Detached;
            }
        }

        public async Task TopAsync(int id)
        {
            var dis = GetById(id);
            dis.IsTopped = !dis.IsTopped;
            db.Discussions.Update(dis);

            await db.SaveChangesAsync();
            db.Entry(dis).State = EntityState.Detached;
        }

        public DiscussionPost GetLastestPostOf(int discussionId)
        {
            return db.DiscussionPosts
                .AsNoTracking()
                .OrderByDescending(d => d.CreationTime)
                .FirstOrDefault(d => d.DiscussionId == discussionId);
        }

        public List<DiscussionPost> GetPostsById(int discussionId)
        {
            return db.DiscussionPosts
                .AsNoTracking()
                .Where(p => p.DiscussionId == discussionId)
                .ToList();
        }

        public async Task<int> AddPostAsync(DiscussionPost post)
        {
            var now = DateTime.Now;

            var dis = GetById(post.DiscussionId);
            dis.UpdateTime = now;
            db.Discussions.Update(dis);

            post.CreationTime = now;
            var result = db.DiscussionPosts.Add(post);

            await db.SaveChangesAsync();
            db.Entry(dis).State = EntityState.Detached;
            db.Entry(post).State = EntityState.Detached;
            return result.Entity.Id;
        }

        public async Task RemovePostAsync(int postId)
        {
            db.DiscussionPosts
                .Remove(db.DiscussionPosts.FirstOrDefault(p => p.Id == postId));

            await db.SaveChangesAsync();
        }

        public List<Discussion> GetByUserId(string userId)
        {
            return db.Discussions
                .AsNoTracking()
                .Include(d => d.DiscussionPosts)
                .Where(d => d.CreatorUserId == userId)
                .OrderByDescending(d => d.UpdateTime)
                .ToList();
        }

        public List<DiscussionPost> GetPostsByUserId(string userId)
        {
            return db.DiscussionPosts
                .AsNoTracking()
                .Where(p => p.UserId == userId)
                .OrderByDescending(p => p.CreationTime)
                .ToList();
        }
    }
}
