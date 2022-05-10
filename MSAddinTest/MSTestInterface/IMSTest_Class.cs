using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSAddinTest.MSTestInterface
{
    /// <summary>
    /// 类执行器的插件接口
    /// </summary>
    public interface IMSTest_Class:IMSTest
    {
        /// <summary>
        /// 执行插件方法
        /// </summary>
        /// <param name="arg">用于初始化的参数</param>
        /// <returns>执行结果json串</returns>
        void Execute(IMSTestArg arg);
    }
}
