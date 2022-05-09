using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSAddinTest.Core.Command
{
    /// <summary>
    /// 命令执行接口
    /// </summary>
    internal interface ICommandInvoker
    {
        object InvokeCommand(IPluginCommand command); 
    }
}
