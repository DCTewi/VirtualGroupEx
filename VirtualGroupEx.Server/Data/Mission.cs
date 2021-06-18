using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VirtualGroupEx.Server.Data
{
    public class Mission
    {
        public int Id { get; set; }

        /// <summary>
        /// Meaning              |
        /// 1-9     Low Priority |
        /// 10-99   Normal       |
        /// 100-999 Imperative   |
        /// </summary>
        [Required, Range(1, 999)]
        public int Priority { get; set; } = 50;

        [Required]
        public MissionType Type { get; set; }

        [Required]
        public MissionStatus Status { get; set; }

        public DateTime CreationTime { get; set; } = DateTime.Now;

        public string CreatorUserId { get; set; }

        [Required, MaxLength(30)]
        public string Title { get; set; }

        [Required, MaxLength(38), RegularExpression("^[0-9]{8}.*$")]
        public string OriginInfo { get; set; }

        [MaxLength(256)]
        public string Description { get; set; }

        [Required, RegularExpression("^([0-9]{1,}:)?[0-9]{1,2}:[0-9]{2}$")]
        public string Length { get; set; }

        [Url]
        public string PublishAddress { get; set; }

        public int DiscussionId { get; set; }

        [JsonIgnore]
        public List<MissionSection> Sections { get; set; }

        public string ColorString =>
            Status switch
            {
                MissionStatus.Desperated => "secondary",
                MissionStatus.Completed => "primary",
                MissionStatus.Released => "success",
                MissionStatus.Processing when Priority < 10 => "info",
                MissionStatus.Processing when Priority < 100 => "warning",
                MissionStatus.Processing when Priority < 1000 => "danger",
                _ => "dark"
            };

        public string StatusString =>
            Status switch
            {
                MissionStatus.Processing when Priority < 10 => "mission.status.LowPriority",
                MissionStatus.Processing when Priority < 100 => "mission.status.Normal",
                MissionStatus.Processing when Priority < 1000 => "mission.status.HighPriority",
                _ => $"mission.status.{Status}"
            };
    }
}
