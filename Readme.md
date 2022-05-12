# MSAddinTest 使用文档

## 简介

本插件名为：MSAddinTest，是一款面向 Microstation 的 Addin 管理测试插件。通过它，你可以在不关闭 Microstation 的情况下，重新修改编译加载 DLL 库。

你只需在 keyin 前加上 `MSTest test` 即可调用现有的 keyin 命令。

## 功能特点

1. 库热重载

   在进行 Microstation 调试过程中，仅可在方法内部进行修改，无法增加方法或者类等结构型操作，如果想要进行深度修改，只能关闭 Microstation，修改 Addin 库，编译，重新打开 Microstation，再开始调试。而启动 Microstation 相对费时，会浪费掉很多时间。

   本插件主要解决这个痛点，提供了对库的热重载功能，可以在不关闭 Microstation 的情况下，重新修改编译加载 DLL 库。

2. 自动启动

   支持一键配置在启动时加载。

3. 自动加载 DLL

   支持配置启动时自动加载测试 DLL，可以指定自动加载某些库。

4. 支持断点调试

   支持测试 DLL 的断点调试。

5. 搭建测试简单，代码入侵小，与正常开发异

   测试 keyin 时，只需要 `MsTest test keyin` 即可执行测试，其它详见安装与使用。

## 安装与使用

### 插件安装

1. 拷贝 `MSAddinTest.dll` 到 Microstaion 中的 `mdlapp` 目录里
2. 启动 Microstation
3. 加载  `MSAddinTest.dll`
4. 输入 `MSTest install` 开启自动启动。可以输入 `MSTest uninstall` 关闭自动启动。

### 测试标记

MSAddinTest 支持对以下三种方式的接口进行调用。

#### Addin 库

想要支持 Addin 库的调用，须满足以下条件：

1. 自定义的 `Addin` 类继承抽象类 `MSTest_Addin`
2. 实现抽象方法 `void Init(AddIn addIn)`

示例如下：

``` csharp
/// <summary>
/// 测试 addin 调试
/// 1. 继承抽象类 MSTest_Addin
/// 2. 在 Init 中获取传递的 addin 供当前库使用
/// </summary>
[AddIn(MdlTaskID = "TestAddinPlugin")]
internal class PluginAddin : MSTest_Addin
{
    public static AddIn Instance { get; private set; }
    public PluginAddin(IntPtr mdlDescriptor) : base(mdlDescriptor)
    {

    }

    public override void Init(AddIn addin)
    {
        Instance = addin;
        Run(new string[] { });
    }

    protected override int Run(string[] commandLine)
    {
        return 0;
    }
}
```

> 如果需要使用 Addin，可以在 Init 方法中将 addin 保存

**使用方法：**

通过 `MSTest test + keyin + arg` 的方式调用 keyin 对应的静态方法

**其它：**

通过上述配置后，插件会自动读取命令表，从而根据命令表查找到 Keyin 对应的静态方法。一般 keyin 会有多个单词，如果觉得通过 MSTest test + keyin 的方式太长，可以在静态方法中添加 `MSTestAttribute` 特性来添加 keyin 别名。

如下所示：

``` csharp
// 可以通过 MSTest test element 来调用

[MSTest("element",Description ="这是keyin别名")]
public static void TestElement(string unparsed)
{
   MessageBox.Show("我是 keyin,我被调用了");
}
```

#### 静态方法

除了 keyin 对应的静态方法外，想要调用其它静态方法，须满足以下条件：

1. 类继承接口 IMSTest_StaticMethod
2. 静态方法添加特性 MSTestAttribute
3.  静态方法有且仅有一个 IMSTestArg 参数

示例如下：

``` csharp
/// <summary>
/// 测试静态方法
/// 1. 类继承接口 IMSTest_StaticMethod
/// 2. 静态方法添加特性 MSTest
/// 3. 静态方法有且仅有一个IMSTestArg参数
/// </summary>
public class TestStaticMethodExecutor : IMSTest_StaticMethod
{
    [MSTest("static")]
    public static object Execute(IMSTestArg arg)
    {
        MessageBox.Show("IStaticMethodPlugin 被调用了!");
        return true;
    }
}

// 通过 MSTest test static 来调用，static 指的是 [MSTest("static")] 中的 static
```

**使用方法：**

通过 `MSTest test name` 调用。

> 静态方法必须添加 MSTestAttribute 才能被调用

#### 实例类

本插件也支持对实例的调试，须满足以下条件：

1. 继承接口 IMSTest_Class
2. 类上添加特性 MSTestAttribute

示例如下：

``` csharp
/// <summary>
/// 测试类执行器
/// 1. 继承接口 IMSTest_Class
/// 2. 类添加特性 MSTest
/// </summary>
[MSTest("class", Description = "测试 IClassPlugin 插件")]
internal class TestClassExecutor : IMSTest_Class
{
    // 实现接口
    public void Execute(IMSTestArg arg)
    {
        MessageBox.Show("IClassPlugin 被调用了!");
    }
}

// 通过 MSTest test class 调用
```

