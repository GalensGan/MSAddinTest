using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSAddinTest.MSTestInterface
{
    /// <summary>
    /// 命令间传递的参数接口
    /// </summary>
    public interface IMSTestArg
    {
        string UnparsedParams { get; set; }
    }
}
