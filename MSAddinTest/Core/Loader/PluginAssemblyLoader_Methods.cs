using MSAddinTest.MSTestInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSAddinTest.Core.Loader
{
    public partial class PluginAssemblyLoader
    {
        /// <summary>
        /// 实例化执行对象并执行
        /// </summary>
        /// <param name="name"></param>
        /// <param name="arg"></param>
        /// <returns></returns>
        public FuncResult Execute(string name, IMSTestArg arg)
        {
            var nameTemp = name.Trim().ToLower();
            // 对名称进行匹配
            // 如果是 keyin，通过匹配前缀是否为 keyin 来确定
            // 名称不区分大小写
            var executors = _executors.FindAll(x => nameTemp.StartsWith(x.Name.ToLower()));

            foreach (var executor in executors)
            {
                // 获取参数
                string strArg = name.Substring(nameTemp.IndexOf(executor.Name) + 1);
                if (!string.IsNullOrEmpty(strArg))
                {
                    arg.UnparsedParams = strArg.Trim();
                }

                executor.Execute(arg);
            }

            return new FuncResult(true)
            {
                Data = executors.Count,
            };
        }

        /// <summary>
        /// 重新加载
        /// </summary>
        public void Reload()
        {
            // 通过hash值对比文件是否更改

            LoadAssembly();
        }
    }
}
