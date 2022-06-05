using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Landyvest.API.Shared
{
    public class ConfigHelper
    {
        private static ConfigHelper _appSettings;

        public string appSettingValue { get; set; }

        public static string AppSetting(string key)
        {
            _appSettings = GetCurrentSettings(key);
            return _appSettings.appSettingValue;
        }

        public ConfigHelper(IConfiguration config, string Key)
        {
            this.appSettingValue = config.GetValue<string>(Key);
        }

        public static ConfigHelper GetCurrentSettings(string key)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            IConfigurationRoot configuration = builder.Build();

            var settings = new ConfigHelper(configuration.GetSection("JWT"), key);

            return settings;
        }
    }


    public class ConfigHelpers
    {
        private static ConfigHelpers _appSettings;
        private static string _section;

        public string appSettingValue { get; set; }

        public static string AppSetting(string section = "", string key = "")
        {
            _section = section;
            _appSettings = GetCurrentSettings(key);
            return _appSettings.appSettingValue;
        }

        public ConfigHelpers(IConfiguration config, string Key)
        {
            appSettingValue = config.GetValue<string>(Key);
        }

        public static ConfigHelpers GetCurrentSettings(string key)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            IConfigurationRoot configuration = builder.Build();

            var settings = new ConfigHelpers(configuration.GetSection(_section), key);
            return settings;
        }
    }
}
