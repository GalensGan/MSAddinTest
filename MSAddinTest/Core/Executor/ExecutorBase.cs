using MSAddinTest.MSTestInterface;
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
    public abstract class ExecutorBase : MarshalByRefObject
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
        /// 名称可以有多个，对于 addin，包含 keyin 和特性定义的名称
        /// </summary>
        public List<string> Names { get; set; } = new List<string>();

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        public abstract void Execute(IMSTestArg plugin);

        /// <summary>
        /// 通过输入字符串匹配当前执行器
        /// </summary>
        /// <param name="inputStr"></param>
        /// <param name="name"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public bool IsMatch(string inputStr, out string name,out string args)
        {
            args = inputStr;
            name = Names.Find(x=>inputStr.ToLower().StartsWith(x.ToLower()));
            if (name == null) return false;

            // 生成参数
            args = inputStr.Substring(inputStr.IndexOf(name) + 1);

            return true;
        }
    }
}
