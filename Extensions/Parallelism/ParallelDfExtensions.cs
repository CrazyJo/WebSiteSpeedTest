using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Extensions.Parallelism
{
    public static class ParallelDfExtensions
    {
        // Environment.ProcessorCount
        static int degreeOfParallelism;

        public static int DegreeOfParallelism
        {
            get { return degreeOfParallelism; }
            set
            {
                if (value > 0)
                {
                    degreeOfParallelism = value;
                }
            }
        }

        static ParallelDfExtensions()
        {
            degreeOfParallelism = Environment.ProcessorCount;

            //int processorCount = Environment.ProcessorCount;

            //if (processorCount > 2)
            //{
            //    degreeOfParallelism = processorCount - 2;
            //}
            //else
            //{
            //    degreeOfParallelism = 1;
            //}
        }

        public static Task ForEach<TInput>(this IEnumerable source, Action<TInput> body)
        {
            var workerBlock = new ActionBlock<TInput>(body,
                new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = degreeOfParallelism });

            foreach (TInput node in source)
            {
                workerBlock.Post(node);
            }

            workerBlock.Complete();

            return workerBlock.Completion;
        }

        public static Task ForEach<T>(this IEnumerable<T> source, Func<T, Task> body)
        {
            var workerBlock = new ActionBlock<T>(body,
                new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = degreeOfParallelism });

            foreach (T node in source)
            {
                workerBlock.Post(node);
            }

            workerBlock.Complete();

            return workerBlock.Completion;
        }

        public static Task For(int fromInclusive, int toExclusive, Func<int, Task> body)
        {
            var workerBlock = new ActionBlock<int>(body,
                new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = degreeOfParallelism });

            for (; fromInclusive < toExclusive; fromInclusive++)
                workerBlock.Post(fromInclusive);

            workerBlock.Complete();
            return workerBlock.Completion;
        }

        public static Task For(int fromInclusive, int toExclusive, Action<int> body)
        {
            var workerBlock = new ActionBlock<int>(body, 
                new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = degreeOfParallelism });

            for (; fromInclusive < toExclusive; fromInclusive++)
                workerBlock.Post(fromInclusive);

            workerBlock.Complete();
            return workerBlock.Completion;
        }
    }
}
