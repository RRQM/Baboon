using System.Collections.Concurrent;

namespace Baboon
{
    /// <summary>
    /// LoggerFactoryService
    /// </summary>
    public class LoggerFactoryService : ILoggerFactoryService
    {
        private readonly IConfigService m_config;

        private readonly ConcurrentDictionary<string, ILogger> m_loggers = new ConcurrentDictionary<string, ILogger>();

        /// <summary>
        /// LoggerFactoryService
        /// </summary>
        /// <param name="config"></param>
        public LoggerFactoryService(IConfigService config)
        {
            this.m_config = config;
        }

        /// <inheritdoc/>
        public ILogger GetLogger(string id)
        {
            return this.m_loggers.GetOrAdd(id, (newId) => new Logger(this.m_config.GetPathDirLogs(), id));
        }
    }
}