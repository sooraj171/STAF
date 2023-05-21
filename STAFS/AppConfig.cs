using Microsoft.Extensions.Configuration;
using System.IO;

namespace STAF
{
    public static class AppConfig
    {

        public static IConfigurationRoot GetConfig()
        {
            IConfigurationRoot configuration = null;
            try
            {
                string configFilePath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
                if (File.Exists(configFilePath))
                {

                    var builder = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

                    configuration = builder.Build();

                }
            }
            catch { }
            return configuration;
        }
    }
}
