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

using System.ComponentModel;
using System.Windows;

namespace Baboon.Mvvm;

public static class DesignerHelper
{
    private static bool m_isValueAlreadyValidated = default;


    /* 项目“Baboon (net6.0-windows)”的未合并的更改
    在此之前:
            private static bool m_isInDesignMode = default;


            public static bool IsInDesignMode => IsCurrentAppInDebugMode();
    在此之后:
            private static bool m_isInDesignMode = default;


            public static bool IsInDesignMode => IsCurrentAppInDebugMode();
    */
    private static bool m_isInDesignMode = default;


    public static bool IsInDesignMode => IsCurrentAppInDebugMode();


    public static bool IsDebugging => System.Diagnostics.Debugger.IsAttached;

    private static bool IsCurrentAppInDebugMode()
    {
        if (m_isValueAlreadyValidated)
        {
            return m_isInDesignMode;
        }

        m_isInDesignMode = (bool)(
            DesignerProperties.IsInDesignModeProperty
                .GetMetadata(typeof(DependencyObject))
                ?.DefaultValue ?? false
        );

        m_isValueAlreadyValidated = true;

        return m_isInDesignMode;
    }
}
