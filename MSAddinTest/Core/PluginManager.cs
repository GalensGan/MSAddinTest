using MSAddinTest.Core.Command;
using MSAddinTest.Core.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSAddinTest.Core
{
    /// <summary>
    /// 插件管理器
    /// </summary>
    internal class PluginManager: ICommandInvoker
    {
        public static PluginManager Manager { get; private set; }

        private object _args;

        /// <summary>
        /// 插件设置
        /// </summary>
        private PluginSetting _pluginSetting;

        private PluginDomainContainer _pluginDomains = new PluginDomainContainer();

        public PluginManager(object args)
        {
            Manager = this;
            _args = args;

            // 读取设置
            _pluginSetting = new PluginSetting(_args);
        }

        #region 插件相关
        /// <summary>
        /// 调用命令
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public FuncResult InvokeCommand(IPluginCommand command)
        {
            // 调用插件
            command.Init(_pluginDomains, _pluginSetting);
            return command.Start();
        }
        #endregion
    }
}
