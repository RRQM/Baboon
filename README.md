# Baboon

## 一、介绍

这是一个轻量级`wpf`和`winform`的插件化开发的基础库。它内置了模块化加载、模块化日志记录、模块化`IOC`注册、以及开发`wpf`时的`Mvvm`必要的`Command`和`EventTrigger`。

## 二、安装教程

nuget安装`Baboon`即可。

```
Install-Package Baboon
```

## 三、使用

### 3.1 Wpf使用

在`Wpf`安装完成以后，需要在`App.xaml`和`App.xaml.cs`中，修改基类继承。

```xml
<baboon:BaboonWpfApplication x:Class="Baboon.Wpf.App"
                          xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
                          xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
                          xmlns:baboon="clr-namespace:Baboon;assembly=Baboon">
    <Application.Resources />
</baboon:BaboonWpfApplication>
```

同时需要在`App.xaml.cs`中，实现抽象类成员。包括注册容器和创建主窗口。

```csharp
public partial class App : BaboonWpfApplication
{
    protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
    {
        //可以直接注册模块
        //moduleCatalog.Add<SayHelloModule>;
    }

    protected override Window CreateMainWindow(IWindowManager windowManager)
    {
        return windowManager.GetWindow<MainWindow>();
    }

    protected override Task InitializeAsync(AppModuleInitEventArgs e)
    {
        //程序初始化时执行
        //可以在这里注册服务

        //注册单例模式的View和ViewModel
        e.Services.AddSingletonView<MainWindow, MainViewModel>();
        return Task.CompletedTask;
    }

    protected override Task StartupAsync(AppModuleStartupEventArgs e)
    {
        //程序启动时执行
        //可以在这里通过ServiceProvider获取服务
        //this.ServiceProvider.Resolve<MainViewModel>();
        return Task.CompletedTask;
    }
}
```

### 3.2 Winform使用

在`Winform`安装完成以后，需要在`Program.cs`中，修改部分逻辑。

首先，需要新建一个类，继承自`BaboonWinformApplication`。

然后重写`CreateMainForm`和`InitializeAsync`和`StartupAsync`。

```csharp
class MyApp : BaboonWinformApplication
{
    protected override Form CreateMainForm(IFormManager formManager)
    {
        return formManager.GetForm<Form1>();
    }

    protected override Task InitializeAsync(AppModuleInitEventArgs e)
    {
        //注意：非必要，不要把Form注册到容器中，不然无法释放内存。
        //添加服务
        //e.Services.AddSingleton<>();
        return Task.CompletedTask;
    }

    protected override Task StartupAsync(AppModuleStartupEventArgs e)
    {
        return Task.CompletedTask;
    }
}
```

然后在`Program.cs`中，使用下列代码替换`Main`方法。

```csharp
[STAThread]
static void Main()
{
    var myApp = new MyApp();
    myApp.Run();
}
```

## 四、使用模块

### 4.1 创建模块

新建一个库项目，命名为`SayHello.Module`，添加对`Baboon`的引用。

然后新建一个类，命名为`SayHelloModule`，继承自`AppModuleBase`或实现`IAppModule`接口。

然后实现基本成员。

```csharp
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

然后将编译好的dll文件，放入到主运行程序的`Modules`文件夹下。因为`Baboon`会自动加载`Modules`文件夹下的所有模块。

如果你的库项目的名称不是以`.Module`结尾，则需要在主运行程序中（`Wpf`是`BaboonWpfApplication`，`Winform`是`BaboonWinformApplication`），重写`FindModule`方法，注册模块。

```csharp
protected override bool FindModule(string path)
{
    var name = Path.GetFileNameWithoutExtension(path);
    return name.EndsWith("Module");
}
```

### 4.3 直接加载模块

如果你的模块是直接引用的，则可以直接使用。

例如：

```csharp
 protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
 {
     //可以直接注册模块
     //moduleCatalog.Add<SayHelloModule>;
 }
```

## 五、使用IOC

`Baboon`使用了`Microsoft.Extensions.DependencyInjection`作为`IOC`容器。并且规范了使用时机。

### 5.1 注册服务

在主程序中，或者是模块中，重写`InitializeAsync`方法。即可注册服务。

```csharp
protected override Task InitializeAsync(AppModuleInitEventArgs e)
{
    e.Services.AddSingleton<Form1>();
    return Task.CompletedTask;
}
```

### 5.2 获取服务

在主程序中，或者是模块中，重写`StartupAsync`方法。即可获取服务。

```csharp
protected override Task StartupAsync(AppModuleStartupEventArgs e)
{
    var form1 = e.Services.GetRequiredService<Form1>();
    return Task.CompletedTask;
}
```


## 六、主线程切换

`Baboon`提供了一个简单的线程切换的方法。可以很方便的在子线程中切换到主线程，并执行一些`UI`操作。

>例如：
>我们有以下需求：
>在主窗体加载事件中，需要使用子线程，执行一些耗时的操作。执行完成后，需要把主窗体的`Title`修改为“Hello”。

那么在wpf中你只需要这样写：

```csharp
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

在`Winform`中，你只需要这样写：

