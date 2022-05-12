using Bentley.DgnPlatformNET;
using MSAddinTest.Core.Loader;
using MSAddinTest.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSAddinTest.Core.Settings
{
    /// <summary>
    /// 设置
    /// </summary>
    public class PluginSetting
    {
        private JObject _jsonConfig;
        private string _savePath;

        public PluginSetting(object addin)
        {
            _savePath = Path.Combine(ConfigurationManager.GetVariable("_USTN_HOMEROOT"), $@"MSAddinTest\config.json");
            // 监听Ord关闭事件，关闭之前要保存配置
            Index.MSAddin.Instance.ExitDesignFileStateEvent += Instance_ExitDesignFileStateEvent;

            // 从指定目录读取配置文件
            Read();
        }

        private void Instance_ExitDesignFileStateEvent(Bentley.MstnPlatformNET.AddIn sender, EventArgs eventArgs)
        {
            // 退出文件时，保存配置
            Save();
        }

        private void Read()
        {
            // 从本地读取设置
            // 保存的位置位于用户目录          
            if (!File.Exists(_savePath))
            {
                _jsonConfig = new JObject();
                return;
            }

            // 从文件中读取
            var content = File.ReadAllText(_savePath);
            if (string.IsNullOrEmpty(content))
            {
                _jsonConfig = new JObject();
                return;
            }

            _jsonConfig = JObject.Parse(content);
        }

        /// <summary>
        /// 保存配置
        /// </summary>
        public void Save()
        {
            // 保存设置到本地
            Directory.CreateDirectory(Path.GetDirectoryName(_savePath));
            using (var fileStream = File.Create(_savePath))
            {
                var sr = new StreamWriter(fileStream);
                var content = JsonConvert.SerializeObject(_jsonConfig, Formatting.Indented);
                sr.Write(content);
                sr.Close();
            }
        }

        #region 属性更改相关逻辑
        public event Action<UpdateSettingArg> SettingChanged;
        private void TriggerSettingChangedEvent(UpdateSettingArg arg)
        {
            SettingChanged?.Invoke(arg);
        }
        #endregion

        #region 设置属性       

        // 添加插件
        public void AddPlugin(string pluginName, string dllFullPath, bool isAutoReload)
        {
            // 获取插件目录
            JArray plugins = _jsonConfig.SelectTokenOrDefault<JArray>("plugins", null);
            if (plugins == null)
            {
                // 添加配置
                plugins = new JArray();
                _jsonConfig.Add("plugins", plugins);
            }

            // 查找是否已经存在配置
            JToken pluginObj = plugins.SelectToken($"[?(@.name=='{pluginName}')]");

            if (pluginObj == null) plugins.Add(new JObject()
            {
              new JProperty("name",pluginName),
              new JProperty("dllFullPath",dllFullPath),
              new JProperty("autoReload",isAutoReload),
              new JProperty("autoLoad",false),
            });
        }

        /// <summary>
        /// 更新插件设置
        /// </summary>
        /// <param name="settingParams"></param>
        public void UpdatePluginSetting(UpdateSettingArg settingParams)
        {
            // 更新插件设置
            var plugins = _jsonConfig.SelectTokens($"$..plugins[?(@.name=='{settingParams.Name}')]");
            foreach (var plugin in plugins)
            {
                settingParams.UpdateSetting(plugin);
                // 触发更新
                TriggerSettingChangedEvent(settingParams);
            }
        }

        public void RemovePluginSetting(string pluginName)
        {
            // 找到插件
            var jToen = _jsonConfig.SelectToken($"$.plugins[?(@.name=='{pluginName}')]");
            if (jToen == null) return;

            jToen.Remove();

            // 保存
            Save();
        }
        #endregion

        #region 获取设置
        /// <summary>
        /// 是否自动重载
        /// </summary>
        /// <param name="pluginName"></param>
        /// <returns></returns>
        public bool IsAutoReload(string pluginName)
        {
            var path = $"$.plugins[?(@.name=='{pluginName}')].autoReload";
            var value = _jsonConfig.SelectToken(path);
            if (value == null) return true;

            return value.ToObject<bool>();
        }

        /// <summary>
        /// 获取自动加载的插件
        /// </summary>
        /// <returns></returns>
        public List<LoaderSetup> GetAutoLoadLoaderSetups()
        {
            var path = "$..plugins[?(@.autoLoad==true)]";
            var plugins = _jsonConfig.SelectTokens(path);
            var results = new List<LoaderSetup>();
            foreach (var plugin in plugins)
            {
                var loader = new LoaderSetup()
                {
                    PluginName = plugin.SelectTokenOrDefault("name", string.Empty),
                    DllFullPath = plugin.SelectTokenOrDefault("dllFullPath", string.Empty),
                    PluginSetting = this,
                };
                if (string.IsNullOrEmpty(loader.PluginName)) continue;
                // 判断文件是否存在
                if (!File.Exists(loader.DllFullPath)) continue;

                results.Add(loader);
            }

            return results;
        }
        #endregion
    }
}
