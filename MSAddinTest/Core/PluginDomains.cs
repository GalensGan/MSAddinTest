using MSAddinTest.Core.DomainLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSAddinTest.Core
{
    /// <summary>
    /// 保存插件子程序域
    /// </summary>
    internal class PluginDomains : Dictionary<string, PluginDomainLoader>
    {
        public new PluginDomainLoader this[string name]
        {
            get
            {
                if (TryGetValue(name, out PluginDomainLoader loader)) return loader;

                return null;
            }
        }

        public void Add(PluginDomainLoader domainLoader)
        {
            Add(domainLoader.PluginDomainSetup.PluginName, domainLoader);
        }
    }
}
