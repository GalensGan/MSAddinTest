using MSAddinTest.MSTestInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestAddinPlugin.TestAddin
{
    internal class PluginKeyinFuncs
    {
        [MSTest("element",Description ="这是keyin别名")]
        public static void TestElement(string unparsed)
        {
           MessageBox.Show("我是 keyin,我被调用了");
        }

        public static void TestAddin(string unparsed)
        {
            MessageBox.Show("我是纯 keyin,且我没有被 MSTest 标记,我被调用了");
        }
    }
}
