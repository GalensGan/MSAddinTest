using MSAddinTest.Core.Loader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSAddinTest.Core.Command
{
    /// <summary>
    /// 添加Addin插件
    /// </summary>
    internal class LoadAddinPluginCommand: LoadPluginCommand
    {
        protected override void AssemblyLoaded(PluginAssemblyLoader loader)
        {
            base.AssemblyLoaded(loader);

            // 对 addin 进行初始化
        }
    }
}
