using MSAddinTest.Core.Executor;
using MSAddinTest.PluginInterface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace MSAddinTest.Core
{
    /// <summary>
    /// 该类实例位于子程序域中
    /// 子程序域向默认域中传递的所有值，都需要继承 MarshalByRefObject
    /// </summary>
    public partial class RemoteLoader : MarshalByRefObject
    {
        private Assembly _assembly;
        private readonly List<ExecutorBase> _executors = new List<ExecutorBase>();

        public void LoadAssembly(string assemblyFile)
        {
            try
            {
                // 读取文件然后加载
                byte[] bytes = File.ReadAllBytes(assemblyFile);
                _assembly = Assembly.Load(bytes);
                //_assembly = Assembly.LoadFrom(assemblyFile);
                //return _assembly;

                var results = BuilderExecutors(_assembly);
                _executors.AddRange(results);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region 加载需要的程序集
        private string _msBaseName;
        private List<string> _allFileNames;
        public void SetAssemblyResolver(string msBaseName)
        {
            _msBaseName = msBaseName;
            _allFileNames = Directory.GetFiles(_msBaseName, "*.dll", SearchOption.AllDirectories).ToList();

            var appDomain = AppDomain.CurrentDomain;
            appDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            appDomain.TypeResolve += AppDomain_TypeResolve;
            appDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            ;
        }

        // 某个类型未找到时，去加载类型
        private Assembly AppDomain_TypeResolve(object sender, ResolveEventArgs args)
        {
           return CurrentDomain_AssemblyResolve(sender, args);
        }

        private Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            // 加载 MS 中的程序集
            // 判断是否已经加载
            var exist = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(x => x.FullName == args.Name);
            if (exist != null) return exist;

            // 找到文件，然后加载
            string targetFileName = args.Name.Split(',')[0];
            string fileName = _allFileNames.Find(x => x.Contains(targetFileName));
            if (fileName == null) return null;

            byte[] bytes = File.ReadAllBytes(fileName);
            var assembly = Assembly.Load(bytes);

            return assembly;
        }
        #endregion

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

            return results;
        }

        // 获取类执行器
        private IEnumerable<ExecutorBase> GenerateClassExecutor(Assembly assembly)
        {
            List<ExecutorBase> results = new List<ExecutorBase>();

            // 生成运行数据
            var iPluginType = typeof(IClassPlugin);
            var pluginTypes = _assembly.GetTypes().Where(x => !x.IsInterface && !x.IsAbstract && iPluginType.IsAssignableFrom(x));
            // 获取非 addin 插件
            {
                var commonPluginTypes = pluginTypes;
                foreach (var pluginType in commonPluginTypes)
                {
                    var classExecutor = new ClassExecutor(pluginType);

                    // 获取Plugin特定
                    var pluginAttr = pluginType.GetCustomAttributes(typeof(PluginAttribute)).FirstOrDefault();
                    if (pluginAttr is PluginAttribute attr)
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
            var iPluginType = typeof(IStaticMethodPlugin);
            var pluginTypes = _assembly.GetTypes().Where(x => !x.IsInterface && !x.IsAbstract && iPluginType.IsAssignableFrom(x));
            int count = pluginTypes.Count();
            // 获取静态方法执行器
            {
                var commonPluginTypes = pluginTypes;
                foreach (var pluginType in commonPluginTypes)
                {
                    var methodInfos = pluginType.GetMethods().Where(x => x.GetCustomAttribute(typeof(PluginAttribute)) != null);
                    foreach (var methodInfo in methodInfos)
                    {
                        var classExecutor = new StaticMethodExecutor(pluginType, methodInfo);

                        // 获取入口描述
                        var pluginAttr = methodInfo.GetCustomAttributes(typeof(PluginAttribute)).FirstOrDefault();
                        if (pluginAttr is PluginAttribute attr)
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


        /// <summary>
        /// 实例化执行对象并执行
        /// </summary>
        /// <param name="name"></param>
        /// <param name="arg"></param>
        /// <returns></returns>
        public object Execute(string name, PluginArg arg)
        {
            // 名称不区分大小写
            var executors = _executors.FindAll(x => x.Name.ToLower() == name.ToLower());

            foreach (var executor in executors)
            {
                executor.Execute(arg);
            }

            return executors.Count;
        }
    }
}