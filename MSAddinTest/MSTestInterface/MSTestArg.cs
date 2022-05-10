using Bentley.MstnPlatformNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSAddinTest.MSTestInterface
{
    /// <summary>
    /// 执行测试器传递的参数
    /// </summary>
    public class MSTestArg:IMSTestArg
    {
        public string UnparsedParams { get; set; }
    }
}
