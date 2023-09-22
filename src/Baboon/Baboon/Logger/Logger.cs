using System.IO;
using TouchSocket.Core;

namespace Baboon
{
    public class Logger : LoggerGroup, ILogger
    {
        public Logger(string dir, string id)
        {
            this.AddLogger(new FileLogger(Path.Combine(dir, id))
            {
                LogLevel = LogLevel.Trace
            });
        }
    }
}