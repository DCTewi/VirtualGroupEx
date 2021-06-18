using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VirtualGroupEx.Server.Data
{
    public class User : IdentityUser
    {
        [Required, MaxLength(30)]
        public string NickName { get; set; }

        [Required]
        public long Qid { get; set; }

        public bool Available { get; set; } = true;

        public bool IsAdministrator { get; set; } = false;

        public bool IsOperator { get; set; } = false;

        public HashSet<SkillPoint> SkillPoints { get; set; }

        [MaxLength(6)]
        public string Status { get; set; }

        [MaxLength(300)]
        public string Instruction { get; set; }
    }
}
