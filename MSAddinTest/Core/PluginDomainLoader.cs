using MSAddinTest.Plugin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace MSAddinTest.Core
{
    /// <summary>
    /// 程序集动态加载
    /// </summary>
    public class PluginDomainLoader
    {
        public string PluginName { get; private set; }

        // 新建的程序域
        private AppDomain _appDomain;

        // 程序集加载器
        private RemoteLoader _remoteLoader;

        /// <summary>
        /// 初始化程序域
        /// </summary>
        /// <param name="pluginName"></param>
        public PluginDomainLoader(string pluginName)
        {
            PluginName = pluginName;

            var currentDomainBaseDir = AppDomain.CurrentDomain.BaseDirectory;
            AppDomainSetup setup = new AppDomainSetup
            {
                ApplicationName = "app_" + pluginName,
                ApplicationBase = currentDomainBaseDir,
                PrivateBinPath = Path.Combine(currentDomainBaseDir, "Plugins"),
                CachePath = currentDomainBaseDir,
                ShadowCopyFiles = "true",
                ShadowCopyDirectories = currentDomainBaseDir,
            };

            _appDomain = AppDomain.CreateDomain("pluginApp_" + pluginName, null, setup);
            var name = Assembly.GetExecutingAssembly().GetName().FullName;
            _remoteLoader = (RemoteLoader)_appDomain.CreateInstanceAndUnwrap(name, typeof(RemoteLoader).FullName);
        }

        /// <summary>
        /// 在程序域中加载程序集
        /// </summary>
        /// <param name="assemblyFile"></param>
        public void LoadAssembly(string assemblyFile)
        {
            _remoteLoader.LoadAssembly(assemblyFile);
        }

        /// <summary>
        /// 卸载应用程序域
        /// </summary>
        public void Unload()
        {
            try
            {
                if (_appDomain == null) return;
                AppDomain.Unload(_appDomain);
                _appDomain = null;
                _remoteLoader = null;
            }
            catch (CannotUnloadAppDomainException ex)
            {
                throw ex;
            }
        }

        #region remoteLoader中介
        /// <summary>
        /// 执行类型方法
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="methodName"></param>
        /// <returns></returns>
        public object Execute(string name, PluginArg arg)
        {
            return _remoteLoader.Execute(name, arg);
        }
        #endregion
    }
}
