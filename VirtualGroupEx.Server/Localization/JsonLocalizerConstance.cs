using System.Collections.Generic;

using LocaleDictionary = System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, string>>;

namespace VirtualGroupEx.Server.Localization
{
    public static class JsonLocalizerConstance
    {
        public const string LocalizationPath = "Localization";
        public const string LocalizationJson = "i18n.json";

        public const string ControllerRoute = "locale";
        public const string CookieName = "lang";
        public const string DefaultLanguage = "zh-CN";

        public static readonly LocaleDictionary DefaultLocaleDictionary = new LocaleDictionary
                {
                    {
                        "zh-CN", new Dictionary<string, string>
                        {
                            { "Key", "Value" }
                        }
                    }
                };
    }
}
