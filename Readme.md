# MSAddinTest 使用文档

## 简介

本插件名为：MSAddinTest，可以帮助开发者在不关闭 Microstation 的情况下，可以实现代码热重载与快速调试。

对代码的入侵小，使用时只需在 keyin 前加上 `MSTest test` 即可调用现有的 keyin 命令。

The name of this plugin is `MSAddinTest`，it's an efficient plugin for addin developing on Miscrostation platform which allows you to hot-reload addin plugin。

it is so easy to use，when you want to invoke a keyin function，just put the string `MSTest test` before your  addin keyin，aha，just it~

## 功能特点

1. 支持热重载

   在进行 Microstation 调试过程中，仅可在方法内部进行修改，无法增加方法或者类等结构型操作，如果想要进行深度修改，只能关闭 Microstation，修改 Addin 库，编译，重新打开 Microstation，再开始调试。而启动 Microstation 相对费时，会浪费掉很多时间。

   本插件主要解决这个痛点，提供了对库的热重载功能，可以在不关闭 Microstation 的情况下，重新修改编译加载 DLL 库。

2. 支持自动启动

   支持一键配置在启动时加载。

3. 支持自动加载 DLL

   支持配置启动时自动加载测试 DLL，可以指定自动加载某些库。

4. 支持断点调试

   支持测试 DLL 的断点调试。

5. 搭建测试简单，代码入侵小，与正常开发无异

   测试 keyin 时，只需要 `MSTest test keyin` 即可执行测试，其它详见安装与使用。

## 为什么不使用VS的原生热重载?

vs 热重载对于以下修改不支持：

