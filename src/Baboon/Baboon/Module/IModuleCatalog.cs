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
        IEnumerable<IAppModule> GetAppModules();

        /// <summary>
        /// 获取模块信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IAppModule GetAppModule(string id);

        /// <summary>
        /// 直接添加模块
        /// </summary>
        /// <param name="appModule"></param>
        void Add(IAppModule appModule);

        /// <summary>
        /// 是否包含指定Id的模块。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool Contains(string id);

        bool IsReadonly { get; }
    }
}