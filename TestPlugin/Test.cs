using MSAddinTest.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TestPlugin
{
    [Plugin(Name ="test",Description ="test description")]
    public class Test : IPlugin
    {
        public object Execute(PluginArg arg)
        {
            MessageBox.Show("插件运行了！"+arg);
            return true;
        }

        public bool Init(PluginArg arg)
        {
            return true;
        }
    }
}
