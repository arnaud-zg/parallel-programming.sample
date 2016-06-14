using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace AppParallelCalculator
{
    //
    class Program
    {
        //
        CancellationTokenSource tokenSource = new CancellationTokenSource();

        //
        static void Main(string[] args)
        {
            Program program = new Program();
            // program.runTaskClassic();
            // program.runTaskParallel();
            program.runTaskSynchronization();
            Console.ReadLine();
        }

        private void cancelTask()
        {
            tokenSource.Cancel();
        }

        //
        public void runTaskSynchronization()
        {
            var watch = Stopwatch.StartNew();
            List<Task> tasks = new List<Task>();
            Console.WriteLine("\n== Synchronization ==");
            for (int i = 1; i < 20; i++)
            {
                int j = i;
                var t = Task.Factory.StartNew(() =>
                {
                    var result = SumRootN(j);
                    Console.WriteLine("root {0} : {1} - {2} milliseconds",
                        i,
                        Math.Round(result, 2),
                        watch.ElapsedMilliseconds);
                }, tokenSource.Token);
                tasks.Add(t);
                 if (i == 10)
                    this.cancelTask();
            }
            Task.Factory.ContinueWhenAll(tasks.ToArray(),
                  result =>
                  {
                      var time = watch.ElapsedMilliseconds;
                      Console.WriteLine("=== Total time {0} milliseconds ===", time);
                  });
        }

        //
        public void runTaskClassic()
        {
            var watch = Stopwatch.StartNew();
            Console.WriteLine("\n== Classic ==");
            for (int i = 1; i < 20; i++)
            {
                var result = SumRootN(i);
                Console.WriteLine("root {0} : {1} - {2} milliseconds", 
                    i, 
                    Math.Round(result, 2), 
                    watch.ElapsedMilliseconds);
            }
            Console.WriteLine("=== Total time {0} milliseconds ===", watch.ElapsedMilliseconds);
        }

        //
        public void runTaskParallel()
        {
            var watch = Stopwatch.StartNew();
            Console.WriteLine("\n== Parallel ==");
            Parallel.For(1, 20, (i) =>
            {
                var result = SumRootN(i);
                Console.WriteLine("root {0} : {1} - {2} milliseconds",
                    i,
                    Math.Round(result, 2),
                    watch.ElapsedMilliseconds);
            });
            Console.WriteLine("=== Total time {0} milliseconds ===", watch.ElapsedMilliseconds);
        }

        // Sum of the nth root of all integers
        private double SumRootN(int root)
        {
            double result = 0;
            for (int i = 1; i < 10000000; i++)
            {
                result += Math.Exp(Math.Log(i) / root);
                tokenSource.Token.ThrowIfCancellationRequested();
            }
            return result;
        }
    }
}
