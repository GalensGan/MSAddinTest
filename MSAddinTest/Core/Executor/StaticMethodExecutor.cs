using MSAddinTest.PluginInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MSAddinTest.Core.Executor
{
    /// <summary>
    /// 静态方法执行器
    /// </summary>
    internal class StaticMethodExecutor : ExecutorBase
    {
        private MethodInfo _methodInfo;
        public StaticMethodExecutor(Type type, MethodInfo methodInfo) : base(type)
        {
            _methodInfo = methodInfo;
        }

        public override object Execute(PluginArg plugin)
        {
            if (_methodInfo == null) return false;

            // 创建对象实例
            var instance = Activator.CreateInstance(Type);

            // 调用静态方法
            _methodInfo.Invoke(instance, new object[] { plugin });

            return true;
        }
    }
}
