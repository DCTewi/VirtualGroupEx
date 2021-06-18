using System;
using System.ComponentModel.DataAnnotations;

namespace VirtualGroupEx.Server.Data
{
    public class Bulletin
    {
        public int Id { get; set; }

        [Required, Range(1, 999)]
        public int Priority { get; set; }

        public string AuthorUserId { get; set; }

        [Required, MaxLength(30)]
        public string Title { get; set; }

        [Required]
        public string MarkdownContent { get; set; }

        public DateTime PublishTime { get; set; }

    }
}
