namespace VirtualGroupEx.Server.Data
{
    public enum MissionType : int
    {
        SongClip = 1 << 0,
        LiveClip = 1 << 1,
        FullLive = 1 << 2,
        Original = 1 << 3,
        Collabor = 1 << 4,
    }
}
