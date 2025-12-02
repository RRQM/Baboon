// ------------------------------------------------------------------------------
// 此代码版权（除特别声明或在XREF结尾的命名空间的代码）归作者本人若汝棋茗所有
// 源代码使用协议遵循本仓库的开源协议及附加协议，若本仓库没有设置，则按MIT开源协议授权
// CSDN博客：https://blog.csdn.net/qq_40374647
// 哔哩哔哩视频：https://space.bilibili.com/94253567
// Gitee源代码仓库：https://gitee.com/RRQM_Home
// Github源代码仓库：https://github.com/RRQM
// API首页：https://touchsocket.net/
// 交流QQ群：234762506
// 感谢您的下载和使用
// ------------------------------------------------------------------------------

using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;

namespace BaboonDemo.Avalonia;

public static class MenuItemBehaviors
{
    public static readonly AttachedProperty<ICommand?> ClickCommandProperty =
        AvaloniaProperty.RegisterAttached<MenuItem, ICommand?> (
            "ClickCommand",
            ownerType: typeof(MenuItemBehaviors));

    public static readonly AttachedProperty<object?> ClickCommandParameterProperty =
        AvaloniaProperty.RegisterAttached<MenuItem, object?> (
            "ClickCommandParameter",
            ownerType: typeof(MenuItemBehaviors));

    static MenuItemBehaviors()
    {
        ClickCommandProperty.Changed.AddClassHandler<MenuItem>(OnClickCommandChanged);
    }

    public static void SetClickCommand(AvaloniaObject element, ICommand? value)
        => element.SetValue(ClickCommandProperty, value);

    public static ICommand? GetClickCommand(AvaloniaObject element)
        => element.GetValue(ClickCommandProperty);

    public static void SetClickCommandParameter(AvaloniaObject element, object? value)
        => element.SetValue(ClickCommandParameterProperty, value);

    public static object? GetClickCommandParameter(AvaloniaObject element)
        => element.GetValue(ClickCommandParameterProperty);

    private static void OnClickCommandChanged(MenuItem item, AvaloniaPropertyChangedEventArgs e)
    {
        // always remove before re-adding to avoid duplicates
        item.Click -= OnMenuItemClick;

        if (e.NewValue is ICommand)
        {
            item.Click += OnMenuItemClick;
        }
    }

    private static void OnMenuItemClick(object? sender, global::Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (sender is MenuItem mi)
        {
            var command = GetClickCommand(mi);
            var parameter = GetClickCommandParameter(mi);

            if (command?.CanExecute(parameter) == true)
            {
                command.Execute(parameter);
            }
        }
    }
}
