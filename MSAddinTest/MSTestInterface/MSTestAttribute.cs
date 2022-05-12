using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSAddinTest.MSTestInterface
{
    /// <summary>
    /// Plugin特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class MSTestAttribute : Attribute
    {
        public MSTestAttribute(string name)
        {
            Name = name;
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
