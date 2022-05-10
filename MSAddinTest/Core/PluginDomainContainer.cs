using MSAddinTest.Core.Loader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSAddinTest.Core
{
    /// <summary>
    /// 保存
    /// </summary>
    internal class PluginDomainContainer : Dictionary<string, PluginAssemblyLoader>
    {
        public new PluginAssemblyLoader this[string name]
        {
            get
            {
                if (TryGetValue(name, out PluginAssemblyLoader loader)) return loader;

                return null;
            }
        }

        public void Add(PluginAssemblyLoader domainLoader)
        {
            Add(domainLoader.Setup.PluginName, domainLoader);
        }
    }
}
