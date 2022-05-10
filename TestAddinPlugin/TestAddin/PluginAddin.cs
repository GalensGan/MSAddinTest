using Bentley.MstnPlatformNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MSAddinTest.MSTestInterface;

namespace TestAddinPlugin.TestAddin
{
    [AddIn(MdlTaskID = "TestAddinPlugin")]
    internal class PluginAddin : MSTest_Addin
    {
        
        public PluginAddin(IntPtr mdlDescriptor) : base(mdlDescriptor)
        {

        }

        public override void Init(AddIn addIn)
        {
            Run(new string[] { });
        }

        protected override int Run(string[] commandLine)
        {
            return 0;
        }
    }
}
