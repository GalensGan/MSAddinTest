using MSAddinTest.PluginInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSAddinTest.Core.Executor
{
    /// <summary>
    /// 静态方法执行器
    /// </summary>
    internal class StaticMethodExecutor : ExecutorBase
    {
        public StaticMethodExecutor(Type type) : base(type)
        {
        }

        public override object Execute(PluginArg plugin)
        {
            throw new NotImplementedException();
        }
    }
}
