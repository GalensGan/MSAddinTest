using Bentley.MstnPlatformNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MSAddinTest.Index
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

           MessageBox.Show("MSAddinTest");

            return 0;
        }
    }
}
