namespace VirtualGroupEx.Server.Data
{
    public enum MissionStatus : int
    {
        Processing = 1 << 0,
        Desperated = 1 << 1,
        Completed  = 1 << 2,
        Released   = 1 << 3,
    }
}
