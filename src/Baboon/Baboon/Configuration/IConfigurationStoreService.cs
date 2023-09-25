namespace Baboon
{
    /// <summary>
    /// 本地配置存储服务
    /// </summary>
    public interface IConfigurationStoreService
    {
        /// <summary>
        /// 获取对应Key的实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        T Get<T>(string key) where T : new();

        /// <summary>
        /// 重新加载存储服务
        /// </summary>
        void Reload();

        /// <summary>
        /// 设置对应Key的实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void Set<T>(string key, T value);
    }
}