using System.IO;
using TouchSocket.Core;

namespace Baboon
{
    public class Logger : LoggerGroup, ILogger
    {
        public Logger(string id)
        {
            this.AddLogger(new FileLogger(Path.Combine(ConstUtility.Path_Dir_Logs, id))
            {
                LogLevel = LogLevel.Trace
            });
        }
    }
}
