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
    /// 静态方法执行器
    /// </summary>
    internal class StaticMethodExecutor : ExecutorBase
    {
        protected MethodInfo MethodInfo { get; private set; }
        public StaticMethodExecutor(Type type, MethodInfo methodInfo) : base(type)
        {
            MethodInfo = methodInfo;
        }

        public StaticMethodExecutor(Type type,string methodName):base(type)
        {
            // 获取 MethodInfo
            MethodInfo = type.GetMethod(methodName);
        }

        public override void Execute(IMSTestArg plugin)
        {
            if (MethodInfo == null) return;

            // 创建对象实例
            var instance = Activator.CreateInstance(Type);

            // 调用静态方法
            try
            {
                MethodInfo.Invoke(instance, new object[] { plugin });

            }catch(Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }
    }
}
