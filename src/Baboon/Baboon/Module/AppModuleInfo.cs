//using System;

//namespace Baboon
//{
//    /// <summary>
//    /// 模块信息
//    /// </summary>
//    public class AppModuleInfo
//    {
//        private readonly Func<IAppModule> m_appModuleFunc;
//        private IAppModule m_appModule;

//        /// <summary>
//        /// 模块信息
//        /// </summary>
//        /// <param name="appModule"></param>
//        public AppModuleInfo(IAppModule appModule)
//        {
//            this.Description = appModule.Description;
//            this.m_appModule = appModule;
//            this.Loaded = true;
//        }

//        /// <summary>
//        /// 模块信息
//        /// </summary>
//        /// <param name="description"></param>
//        /// <param name="appModuleFunc"></param>
//        public AppModuleInfo(ModuleDescription description, Func<IAppModule> appModuleFunc)
//        {
//            this.Description = description;
//            this.m_appModuleFunc = appModuleFunc;
//        }

//        /// <summary>
//        /// 模块描述
//        /// </summary>
//        public ModuleDescription Description { get; private set; }

//        /// <summary>
//        /// 是否已加载模块到程序域
//        /// </summary>
//        public bool Loaded { get; private set; }

//        /// <summary>
//        /// 根文件夹
//        /// </summary>
//        public string RootDir { get; set; }

//        /// <summary>
//        /// 获得模块
//        /// </summary>
//        /// <returns></returns>
//        public IAppModule GetApp()
//        {
//            if (this.Loaded)
//            {
//                return this.m_appModule;
//            }

//            this.m_appModule = this.m_appModuleFunc.Invoke();
//            this.Loaded = true;
//            return this.m_appModule;
//        }
//    }
//}