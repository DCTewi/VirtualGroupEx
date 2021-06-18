using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace VirtualGroupEx.Server.Data
{
    public class MissionSection
    {
        public int Id { get; set; }

        [Required]
        public int MissionId { get; set; }

        [JsonIgnore]
        public Mission Mission { get; set; }

        [JsonIgnore]
        public List<MissionColumn> MissionColumns { get; set; }

        [MaxLength(6)]
        public string Description { get; set; }

        public Dictionary<SkillPoint, int> Demands { get; set; } = new Dictionary<SkillPoint, int>();

        //public string GetColorString(SkillPoint sp = 0)
        //{
        //    MissionSectionStatus status = MissionSectionStatus.Empty;
        //    if (sp == 0)
        //    {
        //        status = GetStatus();
        //    }
        //    else if (!Demands.ContainsKey(sp) || MissionColumns == null)
        //    {
        //        status = MissionSectionStatus.Empty;
        //    }
        //    else
        //    {

        //        var cols = MissionColumns.Where(col => col.SkillPoint == sp);
        //        if (cols.Count() < Demands[sp])
        //        {
        //            status = MissionSectionStatus.WaitingStaff;
        //        }
        //        else if (cols.Any(col => col.Status != MissionColumnStatus.Completed))
        //        {
        //            status = MissionSectionStatus.Processing;
        //        }
        //        else status = MissionSectionStatus.Completed;
        //    }

        //    return status switch
        //    {
        //        MissionSectionStatus.Empty => "#f5f53d",
        //        MissionSectionStatus.WaitingStaff => "#f5f53d",
        //        MissionSectionStatus.Processing => "#66ccff",
        //        MissionSectionStatus.Completed => "#00e600",
        //        _ => "#999999",
        //    };
        //}

        /// <summary>
        /// You can only call this method after eager loading
        /// </summary>
        /// <returns></returns>
        public ISet<SkillPoint> GetDemandSet()
        {
            if (MissionColumns == null)
            {
                throw new InvalidCastException("You can only call this method after eager loading");
            }

            var demands = new Dictionary<SkillPoint, int>(Demands);

            foreach (var col in MissionColumns)
            {
                demands[col.SkillPoint] -= 1;
            }

            var result = new HashSet<SkillPoint>();

            foreach (var kp in demands)
            {
                if (kp.Value > 0)
                {
                    result.Add(kp.Key);
                }
            }

            return result;
        }

        /// <summary>
        /// You can only call this method after eager loading
        /// </summary>
        /// <param name="sp">Skill point to validate</param>
        /// <returns></returns>
        public bool ValidateDemands(SkillPoint sp)
        {
            if (MissionColumns == null)
            {
                throw new InvalidCastException("You can only call this method after eager loading");
            }

            if (!Demands.ContainsKey(sp))
            {
                return false;
            }

            int joinedCount = MissionColumns.Count(col => col.SkillPoint == sp);

            return joinedCount < Demands[sp];
        }

        /// <summary>
        /// You can only call this method after eager loading
        /// </summary>
        /// <param name="sp">Skill point to count</param>
        /// <returns></returns>
        public int GetColumnCount(SkillPoint sp)
        {
            if (MissionColumns == null || !Demands.ContainsKey(sp))
            {
                return -1;
            }

            return MissionColumns.Count(col => col.SkillPoint == sp);
        }

        /// <summary>
        /// You can only call this method after eager loading
        /// </summary>
        /// <returns>Status</returns>
        public MissionSectionStatus GetStatus()
        {
            if (MissionColumns == null || MissionColumns.Count == 0)
            {
                return MissionSectionStatus.Empty;
            }

            var sum = Demands.Sum(kvp => kvp.Value);
            if (sum > MissionColumns.Count)
            {
                return MissionSectionStatus.WaitingStaff;
            }

            if (MissionColumns.Any(col => col.Status == MissionColumnStatus.Proccessing))
            {
                return MissionSectionStatus.Processing;
            }

            return MissionSectionStatus.Completed;
        }
    }
}
