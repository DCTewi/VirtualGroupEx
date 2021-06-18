using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;

namespace VirtualGroupEx.Server.Localization
{
    public class JsonStringLocalizerFactory : IStringLocalizerFactory
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ILogger<JsonStringLocalizer> logger;

        public JsonStringLocalizerFactory(IHttpContextAccessor httpContextAccessor, 
                                          ILogger<JsonStringLocalizer> logger)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.logger = logger;
        }

        public IStringLocalizer Create(Type resourceSource)
        {
            return new JsonStringLocalizer(httpContextAccessor, logger);
        }

        public IStringLocalizer Create(string baseName, string location)
        {
            return new JsonStringLocalizer(httpContextAccessor, logger);
        }
    }
}
