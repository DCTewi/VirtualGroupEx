using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace VirtualGroupEx.Server.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public DbSet<Bulletin> Bulletins { get; set; }

        public DbSet<Mission> Missions { get; set; }

        public DbSet<MissionSection> MissionSections { get; set; }

        public DbSet<MissionColumn> MissionColumns { get; set; }

        public DbSet<RegisteredQid> RegisteredQids { get; set; }

        public DbSet<Routine> Routines { get; set; }

        public DbSet<Discussion> Discussions { get; set; }

        public DbSet<DiscussionPost> DiscussionPosts { get; set; }

        public DbSet<Notification> Notifications { get; set; }

        public DbSet<ApiKey> ApiKeys { get; set; }

        public DbSet<UploadFileInfo> UploadFileInfos { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>().Property(p => p.SkillPoints)
                .HasConversion(
                    v => JsonConvert.SerializeObject(v),
                    v => JsonConvert.DeserializeObject<HashSet<SkillPoint>>(v),
                    new ValueComparer<HashSet<SkillPoint>>(
                        (a, b) => a.SequenceEqual(b),
                        c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                        c => c.ToHashSet()));

            builder.Entity<User>()
                .Ignore(u => u.AccessFailedCount)
                .Ignore(u => u.LockoutEnabled)
                .Ignore(u => u.LockoutEnd)
                .Ignore(u => u.TwoFactorEnabled)
                .Ignore(u => u.Email)
                .Ignore(u => u.NormalizedEmail)
                .Ignore(u => u.EmailConfirmed)
                .Ignore(u => u.PhoneNumber)
                .Ignore(u => u.PhoneNumberConfirmed);

            builder.Entity<MissionSection>().Property(p => p.Demands)
                .HasConversion(
                    v => JsonConvert.SerializeObject(v),
                    v => JsonConvert.DeserializeObject<Dictionary<SkillPoint, int>>(v),
                    new ValueComparer<Dictionary<SkillPoint, int>>(
                        (a, b) => a.SequenceEqual(b),
                        c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                        c => c.ToArray().ToDictionary(x => x.Key, x => x.Value)));

            //builder.Entity<MissionColumn>().Property(p => p.TimeStamps)
            //    .HasConversion(
            //        v => JsonConvert.SerializeObject(v),
            //        v => JsonConvert.DeserializeObject<Dictionary<MissionColumnStatus, DateTime>>(v),
            //        new ValueComparer<Dictionary<MissionColumnStatus, DateTime>>(
            //            (a, b) => a.SequenceEqual(b),
            //            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
            //            c => c.ToArray().ToDictionary(x => x.Key, x => x.Value)));
        }
    }
}
