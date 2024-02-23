using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MSAddinTest.Core.Loader
{
    /// <summary>
    /// 加载器的事件
    /// </summary>
    public partial class PluginAssemblyLoader
    {
        private void RegistryEvents()
        {
            var appDomain = AppDomain.CurrentDomain;
            appDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            appDomain.TypeResolve += AppDomain_TypeResolve;
            appDomain.UnhandledException += CurrentDomain_UnhandledException;
            appDomain.FirstChanceException += AppDomain_FirstChanceException;
        }

        private void AppDomain_FirstChanceException(object sender, System.Runtime.ExceptionServices.FirstChanceExceptionEventArgs e)
        {
            // Message.MessageManager.ShowException(e.Exception);
        }

        #region 加载需要的程序集
        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.ExceptionObject.ToString());
        }

        // 某个类型未找到时，去加载类型
        private Assembly AppDomain_TypeResolve(object sender, ResolveEventArgs args)
        {
            return CurrentDomain_AssemblyResolve(sender, args);
        }

        private Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            // 获取最新版本的程序集
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            string assemblyName = args.Name.Split(',')[0].Trim();
            Assembly targetAssembly = assemblies.LastOrDefault(x => x.FullName.Contains(assemblyName));

            if (targetAssembly != null) return targetAssembly;

            // 找到文件，然后加载            
            string assemblyFileName = $"{assemblyName}.dll";
            string fileName = _allFileNames.Find(x => x.EndsWith(assemblyFileName));
            if (fileName == null) return null;

            byte[] bytes = File.ReadAllBytes(fileName);
            var assembly = Assembly.Load(bytes);

            // 判断版本是否一致，不一致返回 null,交给其它订阅处理
            if (assembly != null)
            {
                if (assembly.GetName().FullName == args.Name) return assembly;
                return null;
            }

            return assembly;
        }
        #endregion
    }
}
