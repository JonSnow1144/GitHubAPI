using GitHubAPI.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GitHubAPI.Utilities
{
    public class SettingsHelper
    {
        private readonly IConfiguration _configuration;
        public SettingsHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Settings GetSettings()
        {
            var settings = new Settings();
            var config = _configuration.GetSection("GitHubSettings");
            settings.maxusers = int.TryParse(config["url"], out int max) ? max : 10;
            settings.cacheexpirationminutes = int.TryParse(config["cacheexpirationminutes"], out int minutes) ? minutes : 2;

            return settings;
        }

    }
}
