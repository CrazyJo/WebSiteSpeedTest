using System;
using System.Threading;
using System.Threading.Tasks;

namespace CrawlerBot.Utilities
{
    public class TaskThreadManager : ThreadManager
    {
        readonly CancellationTokenSource _cancellationTokenSource;

        public TaskThreadManager(int maxConcurrentTasks)
            : this(maxConcurrentTasks, null)
        {
        }

        public TaskThreadManager(int maxConcurrentTasks, CancellationTokenSource cancellationTokenSource)
            : base(maxConcurrentTasks)
        {
            _cancellationTokenSource = cancellationTokenSource ?? new CancellationTokenSource();
        }

        public override void AbortAll()
        {
            base.AbortAll();
            _cancellationTokenSource.Cancel();
        }

        public override void Dispose()
        {
            base.Dispose();
            if (!_cancellationTokenSource.IsCancellationRequested)
                _cancellationTokenSource.Cancel();
        }

        protected override void RunActionOnDedicatedThread(Func<Task> action)
        {
            Task.Run(async () => await RunAction(action).ConfigureAwait(false), _cancellationTokenSource.Token);
        }
    }
}
