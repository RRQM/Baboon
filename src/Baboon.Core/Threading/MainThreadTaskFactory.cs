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
using System.Runtime.CompilerServices;
using System.Threading;

namespace Baboon.Core;

/// <summary>
/// 主线程任务工厂
/// </summary>
public static class MainThreadTaskFactory
{
    private static Thread s_mainThread;
    private static SynchronizationContext s_mainThreadSyncContext;
    private static bool s_isInitialized;

    /// <summary>
    /// 获取是否已初始化
    /// </summary>
    public static bool IsInitialized => s_isInitialized;

    /// <summary>
    /// 获取主线程
    /// </summary>
    public static Thread Thread => s_mainThread;

    /// <summary>
    /// 获取主线程同步上下文
    /// </summary>
    public static SynchronizationContext ThreadSyncContext => s_mainThreadSyncContext;

    /// <summary>
    /// 初始化主线程任务工厂
    /// </summary>
    public static void Initialize()
    {
        if (s_isInitialized)
        {
            throw new InvalidOperationException();
        }
        s_isInitialized = true;
        s_mainThreadSyncContext = SynchronizationContext.Current;
        s_mainThread = Thread.CurrentThread;
    }

    /// <summary>
    /// 释放主线程异步操作
    /// </summary>
    /// <returns>释放主线程的可等待对象</returns>
    public static ReleaseMainThreadAwaitable ReleaseMainThreadAsync()
    {
        return new ReleaseMainThreadAwaitable();
    }

    /// <summary>
    /// 切换到主线程异步操作
    /// </summary>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>切换到主线程的可等待对象</returns>
    public static MainThreadAwaitable SwitchToMainThreadAsync(CancellationToken cancellationToken = default)
    {
        return new MainThreadAwaitable(cancellationToken);
    }

    /// <summary>
    /// 请求切换到主线程
    /// </summary>
    /// <param name="callback">回调操作</param>
    internal static void RequestSwitchToMainThread(Action callback)
    {
        if (s_mainThreadSyncContext != null)
        {
            s_mainThreadSyncContext.Post(_ => callback(), null);
        }
        else
        {
            callback();
        }
    }

    /// <summary>
    /// 主线程可等待对象
    /// </summary>
    public readonly struct MainThreadAwaitable
    {
        private readonly CancellationToken m_cancellationToken;

        internal MainThreadAwaitable(CancellationToken cancellationToken)
        {
            this.m_cancellationToken = cancellationToken;
        }

        public MainThreadAwaiter GetAwaiter()
        {
            return new MainThreadAwaiter(this.m_cancellationToken);
        }
    }

    /// <summary>
    /// 主线程等待者
    /// </summary>
    public readonly struct MainThreadAwaiter : ICriticalNotifyCompletion
    {
        private readonly CancellationToken m_cancellationToken;

        internal MainThreadAwaiter(CancellationToken cancellationToken)
        {
            this.m_cancellationToken = cancellationToken;
        }

        public bool IsCompleted => false;

        public void GetResult()
        {
            this.m_cancellationToken.ThrowIfCancellationRequested();
        }

        public void OnCompleted(Action continuation)
        {
            RequestSwitchToMainThread(continuation);
        }

        public void UnsafeOnCompleted(Action continuation)
        {
            this.OnCompleted(continuation);
        }
    }

    /// <summary>
    /// 释放主线程可等待对象
    /// </summary>
    public readonly struct ReleaseMainThreadAwaitable
    {
        public ReleaseMainThreadAwaiter GetAwaiter()
        {
            return new ReleaseMainThreadAwaiter();
        }
    }

    /// <summary>
    /// 释放主线程等待者
    /// </summary>
    public readonly struct ReleaseMainThreadAwaiter : ICriticalNotifyCompletion
    {
        public bool IsCompleted => false;

        public void GetResult()
        {
            // 这里简单处理，实际可能需要更复杂的逻辑
        }

        public void OnCompleted(Action continuation)
        {
            ThreadPool.UnsafeQueueUserWorkItem((o) => continuation.Invoke(), null);
        }

        public void UnsafeOnCompleted(Action continuation)
        {
            this.OnCompleted(continuation);
        }
    }
}
