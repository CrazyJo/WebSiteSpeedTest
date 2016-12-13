using System;
using System.Threading;
using System.Threading.Tasks;
using CrawlerBot.Core;

namespace CrawlerBot.Utilities
{
    public abstract class ThreadManager : IThreadManager
    {
        protected bool AbortAllCalled;
        protected int NumberOfRunningThreads;
        protected ManualResetEvent ResetEvent = new ManualResetEvent(true);
        protected object Locker = new object();
        protected bool IsDisplosed;

        protected ThreadManager(int maxThreads)
        {
            if ((maxThreads > 100) || (maxThreads < 1))
                throw new ArgumentException("MaxThreads must be from 1 to 100");

            MaxThreads = maxThreads;
        }

        /// <summary>
        /// Max number of threads to use
        /// </summary>
        public int MaxThreads { get; set; }

        /// <summary>
        /// Will perform the action asynchrously on a seperate thread
        /// </summary>
        public virtual async Task DoWork(Func<Task> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            if (AbortAllCalled)
                throw new InvalidOperationException("Cannot call DoWork() after AbortAll() or Dispose() have been called.");

            if (!IsDisplosed && MaxThreads > 1)
            {
                ResetEvent.WaitOne();
                lock (Locker)
                {
                    NumberOfRunningThreads++;
                    if (!IsDisplosed && NumberOfRunningThreads >= MaxThreads)
                        ResetEvent.Reset();
                }
                RunActionOnDedicatedThread(action);
            }
            else
            {
                await RunAction(action, false).ConfigureAwait(false);
            }
        }

        public virtual void AbortAll()
        {
            AbortAllCalled = true;
            NumberOfRunningThreads = 0;
        }

        public virtual void Dispose()
        {
            AbortAll();
            ResetEvent.Dispose();
            IsDisplosed = true;
        }

        public virtual bool HasRunningThreads()
        {
            return NumberOfRunningThreads > 0;
        }

        protected virtual async Task RunAction(Func<Task> action, bool decrementRunningThreadCountOnCompletion = true)
        {
            try
            {
                 await action.Invoke().ConfigureAwait(false);
            }
            catch (Exception e)
            {
                throw;
            }
            finally
            {
                if (decrementRunningThreadCountOnCompletion)
                {
                    lock (Locker)
                    {
                        NumberOfRunningThreads--;
                        if (!IsDisplosed && NumberOfRunningThreads < MaxThreads)
                            ResetEvent.Set();
                    }
                }
            }
        }

        /// <summary>
        /// Runs the action on a seperate thread
        /// </summary>
        protected abstract void RunActionOnDedicatedThread(Func<Task> action);
    }
}
