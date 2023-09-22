using System;
using System.Collections.Generic;

namespace Baboon
{
    public interface IModuleCatalog
    {
        void Add(Type moduleType);

        IEnumerable<AppModuleInfo> GetAppModules();

        bool RemoveAppModule(string id);

        AppModuleInfo GetAppModuleInfo(string id);

        void Add(IAppModule appModule);

        void Add(ModuleDescriptionBuilder builder);

        bool Contains(string id);

        bool TryGetAppModuleInfo(string id, out AppModuleInfo appModuleInfo);

        int UpdateLocalAppModules();
    }
}