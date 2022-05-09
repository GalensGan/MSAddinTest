using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSAddinTest.Plugin
{
    /// <summary>
    /// addin 父类
    /// </summary>

    public abstract class AddinPlugin : IPlugin
    {
        public object Execute(PluginArg arg)
        {
            throw new NotImplementedException();
        }

        public bool Init(PluginArg arg)
        {
            throw new NotImplementedException();
        }
    }
}
