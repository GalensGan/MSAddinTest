using MSAddinTest.MSTestInterface;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MSAddinTest.Core.Loader
{
    public partial class PluginAssemblyLoader
    {
        /// <summary>
        /// 实例化执行对象并执行
        /// </summary>
        /// <param name="nameWithParams"></param>
        /// <returns></returns>
        public FuncResult Execute(string nameWithParams)
        {
            var nameTemp = nameWithParams.Trim().ToLower();
            // 对名称进行匹配
            // 如果是 keyin，通过匹配前缀是否为 keyin 来确定
            // 名称不区分大小写
            var executors = _executors.FindAll(x => x.IsMatch(nameWithParams, out _, out _));

            foreach (var executor in executors)
            {
                // 获取参数
                executor.IsMatch(nameWithParams, out var executorName, out var strArg); 
                executor.Execute(strArg.Trim());
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
            LoadAssembly();
        }
    }
}
