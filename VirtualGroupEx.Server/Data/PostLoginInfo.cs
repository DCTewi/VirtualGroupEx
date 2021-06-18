namespace VirtualGroupEx.Server.Data
{
    public struct PostLoginInfo
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
