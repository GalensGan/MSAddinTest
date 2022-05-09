using MSAddinTest.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSAddinTest.Core.Executor
{
    public abstract class ExecutorBase
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
        /// 通过类的特性来获取，如果没有特性，就为类型名称
        /// </summary>
       public string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
       public string Description { get; set; }

      public abstract  object Execute(PluginArg plugin);
    }
}
