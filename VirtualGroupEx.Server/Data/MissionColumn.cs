using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VirtualGroupEx.Server.Data
{
    public class MissionColumn
    {
        public int Id { get; set; }

        [Required]
        public int MissionSectionId { get; set; }

        [JsonIgnore]
        public MissionSection MissionSection { get; set; }

        [Required]
        public SkillPoint SkillPoint { get; set; }

        public string UserId { get; set; }

        public MissionColumnStatus Status { get; set; } = MissionColumnStatus.Proccessing;

        [MaxLength(60)]
        public string Note { get; set; }

        public DateTime JoinTime { get; set; } = DateTime.Now;

        // public Dictionary<MissionColumnStatus, DateTime> TimeStamps { get; set; } = new Dictionary<MissionColumnStatus, DateTime>();
    }
}
