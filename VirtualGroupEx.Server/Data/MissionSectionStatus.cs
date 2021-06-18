namespace VirtualGroupEx.Server.Data
{
    public enum MissionSectionStatus
    {
        Empty        = 1 << 0,
        WaitingStaff = 1 << 1,
        Processing   = 1 << 2,
        Completed    = 1 << 3,
    }
}
