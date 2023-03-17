using Bentley.MstnPlatformNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Bentley.MstnPlatformNET.AddIn;

namespace MSAddinTest.MSTestInterface
{
    /// <summary>
    /// Addin 插件
    /// </summary>
    public abstract class MSTest_Addin : IMSTest
    {
        protected MSTest_Addin(IntPtr mdlDescriptor)
        {           
        }

        public virtual void Init(AddIn addIn)
        {
            Run(new string[] { });
        }

        protected abstract int Run(string[] commandLine);

        /// <summary>
        /// 卸载后手动清除向 Addin 中注册的一些事件
        /// </summary>
        protected virtual void OnUnloaded(UnloadedEventArgs eventArgs) { }

        /// <summary>
        /// 发送卸载完成通知
        /// </summary>
        /// <param name="eventArgs"></param>
        public void NotifyOnUnloaded(UnloadedEventArgs eventArgs) => OnUnloaded(eventArgs);

        /// <summary>
        /// 隐式转换成 addin
        /// </summary>
        /// <param name="testAddin"></param>
        public static implicit operator AddIn(MSTest_Addin testAddin)
        {
            return Index.MSAddin.Instance;
        }
    }
}
