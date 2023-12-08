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
    /// 传入此类的方法必须是绑定了特性的
    /// </summary>
    internal class StaticMethodExecutor : ExecutorBase
    {
        public override int Priority { get; } = 50;

        protected MethodInfo MethodInfo { get; private set; }
        public StaticMethodExecutor(MethodInfo methodInfo) : base(null)
        {
            SetMthodInfo(methodInfo);
        }

        public StaticMethodExecutor(Type type, string methodName) : base(type)
        {
            // 获取 MethodInfo
            var methodInfo = type.GetMethod(methodName, BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);

            // 如果为空，提示异常
            if(MethodInfo==null) throw new NullReferenceException($"无法在 {type} 中找到静态方法 {methodName}");

            SetMthodInfo(methodInfo);
        }

        /// <summary>
        /// 在此处解析方法上的特性
        /// 如果有特性，用特性中的值覆盖
        /// </summary>
        /// <param name="methodInfo"></param>
        private void SetMthodInfo(MethodInfo methodInfo)
        {
            MethodInfo = methodInfo;

            // 保存类型
            Type = MethodInfo.DeclaringType;

            // 获取静态方法上面的特性
            var attribute = methodInfo.GetCustomAttributes(typeof(MSTestAttribute)).FirstOrDefault();
            if (attribute is MSTestAttribute attr)
            {
                Names.Add(attr.Name);
                Description = attr.Description;
            }
        }

        public override void Execute(string arg)
        {
            if (MethodInfo == null) return;

            // 调用静态方法
            MethodInfo.Invoke(null, new object[] { arg });
        }
    }
}
