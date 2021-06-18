using System;

namespace VirtualGroupEx.Server.Data
{
    public class UploadFileInfo
    {
        public string Id { get; set; }

        public string OriginName { get; set; }

        public string UploaderId { get; set; }

        public DateTime UploadTime { get; set; }

        public int MissionId { get; set; }
    }
}
