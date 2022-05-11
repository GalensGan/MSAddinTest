using Microsoft.Win32;
using MSAddinTest.Core.Loader;
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
        public override FuncResult Start()
        {
            // 打开选择窗体
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "类库文件|*.dll",
                Multiselect = true,
            };
            var result = openFileDialog.ShowDialog();
            if (result == null || !(bool)result) return new FuncResult(false)
            {
                StatusCode = StatusCode.Failed
            };

            var dllPaths = openFileDialog.FileNames;

            foreach (var dllPath in dllPaths)
            {
                string pluginName = Path.GetFileNameWithoutExtension(dllPath);

                if (PluginContainer.ContainsKey(pluginName))
                {
                    return new FuncResult(false)
                    {
                        StatusCode = StatusCode.AlreadyLoaded
                    };
                }

                var setup = new LoaderSetup()
                {
                    PluginName = pluginName,
                    DllFullPath = dllPath,
                    PluginSetting = PluginSetting,
                };

                var loader = new PluginAssemblyLoader(setup);
                var loaderRes = loader.LoadAssembly();
                if (loaderRes.NotOk)
                {
                    MessageBox.Show(loaderRes.Message);
                    return loaderRes;
                }
                PluginContainer.Add(loader);

                AssemblyLoaded(loader);
            }

            return new FuncResult(true);
        }

        /// <summary>
        /// 保存到文件记录中
        /// </summary>
        /// <param name="loader"></param>
        protected virtual void AssemblyLoaded(PluginAssemblyLoader loader)
        {
            // 加载成功后，将加载记录添加到本地设置中，方便下次调用
            PluginSetting.AddPlugin(loader.Setup.PluginName, loader.Setup.DllFullPath, true);
            PluginSetting.Save();
        }
    }
}
