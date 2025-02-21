using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Baboon;

public static class MainThreadTaskFactory
{
    private static Thread mainThread;
    private static SynchronizationContext mainThreadSyncContext;
    private static bool isInitialized;

    public static bool IsInitialized { get => isInitialized; }

    public static Thread Thread => mainThread;

    public static SynchronizationContext ThreadSyncContext => mainThreadSyncContext;

    public static void Initialize()
    {
        if (isInitialized)
        {
            throw new InvalidOperationException();
        }
        isInitialized = true;
        mainThreadSyncContext = SynchronizationContext.Current;
        mainThread = Thread.CurrentThread;
    }

    public static ReleaseMainThreadAwaitable ReleaseMainThreadAsync()
    {
        return new ReleaseMainThreadAwaitable();
    }

    public static MainThreadAwaitable SwitchToMainThreadAsync(CancellationToken cancellationToken = default)
    {
        return new MainThreadAwaitable(cancellationToken);
    }
    internal static void RequestSwitchToMainThread(Action callback)
    {
        if (mainThreadSyncContext != null)
        {
            mainThreadSyncContext.Post(_ => callback(), null);
        }
        else
        {
            callback();
        }
    }

    public readonly struct MainThreadAwaitable
    {
        private readonly CancellationToken cancellationToken;

        internal MainThreadAwaitable(CancellationToken cancellationToken)
        {
            this.cancellationToken = cancellationToken;
        }

        public MainThreadAwaiter GetAwaiter()
        {
            return new MainThreadAwaiter(cancellationToken);
        }
    }

    public readonly struct MainThreadAwaiter : ICriticalNotifyCompletion
    {
        private readonly CancellationToken cancellationToken;

        internal MainThreadAwaiter(CancellationToken cancellationToken)
        {
            this.cancellationToken = cancellationToken;
        }

        public bool IsCompleted => false;

        public void GetResult()
        {
            cancellationToken.ThrowIfCancellationRequested();
        }

        public void OnCompleted(Action continuation)
        {
            RequestSwitchToMainThread(continuation);
        }

        public void UnsafeOnCompleted(Action continuation)
        {
            OnCompleted(continuation);
        }
    }

    public readonly struct ReleaseMainThreadAwaitable
    {
        public ReleaseMainThreadAwaiter GetAwaiter()
        {
            return new ReleaseMainThreadAwaiter();
        }
    }

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
            OnCompleted(continuation);
        }
    }
}