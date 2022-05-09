using MSAddinTest.Utils;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSAddinTest.Core.Settings
{
    /// <summary>
    /// 设置
    /// </summary>
    internal class PluginSetting
    {
        public JObject JsonConfig { get; private set; }
        public PluginSetting(object addin)
        {
            // 监听Ord关闭事件，关闭之前要保存配置


            // 从指定目录读取配置文件
            Read();
        }

        private void Read()
        {
            // 从本地读取设置
            JsonConfig = new JObject();
        }

        /// <summary>
        /// 保存配置
        /// </summary>
        public void Save()
        {
            // 保存设置到本地
        }

        #region 设置属性
        /// <summary>
        /// 是否全局自动重加载
        /// </summary>
        public bool IsGlobalAutoReload
        {
            get
            {
                return JsonConfig.SelectTokenOrDefault("isAutoReload", true);
            }
        }

        // 添加插件
        public void AddPlugin(string pluginName, string dllFullPath, bool isAutoReload)
        {
            // 获取插件目录
            JArray plugins = JsonConfig.SelectTokenOrDefault<JArray>("plugins",null);
            if (plugins == null)
            {
                // 添加配置
                plugins = new JArray();
                JsonConfig.Add("plugins", plugins);
            }

            // 查找是否已经存在配置
            JToken pluginObj = plugins.FirstOrDefault(p => p["pluginName"].ToString() == pluginName);

            var newPlugin = new JObject()
            {
              new JProperty("pluginName",pluginName),
              new JProperty("dllFullPath",dllFullPath),
              new JProperty("isAutoReload",isAutoReload)
            };
            if (pluginObj != null) pluginObj.Replace(newPlugin);
            else plugins.Add(newPlugin);
        }
        #endregion
    }
}
