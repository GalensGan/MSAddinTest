using Bentley.MstnPlatformNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAddinPlugin.TestAddin
{
    [AddIn(MdlTaskID = "TestAddinPlugin")]
    internal class PluginAddin : AddIn
    {
        public PluginAddin(IntPtr mdlDescriptor) : base(mdlDescriptor)
        {

        }

        protected override int Run(string[] commandLine)
        {
            return 0;
        }
    }
}
