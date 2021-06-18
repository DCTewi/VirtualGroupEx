using System.Collections.Generic;

namespace VirtualGroupEx.Server.Middlewares
{
    public class AccessProtectionOptions
    {
        public HashSet<string> Whitelist { get; set; } = new HashSet<string>();

        public string AuthPath = "/login";

        public string LogoutPath = "/logout";
    }
}
