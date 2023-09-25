using System;
using System.Collections.Generic;

namespace Baboon
{
    /// <summary>
    /// 模块目录
    /// </summary>
    public interface IModuleCatalog
    {
        /// <summary>
        /// 以类型直接添加模块
        /// </summary>
        /// <param name="moduleType"></param>
        void Add(Type moduleType);

        /// <summary>
        /// 获取当前目录下的所有模块
        /// </summary>
        /// <returns></returns>
        IEnumerable<AppModuleInfo> GetAppModules();

        /// <summary>
        /// 移除模块
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool RemoveAppModule(string id);

        /// <summary>
        /// 获取模块信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        AppModuleInfo GetAppModuleInfo(string id);

        /// <summary>
        /// 直接添加模块
        /// </summary>
        /// <param name="appModule"></param>
        void Add(IAppModule appModule);

        /// <summary>
        /// 添加一个模块构建器，能够实现需要时加载模块。
        /// </summary>
        /// <param name="builder"></param>
        void Add(ModuleDescriptionBuilder builder);

        /// <summary>
        /// 是否包含指定Id的模块。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool Contains(string id);

        /// <summary>
        /// 尝试获取模块信息。
        /// </summary>
        /// <param name="id"></param>
        /// <param name="appModuleInfo"></param>
        /// <returns></returns>
        bool TryGetAppModuleInfo(string id, out AppModuleInfo appModuleInfo);

        /// <summary>
        /// 更新本地模块目录。
        /// </summary>
        /// <returns></returns>
        int UpdateLocalAppModules();
    }
}