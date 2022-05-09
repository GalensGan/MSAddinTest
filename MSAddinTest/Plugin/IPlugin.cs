using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSAddinTest.Plugin
{
    /// <summary>
    /// 插件接口
    /// 继承该接口必须有一个无参的构造函数
    /// </summary>
    public interface IPlugin
    {
        /// <summary>
        /// 执行插件方法
        /// </summary>
        /// <param name="arg">用于初始化的参数</param>
        /// <returns>执行结果json串</returns>
        object Execute(PluginArg arg);

        /// <summary>
        /// 插件初始化
        /// </summary>
        /// <returns></returns>
        bool Init(PluginArg arg);
    }
}
