using MSAddinTest.Core.Executor;
using MSAddinTest.MSTestInterface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace MSAddinTest.Core.Loader
{
    /// <summary>
    /// 程序集动态加载
    /// </summary>
    public partial class PluginAssemblyLoader
    {
        public LoaderSetup Setup { get; private set; }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="pluginName"></param>
        public PluginAssemblyLoader(LoaderSetup pluginDomainSetup)
        {
            Setup = pluginDomainSetup;

            // 监听事件
            RegistryEvents();

            // 获取所有 dll 文件
            _allFileNames = Directory.GetFiles(Setup.CurrentDomainBaseDirectory, "*.dll", SearchOption.AllDirectories).ToList();
        }

        // 所有根目录下的 dll 文件
        private List<string> _allFileNames;

        // 从程序集中读取的执行器
        private List<ExecutorBase> _executors = new List<ExecutorBase>();

        public FuncResult LoadAssembly()
        {
            try
            {
                // 读取文件然后加载
                byte[] bytes = File.ReadAllBytes(Setup.DllFullPath);
                var assembly = Assembly.Load(bytes);

                var results = BuilderExecutors(assembly);
                _executors = results.ToList();

                return new FuncResult(true);
            }
            catch (Exception ex)
            {
                return new FuncResult(false, ex.Message);
            }
        }



        /// <summary>
        /// 生成执行器
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerable<ExecutorBase> BuilderExecutors(Assembly assembly)
        {
            var results = new List<ExecutorBase>();

            // 获取类执行器
            results.AddRange(GenerateClassExecutor(assembly));

            // 静态方法执行器
            results.AddRange(GenerateStaticMethodExecutor(assembly));

            GenerateAddinExecutor(assembly);

            return results;
        }

        // 获取类执行器
        private IEnumerable<ExecutorBase> GenerateClassExecutor(Assembly assembly)
        {
            List<ExecutorBase> results = new List<ExecutorBase>();

            // 生成运行数据
            var iPluginType = typeof(IMSTest_Class);
            var pluginTypes = assembly.GetTypes().Where(x => !x.IsInterface && !x.IsAbstract && iPluginType.IsAssignableFrom(x));
            // 获取非 addin 插件
            {
                var commonPluginTypes = pluginTypes;
                foreach (var pluginType in commonPluginTypes)
                {
                    var classExecutor = new ClassExecutor(pluginType);

                    // 获取Plugin特定
                    var pluginAttr = pluginType.GetCustomAttributes(typeof(MSTestAttribute)).FirstOrDefault();
                    if (pluginAttr is MSTestAttribute attr)
                    {
                        classExecutor.Name = attr.Name;
                        classExecutor.Description = attr.Description;
                    }
                    else
                    {
                        classExecutor.Name = pluginType.Name;
                    }

                    results.Add(classExecutor);
                }
            }

            return results;
        }

        // 读取静态执行器
        private IEnumerable<ExecutorBase> GenerateStaticMethodExecutor(Assembly assembly)
        {
            List<ExecutorBase> results = new List<ExecutorBase>();

            // 生成运行数据
            var iPluginType = typeof(IMSTest_StaticMethod);
            var pluginTypes = assembly.GetTypes().Where(x => !x.IsInterface && !x.IsAbstract && iPluginType.IsAssignableFrom(x));
            // 获取静态方法执行器
            {
                var commonPluginTypes = pluginTypes;
                foreach (var pluginType in commonPluginTypes)
                {
                    var methodInfos = pluginType.GetMethods().Where(x => x.GetCustomAttribute(typeof(MSTestAttribute)) != null);
                    foreach (var methodInfo in methodInfos)
                    {
                        var classExecutor = new StaticMethodExecutor(pluginType, methodInfo);

                        // 获取入口描述
                        var pluginAttr = methodInfo.GetCustomAttributes(typeof(MSTestAttribute)).FirstOrDefault();
                        if (pluginAttr is MSTestAttribute attr)
                        {
                            classExecutor.Name = attr.Name;
                            classExecutor.Description = attr.Description;
                        }
                        else
                        {
                            classExecutor.Name = methodInfo.Name;
                        }

                        results.Add(classExecutor);
                    }
                }
            }

            return results;
        }

        // 读取addin执行器
        private IEnumerable<ExecutorBase> GenerateAddinExecutor(Assembly assembly)
        {
            // 生成运行数据
            var iPluginType = typeof(MSTest_Addin);

            var type = assembly.GetTypes().ToList();

            var pluginTypes = assembly.GetTypes().Where(x => x.IsSubclassOf(iPluginType));



            foreach (var pluginType in pluginTypes)
            {
                // 找到后立即进行初始化
                try
                {
                    var addin = Activator.CreateInstance(pluginType, IntPtr.Zero) as MSTest_Addin;
                    // 进行初始化
                    addin.Init(Index.MSAddin.Instance);
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("Addin初始化失败：" + ex.Message);
                }
            }

            // 开始读取命令表


            // 通过命令表查到静态方法

            return new List<ExecutorBase>();
        }
    }
}