- 添加方法、字段、构造函数等
- 动态对象的增改
- [More ...](https://learn.microsoft.com/zh-cn/visualstudio/debugger/supported-code-changes-csharp?view=vs-2022#unsupported-changes-to-code)

而 MSAddinTest 没有上述限制。

## MSAddinTest 安装

1. 拷贝 `MSAddinTest.dll` 到 Microstaion 中的 `mdlapp` 目录里
2. 启动 Microstation
3. 加载  `MSAddinTest.dll`
4. 输入 `MSTest install` 开启自动启动。可以输入 `MSTest uninstall` 关闭自动启动。

> 支持基于 Microstation 的所有 Bentley 产品

## MSAddinTest 使用

### Debug 流程

**调试：**

1. [标记调用入口](#标记调用入口)
2. 编译代码
3. 启动 Microstation

1. 输入 keyin `MSTest load` 加载待测试 dll
2. VS 中附加进程到 Micostation 开启调试（快捷键：Ctrl+Alt+P）
3. 输入 `MSTest test keyin` 调用待测试的 keyin 命令

**重新修改：**

1. 关闭调试

2. 编辑代码

3. 重新编译

4. 在 Microstation 中使用 `MsTest reload DllName` 来重新加载库文件

   这一步可以通过设置实现自动重载（[转到 MSTest set](#mstest-set)）来跳过。

5. 在 vs 中将 Microstation 重新附加到进程（快捷键：Shift+Alt+P）

6. 输入 `MSTest test keyin` 进行调试

### 标记调用入口

对于 Addin、静态方法、实例方法，标记调用入口的方式略有不同，下面分别进行介绍。

#### Addin 库

想要支持 Addin 库的调用，须将自定义的 `Addin` 类继承抽象类 `TestAddin`。

示例如下：

``` csharp
/// <summary>
/// 测试 addin 调试
/// 1. 继承抽象类 TestAddin
/// 2. 在 Init 中获取传递的 addin 供当前库使用
/// </summary>
[AddIn(MdlTaskID = "TestAddinPlugin")]
internal class PluginAddin
// 通过 #if DEBUG 针对不同的编译场景编写不同的代码
#if DEBUG
  // debug 时，继承 TestAddin，实现热重载
  : TestAddin
{
  	// 可以在此处重写初始化方法，一般不进行重写
    // 该方法为 TestAddin 特有
    public override void Init(AddIn addin)
    {
        // 重写时，一定要调用父类中的方法
      	base.Init(addin);
    }
#else
  // 非 debug 模型式，直接继承 AddIn
	: AddIn
  { 
#endif
  
    public static AddIn Instance { get; private set; }
    public PluginAddin(IntPtr mdlDescriptor) : base(mdlDescriptor)
    {
        // 在此处可以保存 this 用于窗体的加载
        // this 是 TestAddin 实例，可以转换成 Addin 是因为 TestAddin 进行了隐式转换
        Instance = this;
    }
    
    // 在这个方法中释放资源
    public override void Unloaded(UnloadedEventArgs eventArgs) 
    {
        // 当插件重载时，可以在此处卸载上一次加载的事件
    }

    // Run 方法自动调用
    protected override int Run(string[] commandLine)
    {
        return 0;
    }
}
```

**使用方法：**

通过 `MSTest test + keyin + arg` 的方式调用 keyin 对应的静态方法

**其它：**

通过上述配置后，插件会自动读取命令表，从而根据命令表查找到 Keyin 对应的静态方法。

一般 keyin 会有多个单词，如果觉得通过 MSTest test + keyin 的方式太长，可以在静态方法中添加 `MSTestAttribute` 特性来添加 keyin 别名。

如下所示：

``` csharp
// 对于下列 keyin 对应的静态方法, 不仅可以通过 MSTest test keyin 来调用，还可以通过 MSTest test element 来调用
[MSTest("element",Description ="这是keyin别名")]
public static void TestElement(string unparsed)
{
   MessageBox.Show("我是 keyin,我被调用了");
}
```

#### 静态方法

除了 keyin 对应的静态方法外，想要调用其它静态方法，须满足以下条件：

1. 类继承接口 `ITestStaticMethod`
2. 静态方法添加特性 `MSTestAttribute`
3.  静态方法有且仅有一个 string 参数

示例如下：

``` csharp
/// <summary>
/// 测试静态方法
/// 1. 类继承接口 ITestStaticMethod
/// 2. 静态方法添加特性 MSTest
/// 3. 静态方法有且仅有一个IMSTestArg参数
/// </summary>
public class TestStaticMethodExecutor : ITestStaticMethod
{
    [MSTest("static")]
    public static object Execute(string arg)
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

1. 继承接口 `ITestClass`
2. 类上添加特性 `MSTestAttribute`

示例如下：

``` csharp
/// <summary>
/// 测试类执行器
/// 1. 继承接口 ITestClass
/// 2. 类添加特性 MSTest
/// </summary>
[MSTest("class", Description = "测试 IClassPlugin 插件")]
internal class TestClassExecutor : ITestClass
{
    // 实现接口 ITestClass
    // 该接口为实例初始化后的调用入口
    public void Execute(string arg)
    {
        MessageBox.Show("IClassPlugin 被调用了!");
    }
}

// 通过 MSTest test class 调用
```

**使用方法：**

通过 `MSTest test name` 调用。

#### 混合标记

可以将上述三种方式混合使用，可以在一个类上同时实现 `Addin`、`静态方法 `和 `实例方法` 三种测试方式。

其中 Addin 和 静态方法 的调用名称一样时，保留 Addin 测试入口。

## 内置 Keyin 介绍

### keyin 汇总

| keyin 名称               | 作用                                                         |
| ------------------------ | ------------------------------------------------------------ |
| MSTest install           | 在用户配置中添加 MSAddinTest 自启动配置。安装后，每次打开软件都会自动加载 MSAddinTest。 |
| MSTest uninstall         | 取消自启动。                                                 |
| MSTest load              | 加载待测试的 Dll。只有加载了的  Dll，才能调用其中的方法。如果已经加载过，则等效于 reload。 |
| MSTest unload + dllName  | 卸载已经加载的 dll                                           |
| MSTest reload + dllName  | 重新加载目标 dll。当修改编译后，需要调用该 keyin 进行重新加载。 |
| MSTest test + keyin/name | 调用方法。                                                   |
| MSTest set               | 设置参数                                                     |

### MSTest set

该命令目前支持以下两个参数的设置：

1. 目标 Dll 在启动时就加载

   格式：`dllName.autoLoad=true`

   多个配置用逗号分隔。

2. 目标 Dll 的自动热重载

   格式：`dllName.autoReload=true`

   当修改编译后，MSTestAddin 会自动重载目标 dll，不需要手动调用 `MSTest reload`

也可以同时设置：

```csharp
MSTest set dllName.autoLoad=true,dllName.autoReload=true
```

如果后一个参数的 dll 名称与前一个是一样，则可以省略，如下：

``` csharp
MSTest set dllName.autoLoad=true,autoReload=true
```

> 该配置的文件路径为：
>
> "_USTN_HOMEROOT" 变量位置下的`MSAddinTest\config.json`

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

## 实现原理

通过向默认域中加载不同版本的程序集来实现 DLL 版本的重载。为了可以重新编译已加载的 DLL，需要保证加载的 DLL 在被加载后不被锁定，本程序采用从内存的加载方式实现了这个需求。

该实现是一种伪热加载的实现方法，每次加载的 Dll 都会驻留在内存中，卸载某个程序集时，只是丢弃了其引用，并没有从内存里释放。

可能有人要问了，为什么采用这种方式呢？

该程序的前一个版本采用了子程序域的思想，虽然可以完美解决 Dll 的加载与卸载，但是无法跨程序域复用一些单例对象，比如 `Session.Instance`，所以最终采用了这种方式。

如果感兴趣，可以切换到 `Addin` 分支进行阅读。

当然，后期如果解决了上述所提到的问题，也会考虑重构成程序域的方式，毕竟那才是正统。

## 使用需知

1. DLL 一旦加载后，就不能卸载，所以每次加载都会驻留在内存中
2. 通过 `AppDomain.CurrentDomain.GetAssemblies()` 获取到的程序集是按加载顺序排列的
    如果在程序中涉及到序列化和反序列化操作时，序列化时不要保存程序集的版本信息，反序列化时取最新的程序集来获取相应的 Type。
3. Tool 工具内抛出的异常无法被 Catch 到，如果不处理，会触发 MS 异常捕获，超过两次后会闪退

## UI

本插件预留了 UI 接口，但短期内不会实现。欢迎 PR。

## 开发环境

1. VisualStudio 2022
2. Microstation CE
3. 环境变量 `Microstation`，值指向 Microstation 安装目录，路径末没有 `\` 号

## 赞助与支持

如果觉得插件不错，可以请作者喝一杯咖啡哟！

<div style="display:flex;justify-content:space-around;">
<img height="200px" src="https://i.loli.net/2021/08/13/JOw9cxomhBAZFW8.png" alt="wechat">
<img height="200px" src="https://i.loli.net/2021/08/13/U2s7gKn1zRw3uP4.png" alt="ailipay">
<div />

