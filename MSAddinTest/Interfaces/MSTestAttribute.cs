using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSAddinTest.Interfaces
{
    /// <summary>
    /// Plugin 特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class MSTestAttribute : Attribute
    {
        /// <summary>
        /// 内部会将 name.Trim() 作为 Name
        /// </summary>
        /// <param name="name"></param>
        public MSTestAttribute(string name)
        {
            Name = name.Trim();
        }

        /// <summary>
        /// 必须是全局唯一的
        /// </summary>
        public string Name { get;private set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
    }
}
