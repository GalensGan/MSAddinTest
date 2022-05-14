using Bentley.MstnPlatformNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSAddinTest.MSTestInterface
{
    /// <summary>
    /// Addin 插件
    /// </summary>
    public abstract class MSTest_Addin : IMSTest
    {
        protected MSTest_Addin(IntPtr mdlDescriptor) { }

        protected abstract int Run(string[] commandLine);

        /// <summary>
        /// 子类用于初始化
        /// </summary>
        /// <param name="addinArg"></param>
        public virtual void Init(AddIn addIn) { }

        /// <summary>
        /// 卸载后手动清除向 Addin 中添加的一些事件
        /// </summary>
        public virtual void Unloaded() { }
    }
}
