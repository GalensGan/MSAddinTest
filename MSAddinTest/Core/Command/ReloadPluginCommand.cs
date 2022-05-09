using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSAddinTest.Core.Command
{
    /// <summary>
    /// 重新加载插件
    /// </summary>
    internal class ReloadPluginCommand : CommandBase
    {
        private string _pluginName;
        public ReloadPluginCommand(string pluginName)
        {
            _pluginName = pluginName;
        }

        public override object Start()
        {
            if (PluginDomains.TryGetValue(_pluginName, out var pluginDomain)) pluginDomain.Reload();

            return true;
        }
    }
}
