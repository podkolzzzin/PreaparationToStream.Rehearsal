using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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


    [MemoryDiagnoser]
    public class StringBenchmark
    {
        private readonly string str1, str2, str3, str4, str5;

        public StringBenchmark()
        {
            str1 = new string('1', 10);
            str2 = new string('2', 20);
            str3 = new string('3', 30);
            str4 = new string('4', 40);
            str5 = new string('5', 50);
        }

        [Benchmark(Baseline = true)]
        public string StringPlus()
        {
            return str1 + str2 + str3 + str4 + str5; // String.Concat(str1, str2, str3, str4, str5);
        }

        [Benchmark]
        public string StringBuilderAppend()
        {
            var sb = new StringBuilder();
            sb.Append(str1);
            sb.Append(str2);
            sb.Append(str3);
            sb.Append(str4);
            sb.Append(str5);
            return sb.ToString();
        }
    }


    public class ListStructVsListClass
    {
        public class SampleRef
        {
            public Lazy<int> Value { get; set; }
            public string Test { get; set; }
        }

        public class SampleVal
        {
            public int Value { get; set; }
            public string Test { get; set; }
        }

        private readonly List<SampleRef> sampleRefs;
        private readonly List<SampleVal> sampleVals;
        public int Count = 60000;
        private readonly Random random = new Random(43);

        public ListStructVsListClass()
        {
            sampleRefs = Enumerable.Range(0, Count).Select(x => new SampleRef() { Test = Guid.NewGuid().ToString(), Value = new Lazy<int>(random.Next()) }).ToList();
            sampleVals = sampleRefs.Select(x => new SampleVal() { Test = x.Test, Value = x.Value.Value }).ToList();
        }

        [Benchmark]
        public string SampleRefsTest()
        {
            string result = null;
            foreach (var item in sampleRefs)
            {
                if (item.Value.Value % 2 == 0)
                    result = item.Test;
            }
            return result;
        }

        [Benchmark]
        public string SampleValsTest()
        {
            string result = null;
            foreach (var item in sampleVals)
            {
                if (item.Value % 2 == 0)
                    result = item.Test;
            }
            return result;
        }
    }

    [MemoryDiagnoser]
    public class LinkedListVsListBenchmark
    {
        private const int Count = 60000;

        private readonly List<int> list;
        private readonly LinkedList<int> linked;
        private readonly Random random = new Random(3);

        public LinkedListVsListBenchmark()
        {
            list = Enumerable.Range(0, Count).Select(x => random.Next()).ToList();
            linked = new LinkedList<int>();
            list.ForEach(x => linked.AddLast(x));
        }

        [Benchmark]
        public int ListIterate()
        {
            int result = 0;
            foreach (var item in list)
                result = item;
            return result;
        }

        [Benchmark]
        public int LinkedIterate()
        {
            int result = 0;
            foreach (var item in linked)
                result = item;
            return result;
        }

        [Benchmark]
        public List<int> ListAdd()
        {
            List<int> local = new List<int>();
            for (int i = 0; i < Count; i++)
            {
                local.Add(i);
            }
            return local;
        }

        [Benchmark]
        public LinkedList<int> LinkedAddLast()
        {
            LinkedList<int> local = new LinkedList<int>();
            for (int i = 0; i < Count; i++)
            {
                local.AddLast(i);
            }
            return local;
        }

        [Benchmark]
        public List<int> ListInsert()
        {
            List<int> local = new List<int>();
            for (int i = 0; i < Count; i++)
            {
                local.Insert(0, i);
            }
            return local;
        }

        [Benchmark]
        public LinkedList<int> LinkedAddFirst()
        {
            LinkedList<int> local = new LinkedList<int>();
            for (int i = 0; i < Count; i++)
            {
                local.AddFirst(i);
            }
            return local;
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<TaskRunThreadStartPoolQueueBenchmark>();
            BenchmarkRunner.Run<SleepDelayBenchmark>(DefaultConfig.Instance.AddColumn(StatisticColumn.P95));
        }
    }
}
