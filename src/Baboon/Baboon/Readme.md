# Baboon项目总结

## 项目简介
Baboon是一个轻量级WPF插件化开发的基础库，内置了模块化加载、模块化日志记录、模块化本地存储、模块化IOC注册，以及开发Mvvm必要的Command和EventTrigger。

## 安装方法
通过nuget安装`Baboon`：
```
Install-Package Baboon
```

## 使用说明
### 基础配置
安装完成后，需要在`App.xaml`和`.cs`中修改基类继承，同时在`App.xaml.cs`中实现抽象类成员，包括注册容器和创建主窗口。

### 新建模块
- **创建项目与类**：新建一个项目，命名为`SayHello.Module`，添加对`Baboon`的引用，新建`SayHelloModule`类，继承自`AppModuleBase`或实现`IAppModule`接口，并实现基本成员。
- **模块描述**：`ModuleDescription`可以直接赋值，也可以通过xml文件定义。

### 模块注册
在`App`中重写`ConfigureModuleCatalog`方法来注册模块，注册方式有三种：
- **实例注册**：直接将模块实例注册到主程序，会立刻加载模块，且主程序需要引用模块。
- **类型注册**：将模块类型注册到主程序，会立刻加载模块，且主程序需要引用模块。
- **构建器注册**：只注册模块信息，需要时自动加载模块，主程序不需要引用模块。

### 模块加载与使用
- **加载**：模块加载是自动的，包括构建器注册的模块，会在需要时加载。
- **使用**：从容器中获得`IModuleCatalog`，使用`Contains(id)`方法判断模块是否已注册，通过`TryGetAppModuleInfo`获取模块信息，根据`appModuleInfo.Loaded`属性判断模块是否已加载到主程序，使用`appModuleInfo.GetApp()`获取指定模块。

### 模块管理器
使用`IModuleManagerService`服务可实现对模块的快速管理，包括安装、更新、卸载等。`IModuleCatalog`会对存放在`Modules`文件夹下的所有模块进行注册，但每个模块必须包含名为`Description.xml`的文件。

### 模块日志
每个模块可以拥有单独的日志记录器，通过`IAppModule.Logger`获得。

### 模块本地数据库
Baboon集成了LiteDB数据库，并封装了KV键值存储，使用`IConfigurationStoreService`即可。

### Mvvm框架
- **Command**：`ExecuteCommand`类。
- **ViewModel**：`ObservableObject`和`ViewModelBase`类。
- **容器注册View并绑定**：在`App.xaml.cs`的`RegisterTypes`方法中使用`container.RegisterSingletonView<MainWindow, MainViewModel>();`进行注册。
- **绑定事件触发器**：使用`EventAction`实现。

## 参与贡献
1. Fork本仓库
2. 新建`Feat_xxx`分支
3. 提交代码
4. 新建Pull Request