namespace VirtualGroupEx.Server.Middlewares
{
    public class PostLoginOptions
    {
        public string PostLoginPath { get; set; } = "/postLogin";

        public string LogOutPath { get; set; } = "/logout";
    }
}
