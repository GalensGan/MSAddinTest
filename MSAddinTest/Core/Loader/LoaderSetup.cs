using Bentley.DgnPlatformNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSAddinTest.Core.Loader
{
    /// <summary>
    /// 插件域的设置
    /// 配置参考：https://docs.microsoft.com/en-us/dotnet/api/system.appdomainsetup?view=netframework-4.8
    /// </summary>
    public class LoaderSetup
    {
        /// <summary>
        /// 插件名称
        /// </summary>
        public string PluginName { get; set; }

        /// <summary>
        /// Dll 全路径
        /// </summary>
        private string _dllFullPath;
        public string DllFullPath
        {
            get => _dllFullPath;
            set
            {
                _dllFullPath = value;
                CurrentDomainBaseDirectory = System.IO.Path.GetDirectoryName(_dllFullPath);
            }
        }

        /// <summary>
        /// 当前程序集根目录
        /// </summary>
        public string CurrentDomainBaseDirectory { get; private set; }
    }
}
