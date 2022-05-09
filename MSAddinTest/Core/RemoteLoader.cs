using MSAddinTest.Core.Executor;
using MSAddinTest.Plugin;
using System;
using System.Collections.Generic;
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
        private List<IExecutor> _executors = new List<IExecutor>();

        public void LoadAssembly(string assemblyFile)
        {
            try
            {
                _assembly = Assembly.LoadFrom(assemblyFile);
                //return _assembly;

                // 生成运行数据
                var iPluginType = typeof(IPlugin);
                var pluginTypes = _assembly.GetTypes().Where(x => !x.IsInterface && !x.IsAbstract && iPluginType.IsAssignableFrom(x));
                int count = pluginTypes.Count();
                // 获取非 addin 插件
                {
                    var commonPluginTypes = pluginTypes.Where(x => !x.IsSubclassOf(typeof(AddinPlugin)));
                    foreach (var pluginType in commonPluginTypes)
                    {
                        var classExecutor = new ClassExecutor()
                        {
                            Type = pluginType,
                        };

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

                        _executors.Add(classExecutor);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public object Execute(string name, PluginArg arg)
        {
            var executors = _executors.FindAll(x => x.Name == name);

            foreach (var executor in executors)
            {
                var instance = Activator.CreateInstance(executor.Type);
                if(instance is IPlugin plugin)
                {
                    // 初始化
                    // 传入初始化数据
                    plugin.Init(arg);
                    plugin.Execute(arg);
                }
            }

            return true;
        }
    }
}