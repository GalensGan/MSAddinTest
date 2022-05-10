﻿using MSAddinTest.Core;
using MSAddinTest.Core.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSAddinTest.Index
{
    internal class KeyinFuncs
    {
        /// <summary>
        /// 加载插件
        /// </summary>
        /// <param name="unparsed"></param>
        public static void LoadPlugin(string unparsed)
        {
            var cmd = new LoadPluginCommand();
            PluginManager.Manager.InvokeCommand(cmd);
        }

        /// <summary>
        /// 卸载插件
        /// </summary>
        /// <param name="unparsed"></param>
        public static void UnloadPlugin(string unparsed)
        {
            var cmd = new UnloadPluginCommand(unparsed);
            PluginManager.Manager.InvokeCommand(cmd);
        }

        /// <summary>
        /// 重载插件
        /// </summary>
        /// <param name="unparsed"></param>
        public static void ReloadPlugin(string unparsed)
        {
            var cmd = new ReloadPluginCommand(unparsed);
            PluginManager.Manager.InvokeCommand(cmd);
        }

        /// <summary>
        /// 测试插件
        /// </summary>
        public static void TestPlugin(string unparsed)
        {
            var cmd = new RunPluginCommand(unparsed, new PluginInterface.PluginArg());
            PluginManager.Manager.InvokeCommand(cmd);
        }
    }
}