﻿using Bentley.MstnPlatformNET;
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

        public override FuncResult Start()
        {
            if (PluginContainer.TryGetValue(_pluginName, out var container))
            {
                container.Reload();
                MessageCenter.Instance.ShowInfoMessage($"{_pluginName} 重载成功！", "", false);
            }

            return new FuncResult(true);
        }
    }
}
