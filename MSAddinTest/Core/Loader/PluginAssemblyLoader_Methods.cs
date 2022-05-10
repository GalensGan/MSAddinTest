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
            // 名称不区分大小写
            var executors = _executors.FindAll(x => x.Name.ToLower() == name.ToLower());

            foreach (var executor in executors)
            {
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
