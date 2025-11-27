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

namespace Baboon.Mvvm;

public class EventAction<TSender, TE> : IEventAction
{
    private readonly Action<TSender, TE> m_action;

    public EventAction(string eventName, Action<TSender, TE> action)
    {
        this.EventName = eventName;
        this.m_action = action;
    }

    public string EventName { get; }

    private void Event(TSender sender, TE e)
    {
        this.m_action?.Invoke(sender, e);
    }
}