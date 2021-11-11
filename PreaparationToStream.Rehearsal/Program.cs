using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using System.Threading;
using System.Threading.Tasks;

namespace PreaparationToStream.Rehearsal
{
    public class SleepDelayBenchmark
    {
        [Benchmark(Baseline = true)]
        public void Sleep() => Thread.Sleep(1);

        [Benchmark]
        public Task Delay() => Task.Delay(1);
    }

    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<SleepDelayBenchmark>(DefaultConfig.Instance.AddColumn(StatisticColumn.P95));
        }
    }
}
