using System.Collections.Generic;
using System.IO;

namespace Baboon
{
    public interface IModuleManagerService
    {
        void AddInstallToDo(string filePath);
        void AddUninstallToDo(string id);
        void ClearInstallToDo();
        void ClearUninstallToDo();
        List<string> GetInstallToDoList();
        List<string> GetUninstallToDoList();
        void Install(Stream stream);
        void RemoveInstallToDo(string id);
        void RemoveUninstallToDo(string id);
        void Uninstall(string id);
    }
}
