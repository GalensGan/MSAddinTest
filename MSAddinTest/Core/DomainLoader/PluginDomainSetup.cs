using Bentley.DgnPlatformNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSAddinTest.Core.DomainLoader
{
    /// <summary>
    /// 插件域的设置
    /// 配置参考：https://docs.microsoft.com/en-us/dotnet/api/system.appdomainsetup?view=netframework-4.8
    /// </summary>
    public class PluginDomainSetup
    {
        public PluginDomainSetup()
        {
            // 获取缓存的位置
            CachePath = ConfigurationManager.GetVariable("_USTN_HOMEROOT") + @"MsAddinTest";
            ApplicationBase = @"D:\Personal\Attempt\MSAddinTest\TestAddinPlugin\bin\Debug";
        }

        public string PluginName { get; set; }

        /// <summary>
        /// 子域名称
        /// </summary>
        public string ApplicationName => "pluginApp_" + PluginName;

        /// <summary>
        /// 程序根目录
        /// </summary>
        public string ApplicationBase { get; set; } = AppDomain.CurrentDomain.BaseDirectory;

        /// <summary>
        /// 根目录下面的目录
        /// 用来查找程序集用
        /// 多个目录用分号分隔
        /// </summary>
        public string PrivateBinPath { get; set; } = "Assemblies;Assemblies\\ECFramework;Mdlapps";

        /// <summary>
        /// 插件库文件缓存的位置
        /// </summary>
        public string CachePath { get; set; }

        /// <summary>
        /// Dll 全路径
        /// </summary>
        public string DllFullPath { get; set; }
    }
}
