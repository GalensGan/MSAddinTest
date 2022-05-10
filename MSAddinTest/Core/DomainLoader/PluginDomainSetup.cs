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
        /// <summary>
        /// 插件名称
        /// </summary>
        public string PluginName { get; set; }

        /// <summary>
        /// Dll 全路径
        /// </summary>
        public string DllFullPath { get; set; }
    }
}
