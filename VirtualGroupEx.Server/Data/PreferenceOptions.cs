using System;

namespace VirtualGroupEx.Server.Data
{
    public class PreferenceOptions
    {
        public const string Preferences = "Preferences";


        public string AppTitle { get; set; } = "VirtualGroup Ex";

        public string BackgroundUrl { get; set; } = "";

        public int PageCapacity { get; set; } = 40;

        public int FilePerMission { get; set; } = 5;

        public string DescriptionOriginPrefix { get; set; }

        public string DescriptionOriginPostfix { get; set; }

        public string DescriptionPostfix { get; set; }
    }
}
