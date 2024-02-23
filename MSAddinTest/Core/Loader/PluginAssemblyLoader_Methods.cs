using Bentley.MstnPlatformNET;
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
            // 使用正则表达式将多个空格转换成一个空格
            nameWithParams = System.Text.RegularExpressions.Regex.Replace(nameWithParams, @"\s+", " ");
            var nameTemp = nameWithParams.Trim().ToLower();
            // 对名称进行匹配
            // 如果是 keyin ，通过匹配前缀是否为 keyin 来确定
            // 名称不区分大小写
            var executors = _executors.FindAll(x => x.IsMatch(nameTemp, out _, out _));

            foreach (var executor in executors)
            {
                // 获取参数
                executor.IsMatch(nameTemp, out var executorName, out var strArg);
                executor.Execute(strArg);
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
            var reloadResult = LoadAssembly();
            if (reloadResult)
            {
                // 提示成功               
                MessageCenter.Instance.ShowInfoMessage($"{Setup.PluginName} 插件重载成功!", "", false);
            }
        }
    }
}
