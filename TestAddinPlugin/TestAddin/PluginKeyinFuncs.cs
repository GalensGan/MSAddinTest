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
        public static void TestElement(string unparsed)
        {
           MessageBox.Show("我是 addin,我被调用了");
        }

        public static void TestAddin(string unparsed)
        {

        }
    }
}
