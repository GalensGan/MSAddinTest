using Bentley.MstnPlatformNET;
using MSAddinTest.Core.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSAddinTest.Core.Command
{
    /// <summary>
    /// 卸载插件
    /// </summary>
    internal class UnloadPluginCommand : CommandBase
    {
        public UnloadPluginCommand(string pluginName)
        {
            _pluginNameToUnload = pluginName;
        }
        /// <summary>
        /// 待卸载的插件名称
        /// </summary>
        private readonly string _pluginNameToUnload;

        public override FuncResult Start()
        {
            // 移除插件域,让 GC 回收
            PluginContainer.Remove(_pluginNameToUnload);

            // 从存储中去掉记录
            PluginSetting.RemovePluginSetting(_pluginNameToUnload);

            MessageCenter.Instance.ShowInfoMessage($"{_pluginNameToUnload} 已卸载!", "", false);
            return new FuncResult(true);
        }
    }
}
