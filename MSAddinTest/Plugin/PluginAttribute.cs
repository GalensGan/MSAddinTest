using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSAddinTest.Plugin
{
    /// <summary>
    /// Plugin特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class PluginAttribute:Attribute
    {
        /// <summary>
        /// 必须是全局唯一的
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
    }
}
