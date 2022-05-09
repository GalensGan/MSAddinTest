using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSAddinTest.Core.Executor
{
    /// <summary>
    /// 类执行器
    /// </summary>
    internal class ClassExecutor: IExecutor
    {
        /// <summary>
        /// 类型
        /// </summary>
        public Type Type { get; set; }

        /// <summary>
        /// 执行器名称
        /// 通过类的特性来获取，如果没有特性，就为类型名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
    }
}
