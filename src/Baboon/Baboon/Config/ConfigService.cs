using System.IO;

namespace Baboon
{
    public class ConfigService : IConfigService
    {
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