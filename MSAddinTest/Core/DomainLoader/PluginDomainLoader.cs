using MSAddinTest.PluginInterface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace MSAddinTest.Core.DomainLoader
{
    /// <summary>
    /// 程序集动态加载
    /// </summary>
    public class PluginDomainLoader
    {
        public PluginDomainSetup PluginDomainSetup { get; private set; }

        // 新建的程序域
        private AppDomain _appDomain;

        // 程序集加载器
        private RemoteLoader _remoteLoader;

        /// <summary>
        /// 初始化程序域
        /// </summary>
        /// <param name="pluginName"></param>
        public PluginDomainLoader(PluginDomainSetup pluginDomainSetup)
        {
            PluginDomainSetup = pluginDomainSetup;
        }

        /// <summary>
        /// 在程序域中加载程序集
        /// </summary>
        /// <param name="assemblyFile"></param>
        public bool LoadAssembly()
        {
            var currentDomainBaseDir = AppDomain.CurrentDomain.BaseDirectory;
            // 配置参考：https://docs.microsoft.com/en-us/dotnet/api/system.appdomainsetup?view=netframework-4.8
            AppDomainSetup setup = new AppDomainSetup
            {
                ApplicationName = PluginDomainSetup.ApplicationName,
                ApplicationBase = PluginDomainSetup.ApplicationBase,
                PrivateBinPath = PluginDomainSetup.PrivateBinPath,
                CachePath = PluginDomainSetup.CachePath,
                ShadowCopyFiles = "true",
                ShadowCopyDirectories = currentDomainBaseDir,
            };

            // 设置原因参考：https://www.cnblogs.com/changrulin/p/4762816.html
            AppDomain.CurrentDomain.SetupInformation.ShadowCopyFiles = "true";

            try
            {
                _appDomain = AppDomain.CreateDomain(PluginDomainSetup.ApplicationName, null, setup);
                var name = Assembly.GetExecutingAssembly().GetName().FullName;

                _remoteLoader = (RemoteLoader)_appDomain.CreateInstanceAndUnwrap(name, typeof(RemoteLoader).FullName);
                _remoteLoader.SetAssemblyResolver(AppDomain.CurrentDomain.BaseDirectory);
                _remoteLoader.LoadAssembly(PluginDomainSetup.DllFullPath);

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        /// <summary>
        /// 卸载应用程序域
        /// </summary>
        public void Unload()
        {
            if (_appDomain == null) return;
            AppDomain.Unload(_appDomain);
            _appDomain = null;
            _remoteLoader = null;

            // 删除程序生成的文件
            Directory.Delete(Path.Combine(PluginDomainSetup.CachePath, PluginDomainSetup.ApplicationName), true);
        }

        /// <summary>
        /// 重新加载
        /// </summary>
        public void Reload()
        {
            // 先卸载程序
            Unload();

            // 重新加载程序
            LoadAssembly();
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
