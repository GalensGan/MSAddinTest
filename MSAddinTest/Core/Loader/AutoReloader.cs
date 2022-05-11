using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSAddinTest.Core.Loader
{
    /// <summary>
    /// 自动重新加载
    /// </summary>
    internal class AutoReloader
    {
        private FileSystemWatcher _watcher;
        private PluginAssemblyLoader _assemblyLoader;

        public AutoReloader(PluginAssemblyLoader assemblyLoader)
        {
            _assemblyLoader = assemblyLoader;

            _watcher = new FileSystemWatcher(Path.GetDirectoryName(assemblyLoader.Setup.DllFullPath))
            {
                Filter = "*.dll",
                NotifyFilter = NotifyFilters.LastWrite
            };
            _watcher.Changed += Watcher_Changed;

            // 从设置中获取是否自动重载
            var setting = _assemblyLoader.Setup.PluginSetting;
            _watcher.EnableRaisingEvents = setting.IsAutoReload(_assemblyLoader.Setup.PluginName);

            // 监听数据变化
            setting.SettingChanged += (arg) =>
            {
                if(arg.FieldName == "autoReload" && bool.TryParse(arg.Value,out bool result))
                {
                    _watcher.EnableRaisingEvents = result;
                }
            };
           
        }

        private void Watcher_Changed(object sender, FileSystemEventArgs e)
        {
            // 判断是否是当前文件
            if (e.FullPath != _assemblyLoader.Setup.DllFullPath) return;

            // 文件更改后，需要重新加载
            _assemblyLoader.Reload();
        }
    }
}
