using System.Collections.Concurrent;

namespace Baboon
{
    public class LoggerFactoryService : ILoggerFactoryService
    {
        public LoggerFactoryService(IConfigService config)
        {
            this.m_config = config;
        }

        private readonly ConcurrentDictionary<string, ILogger> m_loggers = new ConcurrentDictionary<string, ILogger>();
        private readonly IConfigService m_config;

        public ILogger GetLogger(string id)
        {
            return this.m_loggers.GetOrAdd(id, (newId) => new Logger(this.m_config.GetPathDirLogs(), id));
        }
    }
}