using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VirtualGroupEx.Server.Data
{
    public class Discussion
    {
        public int Id { get; set; }

        [Required]
        public DiscussionType Type { get; set; }

        [Required]
        public string CreatorUserId { get; set; }

        public DateTime CreationTime { get; set; }

        public DateTime UpdateTime { get; set; }

        public int RelatedMissionId { get; set; }

        public bool IsTopped { get; set; } = false;

        [Required, MaxLength(30)]
        public string Title { get; set; }

        public List<DiscussionPost> DiscussionPosts { get; set; }
    }
}
