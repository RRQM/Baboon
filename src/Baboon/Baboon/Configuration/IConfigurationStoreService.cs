namespace Baboon
{
    public interface IConfigurationStoreService
    {
        T Get<T>(string key);
        void Reload();
        void Set<T>(string key, T value);
    }
}
