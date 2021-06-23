using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using VirtualGroupEx.Server.Data;

namespace VirtualGroupEx.Server.Services
{
    public class UploadService
    {
        private readonly CurrentUserService currentUser;
        private readonly ApplicationDbContext db;
        private readonly IConfiguration configuration;

        public UploadService(CurrentUserService currentUser, 
                             ApplicationDbContext db, 
                             IConfiguration configuration)
        {
            this.currentUser = currentUser;
            this.db = db;
            this.configuration = configuration;
        }

        public string GetFileListJson()
        {
            var infos = db.UploadFileInfos.AsNoTracking().ToList();

            return JsonConvert.SerializeObject(infos);
        }

        public List<UploadFileInfo> GetFileInfoByMissionId(int missionId)
        {
            return db.UploadFileInfos
                .AsNoTracking()
                .Where(info => info.MissionId == missionId)
                .ToList();
        }

        public async Task RefreshFileCacheAsync()
        {
            var infos = db.UploadFileInfos.AsNoTracking().ToList();
            List<string> info2Remove = new();

            foreach (var info in infos)
            {
                var path = Path.Combine(configuration.GetValue<string>("UploadRootPath"), info.MissionId.ToString(), info.Id);
                if (!File.Exists(path))
                {
                    info2Remove.Add(info.Id);
                }
            }

            foreach (var id in info2Remove)
            {
                db.Remove(db.UploadFileInfos.FirstOrDefault(i => i.Id == id));
            }

            await db.SaveChangesAsync();
        }

        public async Task<(FileStream, string)> GenerateFileInfoAsync(int missionId, string filename)
        {
            var newId = Guid.NewGuid().ToString("N");

            UploadFileInfo info = new()
            {
                Id = newId,
                MissionId = missionId,
                OriginName = filename,
                UploadTime = DateTime.Now,
                UploaderId = currentUser.User.Id,
            };

            var result = db.UploadFileInfos.Add(info);
            await db.SaveChangesAsync();

            var path = Path.Combine(configuration.GetValue<string>("UploadRootPath"), missionId.ToString());

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return (new FileStream(Path.Combine(path, result.Entity.Id), FileMode.Create), newId);
        }

        public async Task RemoveFileInfoAsync(string infoId)
        {
            var info = db.UploadFileInfos.FirstOrDefault(i => i.Id == infoId);
            var path = Path.Combine(configuration.GetValue<string>("UploadRootPath"), info.MissionId.ToString(), info.Id);

            if (File.Exists(path))
            {
                File.Delete(path);
            }

            db.UploadFileInfos.Remove(info);

            await db.SaveChangesAsync();
        }

        public string GetFileQueryString(string infoId)
        {
            var info = db.UploadFileInfos.FirstOrDefault(i => i.Id == infoId);

            return $"?mid={info?.MissionId}&hash={info?.Id}&name={info?.OriginName}";
        }
    }
}
