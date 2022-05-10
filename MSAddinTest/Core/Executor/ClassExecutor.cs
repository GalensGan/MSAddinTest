using MSAddinTest.MSTestInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSAddinTest.Core.Executor
{
    /// <summary>
    /// 类执行器
    /// </summary>
    internal class ClassExecutor : ExecutorBase
    {
        public ClassExecutor(Type type) : base(type) { }

        public override void Execute(IMSTestArg pluginArg)
        {
            var instance = Activator.CreateInstance(Type);
            if (instance is IMSTest_Class plugin)
            {
                plugin.Execute(pluginArg);
            }
        }
    }
}
