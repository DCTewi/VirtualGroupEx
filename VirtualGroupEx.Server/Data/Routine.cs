using System;
using System.ComponentModel.DataAnnotations;

namespace VirtualGroupEx.Server.Data
{
    public class Routine
    {
        public int Id { get; set; }

        public DateTime DateTime { get; set; }

        [Required, MaxLength(30)]
        public string RoutineType { get; set; }

        [Required, MaxLength(300)]
        public string RoutineContent { get; set; }

        public string UserId { get; set; }
    }
}
