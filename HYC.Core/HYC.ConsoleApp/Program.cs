using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HYC.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            ////准确获取运行时间
            //System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

            //List<int> ids = new List<int>();
            //for (var i = 0; i < 10; i++)
            //{
            //    ids.Add(i);
            //}
            //int maxTasks = 3;

            //#region ===方式1===
            //sw.Start();
            //List<Task> tasks = new List<Task>();
            //TaskFactory factory = new TaskFactory();
            //ids.ForEach(id =>
            //{
            //    tasks.Add(factory.StartNew(() =>
            //    {
            //        Console.WriteLine(id);
            //    }));

            //    if (tasks.Count > maxTasks)
            //    {
            //        Task.WaitAny(tasks.ToArray());
            //        tasks = tasks.Where(t => t.Status == TaskStatus.Running).ToList();
            //    }
            //});

            //sw.Stop();
            //Console.WriteLine("总耗时 = " + sw.ElapsedMilliseconds + "(ms)");
            //Console.ReadKey();
            //#endregion

            //#region ===方式2===
            //sw.Restart();
            //sw.Start();
            //ParallelOptions parallelOptions = new ParallelOptions();
            //parallelOptions.MaxDegreeOfParallelism = maxTasks;

            //ParallelLoopResult result = Parallel.ForEach(ids, parallelOptions, (id, state, i) =>
            // {
            //     if (i > 5)
            //     {
            //         state.Break();
            //     }

            //     Console.WriteLine(id);
            // });
            //sw.Stop();
            //Console.WriteLine("总耗时 = " + sw.ElapsedMilliseconds + "(ms)");

            //Console.WriteLine("是否完成:{0}", result.IsCompleted);
            //Console.WriteLine("最低迭代:{0}", result.LowestBreakIteration);
            //Console.ReadKey();
            //#endregion


            //#region parallel.Invoke
            //Parallel.Invoke(() =>
            //{
            //    Console.WriteLine("method1");
            //}, () =>
            //{
            //    Console.WriteLine("method2");
            //});

            //#endregion

            #region ===安全线程
            //List<int> list = new List<int>();
            //Parallel.For(0, 10000, item =>
            //{
            //    list.Add(item);
            //});
            ////结果不对？这是因为List<T>是非线程安全集合，意思就是说所有的线程都可以修改他的值
            //Console.WriteLine("List's count is {0}", list.Count());

            ///*正确做法            
            // * 线程安全集合，在System.Collections.Concurrent命名空间中，
            // * 看一下ConcurrentBag<T>泛型集合，其用法和List<T>类似
            // * 例如Dictionary的ConcurrentDictionary
            //*/
            //ConcurrentBag<int> list2 = new ConcurrentBag<int>();
            //Parallel.For(0, 10000, item =>
            //{
            //    list2.Add(item);
            //});
            //Console.WriteLine("ConcurrentBag's count is {0}", list2.Count());
            //Console.ReadLine();
            #endregion

            //SpinLock slock = new SpinLock(false);
            //long sum1 = 0;
            //long sum2 = 0;
            //Parallel.For(0, 100000, i =>
            //{
            //    sum1 += i;
            //});

            //Parallel.For(0, 100000, i =>
            //{
            //    bool lockTaken = false;
            //    try
            //    {
            //        slock.Enter(ref lockTaken);
            //        sum2 += i;
            //    }
            //    finally
            //    {
            //        if (lockTaken)
            //            slock.Exit(false);
            //    }
            //});
            //Console.WriteLine("结果1的值为:{0}", sum1);
            //Console.WriteLine("结果2的值为:{0}", sum2);
            //Console.Read();
        }
    }
}
