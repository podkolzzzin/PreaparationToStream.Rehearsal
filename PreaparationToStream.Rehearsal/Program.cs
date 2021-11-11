using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using System.Threading;
using System.Threading.Tasks;

namespace PreaparationToStream.Rehearsal
{
    [MemoryDiagnoser]
    public class SleepDelayBenchmark
    {
        [Params(1, 5, 50)]
        public int DelayMS;

        [Benchmark(Baseline = true)]
        public void Sleep() => Thread.Sleep(DelayMS);

        [Benchmark]
        public Task Delay() => Task.Delay(DelayMS);
    }

    [MemoryDiagnoser]
    public class TaskRunThreadStartPoolQueueBenchmark
    {
        [Benchmark]
        public void TaskRun()
        {
            bool b = false;
            Task.Run(() =>
            {
                b = true;
            });
            while (!Volatile.Read(ref b))
                ;
        }

        [Benchmark]
        public void ThreadPool()
        {
            bool b = false;
            System.Threading.ThreadPool.QueueUserWorkItem(o =>
            {
                b = true;
            });
            while (!Volatile.Read(ref b))
                ;
        }

        [Benchmark(Baseline = true)]
        public void ThreadStart()
        {
            bool b = false;
            var t = new Thread(() =>
            {
                b = true;
            });
            t.Start();
            while (!Volatile.Read(ref b))
                ;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<TaskRunThreadStartPoolQueueBenchmark>();
            //BenchmarkRunner.Run<SleepDelayBenchmark>(DefaultConfig.Instance.AddColumn(StatisticColumn.P95));
        }
    }
}
