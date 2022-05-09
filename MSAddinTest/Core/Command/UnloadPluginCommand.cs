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
        #region 卸载参数
        /// <summary>
        /// 待卸载的插件名称
        /// </summary>
        public string PluginNameToUnload { get; set; }
        #endregion

        public override object Start()
        {
            var pluginDomainLoader = PluginDomains[PluginNameToUnload];
            if (pluginDomainLoader == null) return StatusCode.NotFound;

            // 卸载插件
            pluginDomainLoader.Unload();

            // 移除插件域,让 GC 回收
            PluginDomains.Remove(PluginNameToUnload);

            return StatusCode.Success;
        }
    }
}
