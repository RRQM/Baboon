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

## 五、使用IOC

Baboon使用了Microsoft.Extensions.DependencyInjection作为IOC容器。并且规范了使用时机。

### 5.1 注册服务

在主程序中，或者是模块中，重写InitializeAsync方法。即可注册服务。

```
protected override Task InitializeAsync(AppModuleInitEventArgs e)
{
    e.Services.AddSingleton<Form1>();
    return Task.CompletedTask;
}
```

### 5.2 获取服务

在主程序中，或者是模块中，重写StartupAsync方法。即可获取服务。

```
protected override Task StartupAsync(AppModuleStartupEventArgs e)
{
    var form1 = e.Services.GetRequiredService<Form1>();
    return Task.CompletedTask;
}
```


## 六、主线程切换

Baboon提供了一个简单的线程切换的方法。可以很方便的在子线程中切换到主线程，并执行一些UI操作。

>例如：
>我们有以下需求：
>在主窗体加载事件中，需要使用子线程，执行一些耗时的操作。执行完成后，需要把主窗体的Title修改为“Hello”。

那么在wpf中你只需要这样写：

```
private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
{
    //切换到子线程
    await Task.Run(async () =>
    {
        //模拟耗时操作
        for (int i = 0; i < 1000; i++)
        {
            Debug.WriteLine(i);
        }

        //切换到主线程
        await MainThreadTaskFactory.SwitchToMainThreadAsync();

        //更新UI
        this.Title = "Hello";
    });
}
```

在Winform中，你只需要这样写：

```
private async void Form1_Load(object sender, EventArgs e)
{
    //切换到子线程
    await Task.Run(async () =>
    {
        //模拟耗时操作
        for (int i = 0; i < 1000; i++)
        {
            Debug.WriteLine(i);
        }

        //切换到主线程
        await MainThreadTaskFactory.SwitchToMainThreadAsync();

        //更新UI
        this.Text = "Hello";
    });
}
```

## 七、Wpf相关操作

### 7.1 注册时绑定View和ViewModel

Baboon提供了一个简单的方法，来注册View和ViewModel。

```
e.Services.AddSingletonView<MainWindow, MainViewModel>();
```

### 7.2 区域导航

Baboon提供了一个简单的区域导航的方法。

首先，需要在主窗体的所需布局中，添加一个ContentControl，命名为contentRoot，作为导航的显示区域。


```
<ContentControl x:Name="contentRoot"/>
```

然后，在主窗体的构造函数中，注册区域导航。

```
public MainWindow(IRegionManager regionManager)
{
    InitializeComponent();
    regionManager.AddRoot("mainRoot",this.contentRoot);
}
```

然后创建一个基本的View和ViewModel。

例如：RegionControl和RegionControlViewModel。

然后需要注册导航。此处使用tag为“RegionControl”来标识。

```
e.Services.AddSingletonNavigate<RegionControl, RegionControlViewModel>("RegionControl");
```

假如，我们想在MainViewModel中导航到RegionControl到`mainRoot`。

那只需要在MainViewModel中注入IRegionManager，然后在MainViewModel的构造函数中，导航到RegionControl。

```
public MainViewModel(IRegionManager regionManager)
{
    this.regionManager.RequestNavigate("mainRoot", "RegionControl");
}

```