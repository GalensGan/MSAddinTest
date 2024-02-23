using Bentley.MstnPlatformNET;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;

namespace MSAddinTest.Core.Loader
{
    /// <summary>
    /// 自动重新加载
    /// </summary>
    internal class AutoReloader
    {
        private readonly FileSystemWatcher _watcher;
        private readonly PluginAssemblyLoader _assemblyLoader;
        private readonly Timer _reloadTimer = new Timer(1000)
        {
            Enabled = false,
            AutoReset = true
        };

        public AutoReloader(PluginAssemblyLoader assemblyLoader)
        {
            _assemblyLoader = assemblyLoader;

            _watcher = new FileSystemWatcher(Path.GetDirectoryName(assemblyLoader.Setup.DllFullPath))
            {
                Filter = "*.dll",
                NotifyFilter = NotifyFilters.LastWrite
            };
            _watcher.Changed += Watcher_Changed;
            _reloadTimer.Elapsed += ReloadTimer_Elapsed;

            // 从设置中获取是否自动重载
            var setting = _assemblyLoader.Setup.PluginSetting;
            _watcher.EnableRaisingEvents = setting.IsAutoReload(_assemblyLoader.Setup.PluginName);

            // 监听数据变化
            setting.SettingChanged += (arg) =>
            {
                if (arg.FieldName == "autoReload" && bool.TryParse(arg.Value, out bool result))
                {
                    _watcher.EnableRaisingEvents = result;
                }
            };

        }       

        private void Watcher_Changed(object sender, FileSystemEventArgs e)
        {
            // 只有当前文件才触发
            if (e.FullPath != _assemblyLoader.Setup.DllFullPath)
            {
                return;
            }

            // 多次触发时，重置计时器
            _reloadTimer.Stop();
            _reloadTimer.Start();
        }

        private void ReloadTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _reloadTimer.Stop();
            // 事件是在另一个线程触发的，直接重载会报错
            // 切换到主线程执行
            Application.Current.Dispatcher.Invoke(() =>
            {
                // 文件更改后，需要重新加载
                _assemblyLoader.Reload();

            });
        }
    }
}
