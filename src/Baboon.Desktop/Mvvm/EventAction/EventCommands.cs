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

using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;

namespace Baboon.Mvvm;

public static class EventCommands
{
    public static IEnumerable<IEventAction> GetEvents(DependencyObject obj)
    {
        return (IEnumerable<IEventAction>)obj.GetValue(EventsProperty);
    }

    public static void SetEvents(DependencyObject obj, IEnumerable<IEventAction> value)
    {
        obj.SetValue(EventsProperty, value);
    }

    // Using a DependencyProperty as the backing store for Events.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty EventsProperty =
        DependencyProperty.RegisterAttached("Events", typeof(IEnumerable<IEventAction>), typeof(EventCommands), new PropertyMetadata(null, OnCommandChanged));

    private static void OnCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (e.NewValue is IEnumerable eventActions)
        {
            foreach (IEventAction eventAction in eventActions)
            {
                if (!string.IsNullOrEmpty(eventAction.EventName))
                {
                    var eventInfo = d.GetType().GetEvent(eventAction.EventName);
                    if (eventInfo == null)
                    {
                        throw new Exception($"没有找到名称为{eventAction.EventName}的事件");
                    }
                    var @delegate = Delegate.CreateDelegate(eventInfo.EventHandlerType, eventAction, "Event");

                    //Delegate @delegate2 = eventAction.Begin(eventInfo.EventHandlerType, typeof(object), typeof(MouseButtonEventArgs));
                    //Delegate @delegate = DelegateBuilder.CreateDelegate(eventAction, "Event", eventInfo.EventHandlerType, BindingFlags.NonPublic);
                    eventInfo.AddEventHandler(d, @delegate);
                }
                else
                {
                    throw new Exception($"事件名不能为空");
                }
            }
        }
    }
}