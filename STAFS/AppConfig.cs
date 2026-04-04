using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace SATF
{
    public static class AppConfig
    {
        /// <summary>
        /// Loads appsettings.json from the first location that contains the file:
        /// AppContext.BaseDirectory, the executing assembly directory, then current working directory.
        /// </summary>
        public static IConfigurationRoot GetConfig()
        {
            foreach (string basePath in GetSearchBasePaths())
            {
                if (string.IsNullOrEmpty(basePath) || !Directory.Exists(basePath))
                    continue;

                string configFilePath = Path.Combine(basePath, "appsettings.json");
                if (!File.Exists(configFilePath))
                    continue;

                var builder = new ConfigurationBuilder()
                    .SetBasePath(basePath)
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

                return builder.Build();
            }

            return null;
        }

        private static IEnumerable<string> GetSearchBasePaths()
        {
            var paths = new List<string>
            {
                AppContext.BaseDirectory
            };

            try
            {
                string location = Assembly.GetExecutingAssembly().Location;
                if (!string.IsNullOrEmpty(location))
                {
                    string dir = Path.GetDirectoryName(location);
                    if (!string.IsNullOrEmpty(dir))
                        paths.Add(dir);
                }
            }
            catch
            {
                // ignored
            }

            paths.Add(Directory.GetCurrentDirectory());
            return paths;
        }
    }
}
