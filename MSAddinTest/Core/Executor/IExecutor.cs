using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSAddinTest.Core.Executor
{
    public interface IExecutor
    {
        /// <summary>
        /// 类型
        /// </summary>
         Type Type { get; set; }

        /// <summary>
        /// 执行器名称
        /// 通过类的特性来获取，如果没有特性，就为类型名称
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        string Description { get; set; }
    }
}
