using MSAddinTest.PluginInterface;
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

        public override object Execute(PluginArg pluginArg)
        {
            var instance = Activator.CreateInstance(Type);
            if (instance is IClassPlugin plugin)
            {
                return plugin.Execute(pluginArg);
            }

            return false;
        }
    }
}
