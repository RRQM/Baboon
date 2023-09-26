# Baboon

#### 介绍
这是一个轻量级wpf插件化开发的基础库。它内置了模块化加载、模块化日志记录、模块化本地存储、模块化IOC注册、以及开发Mvvm必要的Command和EventTrigger。

#### 安装教程

nuget安装`Baboon`即可。

```
Install-Package Baboon
```

#### 使用

安装完成以后，需要在App.xaml和.cs中，修改基类继承。

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
public partial class App : BaboonApplication
{
    protected override Window CreateShell()
    {
        //创建主窗口
        return this.Container.Resolve<MainWindow>();
    }

    protected override void RegisterTypes(IContainer container)
    {
        //注册容器
        container.RegisterSingletonView<MainWindow,MainViewModel>();
    }
}
```

#### 新建模块

新建一个项目，命名为SayHello.Module，添加对`Baboon`的引用。

然后新建一个类，命名为SayHelloModule，继承自`AppModuleBase`或实现`IAppModule`接口。

然后实现基本成员。

```
public class SayHelloModule : AppModuleBase
{
    public SayHelloModule(ILoggerFactoryService loggerFactory) : base(loggerFactory)
    {
       
    }

    public override ModuleDescription Description { get; }

    public override ImageSource Icon => throw new NotImplementedException();

    public override void Show(object parameter = null)
    {
        MessageBox.Show("Hello");
    }
}
```

ModuleDescription是模块描述，可以直接赋值，也可以通过xml文件定义。

```xml
<?xml version="1.0" encoding="utf-8"?>
<package>
	<metadata>
		<Id>SayHello</Id>
		<DisplayName>SayHello</DisplayName>
		<Version>1.0.0</Version>
		<Authors>RRQM</Authors>
		<Description>会弹出Hello</Description>
		<!--<CoverImage>logo.png</CoverImage>-->
		<HasView>false</HasView>
	</metadata>
	<module>SayHello.Module.dll</module>
</package>
```

#### 注册模块

在App中，重写ConfigureModuleCatalog。可以注册模块。

注册的方式有三种，实例注册、类型注册、构建器注册。

实例注册和类型注册，时直接将模块注册到主程序。这会立刻加载模块，并且需要主程序引用模块。

构建器注册则是只注册模块信息，等需要模块的时候，自动加载模块。并且不需要主程序引用模块。

```
public partial class App : BaboonApplication
{
    ...

    protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
    {
        //以实例注册
        moduleCatalog.Add(new SayHelloModule(this.Container.Resolve<ILoggerFactoryService>()));

        //以类型注册
        moduleCatalog.Add(typeof(SayHelloModule));

        //以构建器注册
        moduleCatalog.Add(new ModuleDescriptionBuilder()
        {
            Description = new ModuleDescription(),
            Module = "",
            RootDir = ""
        });
    }
}
```

#### 加载、使用模块

模块的加载是自动的，即使使用构建器注册的。也会在需要时时机加载模块。

使用模块，需要从容器中获得IModuleCatalog。

然后可以Contains(id)的方法，判断是否已注册对应Id的模块。

通过TryGetAppModuleInfo可以获取到模块信息。

appModuleInfo.Loaded属性可以判断当前模块是否已经加载到主程序中。

appModuleInfo.GetApp()即可获取到指定的模块。

```
internal class MainViewModel : ViewModelBase
{
    private readonly IModuleCatalog m_moduleCatalog;

    public MainViewModel(IModuleCatalog moduleCatalog)
    {
        this.m_moduleCatalog = moduleCatalog;
    }

    private void RunSayHello()
    {
        if (!this.m_moduleCatalog.Contains("SayHello"))
        {
            MessageBox.Show("没有找到模块");
        }

        if (this.m_moduleCatalog.TryGetAppModuleInfo("SayHello", out var appModuleInfo))
        {
            if (!appModuleInfo.Loaded)//没有加载到主程序
            {
                if (MessageBox.Show("模块没有加载，是否加载？", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    var app = appModuleInfo.GetApp();

                    app.Show();
                }
            }
            else
            {
                var app = appModuleInfo.GetApp();

                app.Show();
            }
        }
    }
}
```

#### 模块管理器

使用IModuleManagerService服务，可以实现对模块的快速管理。包括安装、更新、卸载等。

同时IModuleCatalog也会对存放在Modules文件夹下的所有模块进行注册。但前提是每个模块必须包含一个名为Description.xml的文件。

#### 模块日志

每个模块可以拥有单独的日志记录器。通过IAppModule.Logger即可获得。

#### 模块本地数据库

Baboon集成了LiteDB数据库，并封装了KV键值存储。使用IConfigurationStoreService即可。

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
