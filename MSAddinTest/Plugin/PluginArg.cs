using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSAddinTest.Plugin
{
    /// <summary>
    /// 用于主子域间传递参数
    /// </summary>
    public class PluginArg:MarshalByRefObject
    {
        public Addin Addin { get; set; } = new Addin();
    }

    public class Addin
    {
        public string Name { get; set; }    
    }
}
