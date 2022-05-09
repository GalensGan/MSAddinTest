﻿using MSAddinTest.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private PluginArg _args;
        private string _executorName;

        /// <summary>
        /// 用户端初始化时，传入执行器的名称和执行器的初始化参数
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
            int executorsCount = 0;
            foreach (var kv in PluginDomains)
            {
                var result = kv.Value.Execute(_executorName, _args);
                if (result is int count) executorsCount += count;
            }

            if(executorsCount == 0)
            {
                MessageBox.Show($"未找到名为{_executorName}的执行器");
            }

            return executorsCount>0;
        }
    }
}
