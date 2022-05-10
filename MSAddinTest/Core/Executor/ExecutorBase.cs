using MSAddinTest.PluginInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSAddinTest.Core.Executor
{
    /// <summary>
    /// 主程序可以获取数据
    /// </summary>
    public abstract class ExecutorBase:MarshalByRefObject
    {
        public ExecutorBase(Type type)
        {
            Type = type;
        }

        /// <summary>
        /// 类型
        /// </summary>
        protected Type Type { get; private set; }

        /// <summary>
        /// 执行器名称
        /// 通过类的特性来获取，如果没有特性，就为类型名称/静态方法名称
        /// </summary>
       public string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
       public string Description { get; set; }

      public abstract  object Execute(PluginArg plugin);
    }
}
