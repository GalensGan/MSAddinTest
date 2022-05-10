using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSAddinTest.PluginInterface
{
    /// <summary>
    /// Addin 插件
    /// </summary>
    public abstract class AddinPlugin:IPlugin
    {
        // 要进行初始化
        public abstract void Init(PluginArg pluginArg);
    }
}
