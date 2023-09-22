namespace Baboon
{
    public class ConfigService : IConfigService
    {
        public string GetPathDirPlugins()
        {
            return ConstUtility.Path_Dir_Plugins;
        }

        public string GetPathDirConfiguration()
        {
            return ConstUtility.Path_Dir_Configuration;
        }

        public string GetPathDirLogs()
        {
            return ConstUtility.Path_Dir_Logs;
        }

        public string GetPathFileConfigurationDb()
        {
            return ConstUtility.Path_File_ConfigurationDb;
        }

        public string GetPathDirTemp()
        {
            return ConstUtility.Path_Dir_Temp;
        }
    }
}
