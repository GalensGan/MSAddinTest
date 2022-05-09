using MSAddinTest.Core.Executor;
using MSAddinTest.Plugin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace MSAddinTest.Core
{
    /// <summary>
    /// 该类实例位于子程序域中
    /// 子程序域向默认域中传递的所有值，都需要继承 MarshalByRefObject
    /// </summary>
    public class RemoteLoader : MarshalByRefObject
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

        /// <summary>
        /// 生成执行器
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerable<ExecutorBase> BuilderExecutors(Assembly assembly)
        {
            var results = new List<ExecutorBase>();

            // 获取类执行器
            results.AddRange(GenerateClassExecutor(assembly));

            return results;
        }
        
        // 获取类执行器
        private IEnumerable<ExecutorBase> GenerateClassExecutor(Assembly assembly)
        {
            List<ExecutorBase> results = new List<ExecutorBase>();

            // 生成运行数据
            var iPluginType = typeof(IClassPlugin);
            var pluginTypes = _assembly.GetTypes().Where(x => !x.IsInterface && !x.IsAbstract && iPluginType.IsAssignableFrom(x));
            int count = pluginTypes.Count();
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

        private void GetStaticMethodExecutor(Assembly assembly)
        {

        }


        /// <summary>
        /// 实例化执行对象并执行
        /// </summary>
        /// <param name="name"></param>
        /// <param name="arg"></param>
        /// <returns></returns>
        public object Execute(string name, PluginArg arg)
        {
            var executors = _executors.FindAll(x => x.Name == name);

            foreach (var executor in executors)
            {
                executor.Execute(arg);
            }

            return executors.Count;
        }
    }
}