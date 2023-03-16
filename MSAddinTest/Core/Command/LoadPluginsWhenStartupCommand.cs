using Bentley.MstnPlatformNET;
using MSAddinTest.Core.Loader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSAddinTest.Core.Command
{
    /// <summary>
    /// 初始化时加载插件
    /// </summary>
    internal class LoadPluginsWhenStartupCommand : CommandBase
    {
        public override FuncResult Start()
        {
            // 读取所有插件需要自动加载的插件
            var loaderSetups = PluginSetting.GetAutoLoadLoaderSetups();
            foreach (var setup in loaderSetups)
            {
                var loader = new PluginAssemblyLoader(setup);
                var loaderRes = loader.LoadAssembly();
                if (loaderRes.NotOk)
                {
                    continue;
                }

                PluginContainer.Add(loader);
                MessageCenter.Instance.ShowInfoMessage($"{loader.Setup.PluginName} 已自动加载!", "", false);
            }

            return new FuncResult(true);
        }
    }
}
