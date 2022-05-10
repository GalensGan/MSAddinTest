using MSAddinTest.MSTestInterface;
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
    [MSTest(Name = "class", Description = "测试 IClassPlugin 插件")]
    internal class TestClassExecutor : IMSTest_Class
    {
        public void Execute(IMSTestArg arg)
        {
            MessageBox.Show("IClassPlugin 被调用了!");
        }
    }
}
