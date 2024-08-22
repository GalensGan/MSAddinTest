using System;
using System.Reflection;

namespace MSAddinTest.Core.Executor
{
    internal class AddinExecutor : StaticMethodExecutor
    {
        public override int Priority { get; } = 100;

        public AddinExecutor(Type type, string methodName) : base(type, methodName)
        {
            Description = "Keyin";
        }

        public override void Execute(string arg)
        {
            if (MethodInfo == null) return;

            // 调用静态方法
            MethodInfo.Invoke(null, new object[] { arg });
        }
    }
}
