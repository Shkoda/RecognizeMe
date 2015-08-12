namespace GlobalPlay.Threading
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading;

    public static class ThreadManager
    {
        private static readonly Dictionary<int, Thread> _threads = new Dictionary<int, Thread>();

        private static readonly Dictionary<int, string> _descriptions = new Dictionary<int, string>();

        private static volatile int _threadCount = 1;

        private static volatile bool _isAborting;

        // public static int MainThreadId;
        public static int ThreadCount
        {
            get { return _threadCount; }
        }

        public static bool IsRunningInMainThread
        {
            get { return Thread.CurrentThread.ManagedThreadId == MainThreadId; }
        }

        public static int MainThreadId { get; private set; }

        public static void RegisterThread(string descr)
        {
            try
            {
                int currentThreadId = Thread.CurrentThread.ManagedThreadId;

                lock (_threads)
                {
                    if (_threads.ContainsKey(currentThreadId))
                    {
                        return;
                    }

                    _threads.Add(currentThreadId, Thread.CurrentThread);
                }

                lock (_descriptions)
                {
                    _descriptions.Add(currentThreadId, descr);
                }

                Interlocked.Increment(ref _threadCount);

                Debugger.Log(string.Format("Registered thread {0}: {1}", currentThreadId, descr), DebugType.Threading);
            }
            catch (Exception e)
            {
                Debugger.Log(
                    string.Format(
                        "Error in ThreadManager while adding {0}. {1}. State of ThreadManager is not guaranted to represent actual information about threading anymore.",
                        descr,
                        e.Message));
            }
        }

        public static void UnRegisterThread()
        {
            try
            {
                if (!_isAborting)
                {
                    int currentThreadId = Thread.CurrentThread.ManagedThreadId;

                    lock (_descriptions)
                    {
                        Debugger.Log(
                            string.Format(
                                "Unregistering thread {0}: {1}",
                                currentThreadId,
                                _descriptions[currentThreadId]),
                            DebugType.Threading);
                        _descriptions.Remove(currentThreadId);
                    }

                    lock (_threads)
                    {
                        _threads.Remove(currentThreadId);
                    }

                    Interlocked.Decrement(ref _threadCount);
                }
                else
                {
                    int currentThreadId = Thread.CurrentThread.ManagedThreadId;
                    Debugger.Log(
                        string.Format(
                            "Unregistering (aborted) thread {0} (no description available on abort)",
                            currentThreadId),
                        DebugType.Threading);
                }
            }
            catch (Exception e)
            {
                Debugger.Log(
                    string.Format(
                        "Error in ThreadManager while removing. {0}. State of ThreadManager is not guaranted to represent actual information about threading anymore.",
                        e.Message));
                throw;
            }
        }

        public static void OutputRunningThreads()
        {
            var sb = new StringBuilder();
            int count = 0;
            lock (_descriptions)
            {
                count = _descriptions.Count;
                foreach (var description in _descriptions)
                {
                    sb.Append(description + ", ");
                }
            }

            Debugger.Log(string.Format("Running threads ({1}): {0}", sb, count), writeToUnityConsole: true);
        }

        public static void AbortAll()
        {
            _isAborting = true;
            lock (_threads)
            {
                foreach (var thread in _threads)
                {
                    // No fooling around
                    if (thread.Key != MainThreadId)
                    {
                        thread.Value.Abort();
                    }
                }
            }

            _isAborting = false;
        }

        public static void SetMainThread()
        {
            MainThreadId = Thread.CurrentThread.ManagedThreadId;
        }
    }
}