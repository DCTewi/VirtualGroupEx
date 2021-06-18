using System.ComponentModel.DataAnnotations;

namespace VirtualGroupEx.Server.Data
{
    public class RegisteredQid
    {
        public int Id { get; set; }

        [Required]
        public long Qid { get; set; }

        public bool Used { get; set; } = false;
    }
}
