using MSAddinTest.PluginInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSAddinTest.Core.Executor
{
    /// <summary>
    /// addin 执行器
    /// </summary>
    internal class AddinExecutor : ExecutorBase
    {
        public AddinExecutor(Type type) : base(type) { }

        
        public override object Execute(PluginArg plugin)
        {
            // 获取命令表

            // 通过命令表获取静态方法

            // 调用静态方法

            return true;
        }
    }
}
