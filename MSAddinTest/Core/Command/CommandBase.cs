using MSAddinTest.Core.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSAddinTest.Core.Command
{
    /// <summary>
    /// 将 init 的通用代码抽象到父类中
    /// </summary>
    internal abstract class CommandBase: IPluginCommand
    {
        protected PluginDomains PluginDomains { get; private set; }
        protected PluginSetting PluginSetting { get; private set; }

        public void Init(PluginDomains pluginDomains, PluginSetting pluginSetting)
        {
            PluginDomains = pluginDomains;
            PluginSetting = pluginSetting;
        }

        public abstract object Start();
    }
}
