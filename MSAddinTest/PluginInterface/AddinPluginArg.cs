using Bentley.MstnPlatformNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSAddinTest.PluginInterface
{
    public class AddinPluginArg:PluginArg
    {
        public string UnparsedParams { get; set; }

        public AddIn AddIn { get; set; }
    }
}
