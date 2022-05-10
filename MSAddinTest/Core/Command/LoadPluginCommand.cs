using Microsoft.Win32;
using MSAddinTest.Core.DomainLoader;
using MSAddinTest.Core.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MSAddinTest.Core.Command
{
    /// <summary>
    /// 加载插件
    /// </summary>
    internal class LoadPluginCommand : CommandBase
    {
        // 执行命令
        public override object Start()
        {
            // 打开选择窗体
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "类库文件|*.dll",
                Multiselect = true,
            };
            var result = openFileDialog.ShowDialog();
            if (result == null || !(bool)result) return StatusCode.Failed;

            var dllPaths = openFileDialog.FileNames;

            foreach (var dllPath in dllPaths)
            {
                string pluginName = Path.GetFileNameWithoutExtension(dllPath);

                if (PluginDomains.ContainsKey(pluginName))
                {
                    return StatusCode.AlreadyLoaded;
                }

                var setup = new PluginDomainSetup()
                {
                    PluginName = pluginName,
                    DllFullPath = dllPath,
                };

                var loader = new PluginDomainLoader(setup);
                if (!loader.LoadAssembly())
                {
                    MessageBox.Show("插件加载失败");
                    return false;
                }
                PluginDomains.Add(loader);

                AssemblyLoaded(loader);
            }

            return StatusCode.Success;
        }

        /// <summary>
        /// 保存到文件记录中
        /// </summary>
        /// <param name="loader"></param>
        protected virtual void AssemblyLoaded(PluginDomainLoader loader)
        {
            // 加载成功后，将加载记录添加到本地设置中，方便下次调用
            PluginSetting.AddPlugin(loader.PluginDomainSetup.PluginName, loader.PluginDomainSetup.DllFullPath, true);
            PluginSetting.Save();
        }
    }
}
