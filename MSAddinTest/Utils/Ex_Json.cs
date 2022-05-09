using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSAddinTest.Utils
{
    /// <summary>
    /// json 扩展方法
    /// </summary>
    internal static class Ex_Json
    {
        /// <summary>
        /// 通过json表达式获取值，如果不存在，则返回默认值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jToken"></param>
        /// <param name="path"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T SelectTokenOrDefault<T>(this JToken jToken, string path, T defaultValue)
        {
            var value = jToken.SelectToken(path, false);
            if (value == null) return defaultValue;

            return value.ToObject<T>();
        }
    }
}
