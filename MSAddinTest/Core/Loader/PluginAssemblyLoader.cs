using Bentley.MstnPlatformNET;
using MSAddinTest.Core.Executor;
using MSAddinTest.MSTestInterface;
using MSAddinTest.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

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
            _allFileNames = Directory.GetFiles(Setup.BaseDirectory, "*.dll", SearchOption.AllDirectories).ToList();
            _allFileNames.AddRange(Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll", SearchOption.AllDirectories).ToList());

            _autoReloader = new AutoReloader(this);
        }

        // 自动重新加载
        private readonly AutoReloader _autoReloader;

        // 所有根目录下的 dll 文件
        private readonly List<string> _allFileNames;

        // 从程序集中读取的执行器
        private readonly List<ExecutorBase> _executors = new List<ExecutorBase>();

        private Assembly _currentAssembly;
        public FuncResult LoadAssembly()
        {
            try
            {
                // 判断文件是否存在
                if (!File.Exists(Setup.DllFullPath))
                {
                    return new FuncResult(false, "文件不存在");
                }

                // 验证文件 hash 值
                //var newFileHash = FileHelper.GetFileHash(Setup.DllFullPath);
                //if (_lastFileHash == newFileHash)
                //    return new FuncResult(false, "文件未改变");
                //else
                //    _lastFileHash = newFileHash;

                // 执行卸载逻辑
                _msAddins.ForEach(x => x.NotifyOnUnloaded(new AddIn.UnloadedEventArgs(AddIn.UnloadReasons.ExitByOtherApp)));
                _msAddins.Clear();

                // 读取文件然后加载
                byte[] bytes = File.ReadAllBytes(Setup.DllFullPath);
                _currentAssembly = Assembly.Load(bytes);

                _executors.Clear();
                BuilderExecutors(_currentAssembly);

                return new FuncResult(true);
            }
            catch (Exception ex)
            {
                return new FuncResult(false, ex.Message + ex.StackTrace);
            }
        }

        /// <summary>
        /// 生成执行器
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerable<ExecutorBase> BuilderExecutors(Assembly assembly)
        {
            var results = new List<ExecutorBase>();

            // 类执行器
            AddExecutors(GenerateClassExecutor(assembly));

            // 静态方法执行器
            AddExecutors(GenerateStaticMethodExecutor(assembly));

            // 添加 addin 执行器
            AddExecutors(GenerateAddinExecutor(assembly));

            return results;
        }

        private void AddExecutors(IEnumerable<ExecutorBase> executors)
        {
            foreach (var executor in executors)
            {
                var executorTemp = _executors.Find(x => x.IsSame(executor));
                // 如果没找到或者优先级比原来高，则添加
                if (executorTemp == null || executorTemp.Priority < executor.Priority)
                {
                    if (executorTemp != null) _executors.Remove(executorTemp);
                    _executors.Add(executor);
                }
            }
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
                    results.Add(classExecutor);
                }
            }

            return results;
        }

        // 读取静态执行器
        // 条件要求：
        // 1-继承接口 IMSTest_StaticMethod
        // 2-具有 MSTestAttribute 属性
        // 3-有一个 string 类型的参数
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
                        // 获取参数
                        var paraInfos = methodInfo.GetParameters();
                        if (paraInfos.Length != 1 || !typeof(string).IsAssignableFrom(paraInfos[0].ParameterType))
                        {
                            MessageCenter.Instance.ShowErrorMessage($"静态方法 {methodInfo.Name} 的参数个数必须有且只有一个 string 参数", "", false);
                            continue;
                        };

                        var classExecutor = new StaticMethodExecutor(methodInfo);

                        results.Add(classExecutor);
                    }
                }
            }

            return results;
        }

        private readonly List<MSTest_Addin> _msAddins = new List<MSTest_Addin>();
        // 读取addin执行器
        private IEnumerable<ExecutorBase> GenerateAddinExecutor(Assembly assembly)
        {
            // 生成运行数据
            var iPluginType = typeof(MSTest_Addin);
            var allTypes = assembly.GetTypes();
            var pluginTypes = allTypes.Where(x => x.IsSubclassOf(iPluginType));

            foreach (var pluginType in pluginTypes)
            {
                // 找到后立即进行初始化
                try
                {
                    var addin = Activator.CreateInstance(pluginType,
                       BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance,
                       null,
                       new object[] { IntPtr.Zero }, CultureInfo.CurrentCulture) as MSTest_Addin;
                    // 完成后调用 run
                    // 在此处调用初始化内容
                    addin.Init(addin);

                    _msAddins.Add(addin);
                }
                catch (Exception ex)
                {
                    if (ex.InnerException != null) ex = ex.InnerException;
                    MessageCenter.Instance.ShowErrorMessage("Addin 初始化失败：" + ex.Message, ex.StackTrace, true);
                }
            }

            // 开始读取命令表
            var resourceNames = assembly.GetManifestResourceNames().Where(x => x.EndsWith(".xml"));

            var results = new List<ExecutorBase>();

            // 读取所有的命令表
            foreach (var resourceName in resourceNames)
            {
                var xmlStream = assembly.GetManifestResourceStream(resourceName);
                var xDoc = XElement.Load(xmlStream);
                XNamespace ns = "http://www.bentley.com/schemas/1.0/MicroStation/AddIn/KeyinTree.xsd";
                var childerens = xDoc.Descendants(ns + "KeyinHandler");
                // 获取属性
                foreach (XElement xElement in childerens)
                {
                    var keyin = xElement.Attribute("Keyin").Value;
                    var function = xElement.Attribute("Function").Value;

                    // 通过这两个参数生成执行器
                    var lastIndex = function.LastIndexOf(".");
                    var fullTypeName = function.Substring(0, lastIndex);
                    var functionName = function.Substring(lastIndex + 1);

                    var functionType = allTypes.FirstOrDefault(x => x.FullName == fullTypeName);
                    if (functionType == null) continue;

                    var addinExecutor = new AddinExecutor(functionType, functionName);
                    addinExecutor.Names.Add(keyin);

                    results.Add(addinExecutor);
                }
            }

            // 通过命令表查到静态方法
            return results;
        }
    }
}
