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
    public class Test : IClassPlugin
    {
        public object Execute(PluginArg arg)
        {
            string msg1 = "插件运行了！";
            string msg2 = "插件被修改了~";
            MessageBox.Show(msg1);
            return true;
        }
    }
}
