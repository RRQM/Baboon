namespace Baboon
{
    public interface ILoggerFactoryService
    {
        ILogger GetLogger(string id);
    }
}
