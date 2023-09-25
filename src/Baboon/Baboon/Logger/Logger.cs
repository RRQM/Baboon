using System.IO;
using TouchSocket.Core;

namespace Baboon
{
    /// <summary>
    /// Logger
    /// </summary>
    public class Logger : LoggerGroup, ILogger
    {
        /// <summary>
        /// Logger
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="id"></param>
        public Logger(string dir, string id)
        {
            this.AddLogger(new FileLogger(Path.Combine(dir, id))
            {
                LogLevel = LogLevel.Trace
            });
        }
    }
}