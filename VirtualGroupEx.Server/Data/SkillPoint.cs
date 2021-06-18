namespace VirtualGroupEx.Server.Data
{
    public enum SkillPoint : int
    {
        Editor = 1 << 0,
        Translator = 1 << 1,
        Timeline = 1 << 2,
        Reviewer = 1 << 3,
        PostProducer = 1 << 4,
        ArtDesigner = 1 << 5,
        Painter = 1 << 6,
        AccountHolder = 1 << 7,
        Compressor = 1 << 8,
    }
}