**使用方法：**

通过 `MSTest test name` 调用。

**其它：**

当然，实例类中也可以同时添加静态方法入口，只需要按静态方法配置即可。

## 引用 DLL 更新

假设有一个 addin.dll，它引用了一个开发中的功能库 utils.dll，当我们修改 utils.dll 时，也希望插件可以热加载，及时反馈到调试上。

要使引用的 Dll 变动触发热重载，需要给引用的程序集添加 AssemblyVersion 自增版本号（当然，也可以每次修改后手动修改该版本号）。

自增版本号修改方法如下：

1. 在 `Properties.AssemblyInfo.cs` 修改如下内容：

   ``` csharp
   // 将下面内容
   [assembly: AssemblyVersion("1.0.0.0")]
   // 改成
   [assembly: AssemblyVersion("1.0.0.*")]
   ```

2. 打开项目配置文件 `XXX.csproj`，修改如下内容：

   ``` xml
   // 将这个配置改成 false
   <Deterministic>false</Deterministic>
   ```

> **特别注意：**
>
> 当被引用 DLL 用于正式环境时，须将其程序集集版本号改成特定版本号，防止其它引用该 DLL 的其它库出现错误。

## 内置 Keyin 介绍

### keyin 汇总

| keyin 名称              | 作用                                                |
| ----------------------- | --------------------------------------------------- |
| MSTest install          | 在用户配置中添加 TestMSAddin 启动加载配置           |
| MSTest uninstall        | 取消启动加载                                        |
| MSTest test + keyin     | 调用方法                                            |
| MSTest load             | 加载其它 Dll。只有加载了的  Dll，才能调用其中的方法 |
| MSTest unload + dllName | 卸载已经加载的 dll                                  |
| MSTest set              | 设置参数                                            |

### MSTest set

该命令目前支持设置以下两个参数：

1. 目标 Dll 的启动时加载

   格式：`dllName.autoLoad=true`

   多个配置用逗号分隔。

2. 目标 Dll 的自动热重载

   格式：`dllName.autoReload=true`

也可以同时设置：

```csharp
dllName.autoLoad=true,dllName.autoReload=true
```

如果后一个参数的 dll 名称与前一个是一样，则可以省略，如下：

``` csharp
dllName.autoLoad=true,autoReload=true
```

## VisualStudio Debug 流程

**调试：**

1. 启动 Microstation
2. 通过输入 keyin `MSTest load` 加载待测试 dll
3. VS 中附加进程到 Micostation 开启调试

**重新修改：**

1. 关闭调试
2. 编辑代码
3. 重新编译，重新编译后即可在 Microstation 调用编辑后的代码
4. 如果要进行 debug，重新附加到进程即可

## 实现原理

通过向默认域中加载不同版本的程序集来实现 DLL 版本的重载。为了可以重新编译已加载的 DLL，需要保证加载的 DLL 在被加载后不被锁定，本程序采用从内存的加载方式实现了这个需求。

该实现是一种伪热加载的实现方法，每次加载的 Dll 都会驻留在内存中，卸载的时候某个程序集时，只是丢弃了其引用，并没有从内存里释放。

可能有人要问了，为什么采用这种方式呢？

该程序的前一个版本采用了子程序域的思想，虽然可以完美解决 Dll 的加载与卸载，但是无法跨程序域复用一些单例对象，比如 `Session.Instance`，所以最终采用了这种方式。

如果感兴趣，可以切换到 `Addin` 分支进行阅读。

当然，后期如果解决了上述所提到的问题，也会考虑重构成程序域的方式，毕竟那才是正统。

## 使用需知

1. DLL 一旦加载后，就不能卸载，所以每次加载都会驻留在内存中
2. 通过 `AppDomain.CurrentDomain.GetAssemblies()` 获取到的程序集是按加载顺序排列的
    如果在程序中涉及到序列化和反序列化操作时，序列化时不要保存程序集的版本信息，反序列化时取最新的程序集来获取相应的 Type。

## UI

本插件预留了 UI 接口，但短期内不会实现。

## 参考

1. [C#基础--应用程序域(Appdomain) AppDomain理解_love_hot_girl的博客-CSDN博客](https://blog.csdn.net/love_hot_girl/article/details/82949177)

2. https://docs.microsoft.com/en-us/dotnet/standard/assembly/load-unload

3. [C# 动态加载组件后怎么在开发环境中调试](https://www.cnblogs.com/DasonKwok/p/10510218.html)

4. [在C#中使用AppDomain实现【插件式】开发](https://www.cnblogs.com/mq0036/p/14646523.html)

5. [cad.net dll动态加载和卸载](https://www.cnblogs.com/JJBox/p/13833350.html)