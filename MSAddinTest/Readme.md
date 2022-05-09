# TestMSAddin 开发文档

## 简述

本程序全称为：Bentley Addin 热加载测试插件。

## 基本原理

通过程序子域来实现插件的安装与卸载。

## 功能及实现

### 插件安装

添加一个命令，来自启动插件。

### 功能调用

主要实现以下功能：

1. 可以通过 `test 测试keyin` 来对 `keyin` 命令进行测试
2. 可以给目标类实现一个接口，然后就可以调用该接口进行测试

### 自动加载 DLL

不必在每次启动时都要手动加载一次。在程序中添加一个 keyin 命令，用来添加自动触发功能

### 卸载插件

主要解决以下问题：

1. 卸载时，要同时能够卸载指定的类库
2. 卸载后，要记录插件相关信息，方便下次再次加载

### 自动热加载插件

### 支持断点调试

### 劫持所有异常

劫持由当前插件引发的所有异常，防止由于两次异常后，Microstation 退出的问题。

### 预留UI接口

方便今后开发UI界面，短期内不会实现。

## 参考

1. [C#基础--应用程序域(Appdomain) AppDomain理解_love_hot_girl的博客-CSDN博客](https://blog.csdn.net/love_hot_girl/article/details/82949177)

2. https://docs.microsoft.com/en-us/dotnet/standard/assembly/load-unload

3. [C# 动态加载组件后怎么在开发环境中调试](https://www.cnblogs.com/DasonKwok/p/10510218.html)

4. [在C#中使用AppDomain实现【插件式】开发](https://www.cnblogs.com/mq0036/p/14646523.html)

5. [cad.net dll动态加载和卸载](https://www.cnblogs.com/JJBox/p/13833350.html)