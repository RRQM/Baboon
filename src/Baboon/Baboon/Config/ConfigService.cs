using System.IO;

namespace Baboon
{

    /// <summary>
    /// ConfigService
    /// </summary>
    public class ConfigService : IConfigService
    {
        /// <summary>
        /// ConfigService
        /// </summary>
        public ConfigService()
        {
            if (!Directory.Exists(this.GetPathDirModules()))
            {
                Directory.CreateDirectory(this.GetPathDirModules());
            }

            if (!Directory.Exists(this.GetPathDirConfiguration()))
            {
                Directory.CreateDirectory(this.GetPathDirConfiguration());
            }

            if (!Directory.Exists(this.GetPathDirLogs()))
            {
                Directory.CreateDirectory(this.GetPathDirLogs());
            }

            if (!Directory.Exists(this.GetPathDirTemp()))
            {
                Directory.CreateDirectory(this.GetPathDirTemp());
            }
        }

        /// <inheritdoc/>
        public string GetPathDirModules()
        {
            return "Modules";
        }

        /// <inheritdoc/>
        public string GetPathDirConfiguration()
        {
            return "Configuration";
        }

        /// <inheritdoc/>
        public string GetPathDirLogs()
        {
            return "Logs";
        }

        /// <inheritdoc/>
        public string GetPathFileConfigurationDb()
        {
            return Path.Combine(this.GetPathDirConfiguration(), "Configuration.db");
        }

        /// <inheritdoc/>
        public string GetPathDirTemp()
        {
            return "Temp";
        }
    }
}