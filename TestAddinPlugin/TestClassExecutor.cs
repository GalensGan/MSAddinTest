using MSAddinTest.PluginInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace TestAddinPlugin
{
    /// <summary>
    /// 测试类执行器
    /// </summary>
    [Plugin(Name = "class", Description = "测试 IClassPlugin 插件")]
    internal class TestClassExecutor : IClassPlugin
    {
        public object Execute(PluginArg arg)
        {
            MessageBox.Show("IClassPlugin 被调用了!");
            return true;
        }
    }
}
