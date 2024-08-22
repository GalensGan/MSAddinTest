using Bentley.MstnPlatformNET;
using MSAddinTest.Core.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MSAddinTest.Startup
{
    /// <summary>
    /// Microstation Addin 入口
    /// </summary>
    [AddIn(MdlTaskID = "MSAddinTest")]
    public class MSAddin : AddIn
    {
        public static AddIn Instance { get; private set; }
        
        public MSAddin(IntPtr mdlDescriptor) : base(mdlDescriptor)
        {
            Instance = this;
        }

        protected override int Run(string[] commandLine)
        {
            // 初始化插件管理器
            new Core.PluginManager(this);

            MessageCenter.Instance.ShowInfoMessage("MSAddintTest 加载成功!","",false);

            // 加载自启动插件
            var cmd = new LoadPluginsWhenStartupCommand();
            Core.PluginManager.Manager.InvokeCommand(cmd);

            return 0;
        }
    }
}
