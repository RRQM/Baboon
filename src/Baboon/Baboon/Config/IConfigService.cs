namespace Baboon
{
    public interface IConfigService
    {
        string GetPathDirConfiguration();
        string GetPathDirLogs();
        string GetPathDirPlugins();
        string GetPathDirTemp();
        string GetPathFileConfigurationDb();
    }
}
