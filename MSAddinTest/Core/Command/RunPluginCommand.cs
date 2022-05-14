using Bentley.MstnPlatformNET;
using MSAddinTest.MSTestInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MSAddinTest.Core.Command
{
    /// <summary>
    /// 执行插件
    /// </summary>
    internal class RunPluginCommand : CommandBase
    {
        private IMSTestArg _args;
        private string _executorName;

        /// <summary>
        /// 用户端初始化时，传入执行器的名称和执行器的初始化参数
        /// </summary>
        /// <param name="excutorName"></param>
        /// <param name="args"></param>
        public RunPluginCommand(string excutorName)
        {
            _executorName = excutorName;
            _args = new MSTestArg();
        }

        /// <summary>
        /// 特性控制可以跨程序集捕获异常
        /// </summary>
        /// <returns></returns>
        //[HandleProcessCorruptedStateExceptions,SecurityCritical]
        public override FuncResult Start()
        {
            int executorsCount = 0;

            // 全部包裹在 TryCatch中
            try
            {
                foreach (var kv in PluginContainer)
                {
                    var result = kv.Value.Execute(_executorName, _args);
                    if (result.Data is int count) executorsCount += count;
                }
            }
            catch (Exception ex)
            {
                MessageCenter.Instance.ShowErrorMessage(ex.Message, ex.StackTrace, true);
            }

            if (executorsCount == 0)
            {
                MessageBox.Show($"未找到名为{_executorName}的执行器");
            }

            return new FuncResult(executorsCount > 0);
        }
    }
}
