using MSAddinTest.MSTestInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MSAddinTest.Core.Executor
{
    /// <summary>
    /// 类执行器
    /// </summary>
    internal class ClassExecutor : ExecutorBase
    {
        public ClassExecutor(Type type) : base(type)
        {
            // 获取Plugin特定
            var pluginAttr = type.GetCustomAttributes(typeof(MSTestAttribute)).FirstOrDefault();
            if (pluginAttr is MSTestAttribute attr)
            {
                Names.Add(attr.Name);
                Description = attr.Description;
            }
            else
            {
                // 如果没有特性，就使用类名
                Names.Add(type.Name);
            }
        }

        public override void Execute(IMSTestArg pluginArg)
        {
            var instance = Activator.CreateInstance(Type, BindingFlags.Public | BindingFlags.NonPublic);
            if (instance is IMSTest_Class plugin)
            {
                plugin.Execute(pluginArg);
            }
        }
    }
}
