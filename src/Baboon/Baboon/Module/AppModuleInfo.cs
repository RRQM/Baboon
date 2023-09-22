using System;

namespace Baboon
{
    public class AppModuleInfo
    {
        private readonly Func<IAppModule> m_appModuleFunc;
        private IAppModule m_appModule;

        public AppModuleInfo(IAppModule appModule)
        {
            this.Description = appModule.Description;
            this.m_appModule = appModule;
            this.Loaded = true;
        }
        public string RootDir { get; set; }
        public AppModuleInfo(ModuleDescription description, Func<IAppModule> appModuleFunc)
        {
            this.Description = description;
            this.m_appModuleFunc = appModuleFunc;
        }

        public ModuleDescription Description { get; private set; }
        public bool Loaded { get; private set; }

        public IAppModule GetApp()
        {
            if (this.Loaded)
            {
                return this.m_appModule;
            }

            this.m_appModule = this.m_appModuleFunc.Invoke();
            this.Loaded = true;
            return this.m_appModule;
        }
    }
}