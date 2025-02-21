# Baboon

## 一、介绍

这是一个轻量级wpf和winform的插件化开发的基础库。它内置了模块化加载、模块化日志记录、模块化IOC注册、以及开发wpf时的Mvvm必要的Command和EventTrigger。

## 二、安装教程

nuget安装`Baboon`即可。

```
Install-Package Baboon
```

## 三、使用

### 3.1 Wpf使用

在Wpf安装完成以后，需要在`App.xaml`和`App.xaml.cs`中，修改基类继承。

```
<baboon:BaboonApplication x:Class="Baboon.Wpf.App"
                          xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
                          xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
                          xmlns:baboon="clr-namespace:Baboon;assembly=Baboon">
    <Application.Resources />
</baboon:BaboonApplication>
```

同时需要在App.xaml.cs中，实现抽象类成员。包括注册容器和创建主窗口。

```
public partial class App : BaboonWpfApplication
{
    protected override Window CreateMainWindow()
    {
        return this.ServiceProvider.Resolve<MainWindow>();
    }

    protected override Task StartupAsync(AppModuleStartupEventArgs e)
    {
        //程序启动时执行
        //可以在这里通过ServiceProvider获取服务
        //this.ServiceProvider.Resolve<MainViewModel>();
        return Task.CompletedTask;
    }

    protected override Task InitializeAsync(AppModuleInitEventArgs e)
    {
        //程序初始化时执行
        //可以在这里注册服务

        //注册单例模式的View和ViewModel
        e.Services.AddSingletonView<MainWindow, MainViewModel>();
        return Task.CompletedTask;
    }
}
```

### 3.2 Winform使用

在Winform安装完成以后，需要在`Program.cs`中，修改部分逻辑。

首先，需要新建一个类，继承自`BaboonWinformApplication`。

然后重写CreateMainForm和InitializeAsync和StartupAsync。

```
class MyApp : BaboonWinformApplication
{
    protected override Form CreateMainForm()
    {
        return this.ServiceProvider.GetRequiredService<Form1>();
    }

    protected override Task InitializeAsync(AppModuleInitEventArgs e)
    {
        e.Services.AddSingleton<Form1>();
        return Task.CompletedTask;
    }

    protected override Task StartupAsync(AppModuleStartupEventArgs e)
    {
        return Task.CompletedTask;
    }
}
```

然后在Program.cs中，使用下列代码替换Main方法。

```
[STAThread]
static async Task Main()
{
    var myApp = new MyApp();
    await myApp.RunAsync();
}
```

## 四、使用模块

### 4.1 创建模块

新建一个库项目，命名为SayHello.Module，添加对`Baboon`的引用。

然后新建一个类，命名为SayHelloModule，继承自`AppModuleBase`或实现`IAppModule`接口。

然后实现基本成员。

```
public class SayHelloModule : AppModuleBase
{
    public SayHelloModule()
    {
        this.Description = new ModuleDescription("D7F3274A-2526-43FD-B278-099630BDA33E", "SayHello", new Version(1, 0, 0, 0), "RRQM", "test");
    }

    public override ModuleDescription Description { get; }

    protected override Task OnInitializeAsync(IApplication application, AppModuleInitEventArgs e)
    {
        return Task.CompletedTask;
    }

    protected override async Task OnStartupAsync(IApplication application, AppModuleStartupEventArgs e)
    {
        System.Windows.MessageBox.Show("Hello 模块已加载");
    }
}
```

### 4.2 发现、加载模块

然后将编译好的dll文件，放入到主运行程序的Modules文件夹下。因为Baboon会自动加载Modules文件夹下的所有模块。

如果你的库项目的名称不是以.Module结尾，则需要在主运行程序中（Wpf是BaboonWpfApplication，Winform是BaboonWinformApplication），重写FindModule方法，注册模块。

```
protected override bool FindModule(string path)
{
    var name = Path.GetFileNameWithoutExtension(path);
    return name.EndsWith("Module");
}
```

### 4.3 直接加载模块

如果你的模块是直接引用的，则可以直接使用。

例如：

```
 protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
 {
     //可以直接注册模块
     //moduleCatalog.Add<SayHelloModule>;
 }
```

## 主线程切换

Baboon提供了一个简单的线程切换的方法。来方便在子线程中切换到主线程。



```
```

#### Mvvm

Baboon简单的封装了基本的Mvvm框架，基本能满足使用。

【Cammand】

ExecuteCommand类。

【ViewModel】

ObservableObject和ViewModelBase类。

【容器注册View并绑定】

```
public partial class App : BaboonApplication
{
    ...
    protected override void RegisterTypes(IContainer container)
    {
        container.RegisterSingletonView<MainWindow, MainViewModel>();
    }
}
```

【绑定事件触发器】

使用EventAction实现。







#### 参与贡献

1.  Fork 本仓库
2.  新建 Feat_xxx 分支
3.  提交代码
4.  新建 Pull Request 
