using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using LocaleDictionary = System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, string>>;

namespace VirtualGroupEx.Server.Localization
{
    public class JsonStringLocalizer : IStringLocalizer
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly LocaleDictionary resources;

        public JsonStringLocalizer(IHttpContextAccessor httpContextAccessor,
                                   ILogger<JsonStringLocalizer> logger)
        {
            this.httpContextAccessor = httpContextAccessor;

            try
            {
                var jsonString = File.ReadAllText(
                    $@"{JsonLocalizerConstance.LocalizationPath}/{JsonLocalizerConstance.LocalizationJson}");

                resources = JsonConvert.DeserializeObject<LocaleDictionary>(jsonString);
            }
            catch (Exception)
            {
                logger.LogWarning(
                    $"I18n.json not found! try to create a new one.");

                resources = JsonLocalizerConstance.DefaultLocaleDictionary;

                if (!Directory.Exists(JsonLocalizerConstance.LocalizationPath))
                {
                    Directory.CreateDirectory(JsonLocalizerConstance.LocalizationPath);
                }
                File.WriteAllText(
                    $@"{JsonLocalizerConstance.LocalizationPath}/{JsonLocalizerConstance.LocalizationJson}",
                    JsonConvert.SerializeObject(resources));
            }
        }

        public LocalizedString this[string name]
        {
            get
            {
                var value = GetString(name);
                return new LocalizedString(name, value ?? name, resourceNotFound: value == null);
            }
        }

        public LocalizedString this[string name, params object[] arguments]
        {
            get
            {
                var format = GetString(name);
                var value = string.Format(format ?? name, arguments);
                return new LocalizedString(name, value, resourceNotFound: format == null);
            }
        }

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            string lang = string.Empty;
            var success = httpContextAccessor.HttpContext?.Request?.Cookies.TryGetValue(JsonLocalizerConstance.CookieName, out lang) ?? false;
            return resources
                .FirstOrDefault(kkp => kkp.Key == (success ? lang : JsonLocalizerConstance.DefaultLanguage)).Value
                .Select(kp => new LocalizedString(kp.Key, kp.Value));
        }

        private string GetString(string name)
        {
            string lang = JsonLocalizerConstance.DefaultLanguage;
            string result = string.Empty;
            if (httpContextAccessor.HttpContext?.Request?.Cookies.TryGetValue(JsonLocalizerConstance.CookieName, out result) ?? false)
            {
                if (resources.ContainsKey(result))
                {
                    lang = result;
                }
            }
            return resources[lang]?.GetValueOrDefault(name, name);
        }
    }
}
