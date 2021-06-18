using System;
using System.ComponentModel.DataAnnotations;

namespace VirtualGroupEx.Server.Data
{
    public class Notification
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public DateTime SentTime { get; set; }

        public bool HasRead { get; set; } = false;

        [Required, MaxLength(30)]
        public string Title { get; set; }

        [Required, MaxLength(256)]
        public string Content { get; set; }
    }
}
