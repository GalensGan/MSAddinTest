using Microsoft.Win32;
using MSAddinTest.Core.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "类库文件|*.dll";
            var result = openFileDialog.ShowDialog();
            if (result == null || !(bool)result) return StatusCode.Failed;

            string dllPath = openFileDialog.FileName;
            string pluginName = Path.GetFileNameWithoutExtension(dllPath);

            if (PluginDomains.ContainsKey(pluginName))
            {
                return StatusCode.AlreadyLoaded;
            }

            var loader = new PluginDomainLoader(pluginName);
            loader.LoadAssembly(dllPath);
            PluginDomains.Add(loader);

            // 加载成功后，将加载记录添加到本地设置中，方便下次调用
            PluginSetting.AddPlugin(pluginName, dllPath, true);
            PluginSetting.Save();

            return StatusCode.Success;
        }
    }
}
