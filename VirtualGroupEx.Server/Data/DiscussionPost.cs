using System;
using System.ComponentModel.DataAnnotations;

namespace VirtualGroupEx.Server.Data
{
    public class DiscussionPost
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public DateTime CreationTime { get; set; }

        [Required, MaxLength(500)]
        public string Content { get; set; }

        [Required]
        public int DiscussionId { get; set; }

        public Discussion Discussion { get; set; }
    }
}
