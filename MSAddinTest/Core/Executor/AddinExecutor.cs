using MSAddinTest.MSTestInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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
            try
            {
                MethodInfo.Invoke(null, new object[] { arg });
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }
    }
}
