namespace Baboon
{
    /// <summary>
    /// 日志记录服务
    /// </summary>
    public interface ILoggerFactoryService
    {
        /// <summary>
        /// 通过Id获取一个日志记录器。通常不同Id会对应不同的日志文件夹。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ILogger GetLogger(string id);
    }
}