```csharp
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

`Baboon`提供了一个简单的方法，来注册`View`和`ViewModel`。

```csharp
e.Services.AddSingletonView<MainWindow, MainViewModel>();
```

### 7.2 区域导航

`Baboon`提供了一个简单的区域导航的方法。

首先，需要在主窗体的所需布局中，添加一个`ContentControl`，命名为`contentRoot`，作为导航的显示区域。


```xml
<ContentControl x:Name="contentRoot"/>
```

然后，在主窗体的构造函数中，注册区域导航。

```csharp
public MainWindow(IRegionManager regionManager)
{
    InitializeComponent();
    regionManager.AddRoot("mainRoot",this.contentRoot);
}
```

然后创建一个基本的`View`和`ViewModel`。

例如：`RegionControl`和`RegionControlViewModel`。

然后需要注册导航。此处使用tag为“RegionControl”来标识。

```csharp
e.Services.AddSingletonNavigate<RegionControl, RegionControlViewModel>("RegionControl");
```

假如，我们想在`MainViewModel`中导航到`RegionControl`到`mainRoot`。

那只需要在`MainViewModel`中注入`IRegionManager`，然后在`MainViewModel`的构造函数中，导航到`RegionControl`。

```csharp
public MainViewModel(IRegionManager regionManager)
{
    this.regionManager.RequestNavigate("mainRoot", "RegionControl");
}

```

### 7.3 全局资源

每个模块在加载时，可以使用自己领域的资源。但是如果需要把自己的资源分享到全局资源时，那么可以使用`Baboon`提供的全局资源服务来添加。

例如：

```csharp
protected override async Task OnStartupAsync(IApplication application, AppModuleStartupEventArgs e)
{
    var resourceService = this.ServiceProvider.GetRequiredService<IResourceService>();
    resourceService.AddResourceDictionary(new ResourceDictionary());
}
```

## 八、消息通知

### 8.1 CommunityToolkit.Mvvm消息通知

`Baboon`引用了`CommunityToolkit.Mvvm`，所以可以使用它的消息通知。以下是一个使用 `CommunityToolkit.Mvvm` 进行消息通知的示例代码，包含消息定义、发送者和接收者的实现。

#### 定义消息类

我们首先定义一个消息类，用于在不同的组件之间传递数据。

```csharp
// 定义一个消息类，继承自 ValueChangedMessage<string>
// 这里使用泛型，指定消息携带的数据类型为 string
using CommunityToolkit.Mvvm.Messaging.Messages;

// 定义一个自定义消息类，继承自 ValueChangedMessage<string>
// 用于在不同组件之间传递字符串类型的消息
public class CustomMessage : ValueChangedMessage<string>
{
    public CustomMessage(string value) : base(value)
    {
    }
}
```

#### 实现消息接收者

接下来，我们创建一个消息接收者类，该类将订阅 `CustomMessage` 并处理接收到的消息。
```csharp
// 定义一个消息接收者类
public class MessageReceiver
{
    public MessageReceiver(IMessenger messenger)
    {
        // 注册消息接收处理方法
        messenger.Register<CustomMessage>(this, (r, m) =>
        {
            // 处理接收到的消息
            Console.WriteLine($"Received message: {m.Value}");
        });
    }
}
```

#### 实现消息发送者

然后，我们创建一个消息发送者类，该类将发送 `CustomMessage`。

```csharp
// 定义一个消息发送者类
public class MessageSender
{
    private readonly IMessenger _messenger;

    public MessageSender(IMessenger messenger)
    {
        _messenger = messenger;
    }

    public void SendMessage(string message)
    {
        // 发送自定义消息
        _messenger.Send(new CustomMessage(message));
    }
}
```

#### 在需要的方法中使用消息通知

最后，在方法中创建消息发送者和接收者，并发送消息。

```csharp
static void Test()
{
    // 创建一个全局的 Messenger 实例
    var messenger = WeakReferenceMessenger.Default;

    // 创建消息接收者实例
    var receiver = new MessageReceiver(messenger);
    // 创建消息发送者实例
    var sender = new MessageSender(messenger);

    // 发送消息
    sender.SendMessage("Hello, Messenger!");
}
```

#### 运行结果

当你运行上述代码时，控制台将输出：
```
Received message: Hello, Messenger!
```

这表明消息成功从发送者传递到了接收者。

### 8.2 使用TouchSocket.Core应用信使

#### 说明

应用信使是在进程内的，行使注册和触发功能的组件。可**代替事件**，可**跨越程序集**，可**依赖倒置**。

#### 注册

下列演示时，是使用`AppMessenger.Default`默认实例，实际上，用户可以自己新实例化的`AppMessenger`。

#### 注册实例

首先让类实例实现`IMessageObject`接口，然后在实例类中声明**异步公共实例**方法，并使用`AppMessage`特性标记。

然后一般情况下，建议在构造函数中，注册消息。

```csharp
public class MessageObject : IMessageObject
{
    public MessageObject()
    {
        AppMessenger.Default.Register(this);
    }

    [AppMessage]
    public Task<int> Add(int a, int b)
    {
        return Task.FromResult(a + b);
    }

    [AppMessage]
    public Task<int> Sub(int a, int b)
    {
        return Task.FromResult(a - b);
    }
}
```

对于实例类，如果构造函数中，没有注册消息，那么在构造函数之后，也可以使用其**实例**注册消息。

```csharp {2} showLineNumbers
var messageObject = new MessageObject();
AppMessenger.Default.Register(messageObject);
```

#### 注册静态方法

注册静态方法，只需在类中直接声明**异步公共实例**方法，并使用`AppMessage`特性标记即可。

```csharp showLineNumbers
public static class MessageObject : IMessageObject
{
    [AppMessage]
    public static Task<int> StaticAdd(int a, int b)
    {
        return Task.FromResult(a + b);
    }
}
```

使用`RegisterStatic`进行注册

```csharp  showLineNumbers
AppMessenger.Default.RegisterStatic<MessageObject>();
```

#### 触发

触发时，泛型类型，即时返回值类型。

```csharp showLineNumbers
int add = await appMessenger.SendAsync<int>("Add", 20, 10);

int sub =await appMessenger.SendAsync<int>("Sub", 20, 10);
```

