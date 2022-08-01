using Bentley.MstnPlatformNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MSAddinTest.MSTestInterface;

namespace TestAddinPlugin.TestAddin
{
    /// <summary>
    /// 测试 addin 调试
    /// 1. 继承抽象类 MSTest_Addin
    /// 2. 在 Init 中获取传递的 addin 供当前库使用
    /// </summary>
    [AddIn(MdlTaskID = "TestAddinPlugin")]
    internal class PluginAddin : MSTest_Addin
    {
        public static AddIn Instance { get; private set; }
        public PluginAddin(IntPtr mdlDescriptor) : base(mdlDescriptor)
        {

        }

        protected override void Init(AddIn addin)
        {
            Instance = addin;
            Run(new string[] { });
        }

        protected override int Run(string[] commandLine)
        {
            return 0;
        }
    }
}
