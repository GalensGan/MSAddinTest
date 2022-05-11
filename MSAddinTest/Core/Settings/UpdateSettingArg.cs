using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSAddinTest.Core.Settings
{
    public class UpdateSettingArg
    {
        /// <summary>
        /// 插件名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 字段名
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; }

        public void UpdateSetting(JToken jToken)
        {
            if (jToken == null) return;

            // 判断数据类型
            if (double.TryParse(Value,out var number))
            {
                jToken[FieldName] = number;
                return;
            }

            if(bool.TryParse(Value,out var boolValue))
            {
                jToken[FieldName] = boolValue;
                return;
            }

            jToken[FieldName] = Value;
        }
    }
}
