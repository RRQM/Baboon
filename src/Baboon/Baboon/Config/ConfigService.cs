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
        public virtual string GetPathDirModules()
        {
            return "Modules";
        }

        /// <inheritdoc/>
        public virtual string GetPathDirConfiguration()
        {
            return "Configuration";
        }

        /// <inheritdoc/>
        public virtual string GetPathDirLogs()
        {
            return "Logs";
        }

        /// <inheritdoc/>
        public virtual string GetPathFileConfigurationDb()
        {
            return Path.Combine(this.GetPathDirConfiguration(), "Configuration.db");
        }

        /// <inheritdoc/>
        public virtual string GetPathDirTemp()
        {
            return "Temp";
        }
    }
}