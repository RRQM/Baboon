using System.Collections.Concurrent;

namespace Baboon
{
    public class LoggerFactoryService : ILoggerFactoryService
    {
        private readonly ConcurrentDictionary<string, ILogger> m_loggers = new ConcurrentDictionary<string, ILogger>();
        public ILogger GetLogger(string id)
        {
            return this.m_loggers.GetOrAdd(id, (newId) => new Logger(id));
        }
    }
}
