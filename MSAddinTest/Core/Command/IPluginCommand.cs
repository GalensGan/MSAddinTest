using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSAddinTest.Core.Command
{
    internal interface IPluginCommand
    {
        void Init(PluginDomains pluginDomains, Settings.PluginSetting pluginSetting);

        object Start();
    }
}
