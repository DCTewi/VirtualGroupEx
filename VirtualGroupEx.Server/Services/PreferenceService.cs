using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using VirtualGroupEx.Server.Data;

namespace VirtualGroupEx.Server.Services
{
    public class PreferenceService
    {
        private readonly Dictionary<string, PreferenceOptions> jsonWrapper = new();

        private readonly PreferenceOptions options;
        public PreferenceOptions Options
        {
            get
            {
                return options;
            }
            set
            {
                UpdatePreference(value);
            }
        }

        public static PreferenceOptions operator ~(PreferenceService instance)
        {
            return instance.Options;
        }

        public PreferenceService(IConfiguration configuration)
        {
            options = configuration.GetSection(PreferenceOptions.Preferences).Get<PreferenceOptions>();
        }

        private void UpdatePreference(PreferenceOptions newPreference)
        {
            jsonWrapper[PreferenceOptions.Preferences] = newPreference;

            var json = JsonConvert.SerializeObject(jsonWrapper);

            File.WriteAllText("preferences.json", json);
        }
    }
}
