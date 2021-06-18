using System;
using System.ComponentModel.DataAnnotations;

namespace VirtualGroupEx.Server.Data
{
    public class ApiKey
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        [Required, MaxLength(30)]
        public string Description { get; set; } = DateTime.Now.ToShortDateString();

        public Guid Key { get; set; }
    }
}
