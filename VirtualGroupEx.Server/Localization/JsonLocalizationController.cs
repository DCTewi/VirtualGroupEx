using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace VirtualGroupEx.Server.Localization
{
    public class JsonLocalizationController : Controller
    {
        [Route(JsonLocalizerConstance.ControllerRoute)]
        public IActionResult Index(string lang, string redirect)
        {
            if (lang != null)
            {
                HttpContext.Response.Cookies.Append(JsonLocalizerConstance.CookieName, lang, new CookieOptions
                {
                    MaxAge = TimeSpan.FromDays(7)
                });
            }
            return LocalRedirect(string.IsNullOrEmpty(redirect) ? "/" : redirect);
        }
    }
}
