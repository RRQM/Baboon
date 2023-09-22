using System.IO;

namespace Baboon
{
    public class ConfigService : IConfigService
    {
        public ConfigService()
        {
            if (!Directory.Exists(GetPathDirPlugins()))
            {
                Directory.CreateDirectory(GetPathDirPlugins());
            }

            if (!Directory.Exists(GetPathDirConfiguration()))
            {
                Directory.CreateDirectory(GetPathDirConfiguration());
            }

            if (!Directory.Exists(GetPathDirLogs()))
            {
                Directory.CreateDirectory(GetPathDirLogs());
            }

            if (!Directory.Exists(GetPathDirTemp()))
            {
                Directory.CreateDirectory(GetPathDirTemp());
            }
        }
        public string GetPathDirPlugins()
        {
            return "Plugins";
        }

        public string GetPathDirConfiguration()
        {
            return "Configuration";
        }

        public string GetPathDirLogs()
        {
            return "Logs";
        }

        public string GetPathFileConfigurationDb()
        {
            return Path.Combine(GetPathDirConfiguration(), "Configuration.db");
        }

        public string GetPathDirTemp()
        {
            return "Temp";
        }
    }
}