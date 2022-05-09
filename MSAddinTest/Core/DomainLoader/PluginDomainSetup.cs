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
        public string PluginName { get; set; }

        /// <summary>
        /// 子域名称
        /// </summary>
        public string ApplicationName => "pluginApp_" + PluginName;

        /// <summary>
        /// 
        /// </summary>
        public string ApplicationBase { get; set; } = @"C:\Users\galens\Desktop\test" + ";" + AppDomain.CurrentDomain.BaseDirectory;

        /// <summary>
        /// 
        /// </summary>
        public string PrivateBinPath { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CachePath { get; set; }= @"C:\Users\galens\Desktop\test";

        /// <summary>
        /// Dll 全路径
        /// </summary>
        public string DllFullPath { get; set; }
    }
}
