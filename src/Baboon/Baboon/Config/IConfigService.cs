namespace Baboon
{
    /// <summary>
    /// 配置服务
    /// </summary>
    public interface IConfigService
    {
        /// <summary>
        /// 获取配置文件夹路径
        /// </summary>
        /// <returns></returns>
        string GetPathDirConfiguration();

        /// <summary>
        /// 获取日志文件夹路径
        /// </summary>
        /// <returns></returns>
        string GetPathDirLogs();

        /// <summary>
        /// 获取模块文件夹路径
        /// </summary>
        /// <returns></returns>
        string GetPathDirModules();

        /// <summary>
        /// 获取临时缓存文件夹路径
        /// </summary>
        /// <returns></returns>
        string GetPathDirTemp();

        /// <summary>
        /// 获取本地数据库文件路径
        /// </summary>
        /// <returns></returns>
        string GetPathFileConfigurationDb();
    }
}