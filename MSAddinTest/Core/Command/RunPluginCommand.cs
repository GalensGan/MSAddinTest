using MSAddinTest.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSAddinTest.Core.Command
{
    /// <summary>
    /// 执行插件
    /// </summary>
    internal class RunPluginCommand : CommandBase
    {
        private PluginArg _args;
        private string _executorName;
        /// <summary>
        /// 传入执行器名称和参数
        /// </summary>
        /// <param name="excutorName"></param>
        /// <param name="args"></param>
        public RunPluginCommand(string excutorName, PluginArg args)
        {
            _executorName = excutorName;
            _args = args;
        }
        public override object Start()
        {
            foreach (var kv in PluginDomains)
            {
                kv.Value.Execute(_executorName, _args);
            }

            return true;
        }
    }
}
