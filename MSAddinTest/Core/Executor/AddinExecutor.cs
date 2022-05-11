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
        public AddinExecutor(Type type, string methodName) : base(type, methodName)
        {
            Description = "Keyin";
        }

        public override void Execute(IMSTestArg pluginArg)
        {
            if (MethodInfo == null) return;

            // 调用静态方法
            try
            {
                string unparsedParams = string.Empty;
                if (pluginArg is MSTestArg arg) unparsedParams = arg.UnparsedParams;
                MethodInfo.Invoke(null, new object[] { unparsedParams });

            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }
    }
}
