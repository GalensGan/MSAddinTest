using MSAddinTest.MSTestInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TestPlugin
{
    [MSTest(Name ="test",Description ="test description")]
    public class Test : IMSTest_Class
    {
        public void Execute(IMSTestArg arg)
        {
            string msg1 = "插件运行了2！";
            string msg2 = "插件被修改了~";
            MessageBox.Show(msg1);
        }


        public static void ReferenceInvoke()
        {
            string msg1 = "我被引用，我被调用了2~";
            MessageBox.Show(msg1);
        }
    }

   
}
