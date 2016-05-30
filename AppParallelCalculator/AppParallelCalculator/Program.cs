using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace AppParallelCalculator
{
    //
    class Program
    {
        //
        static void Main(string[] args)
        {
            runTaskClassic();
            runTaslParallel();
            Console.ReadLine();
        }

        //
        public static void runTaskClassic()
        {
            Console.WriteLine("== Classic ==");
            var watch = Stopwatch.StartNew();
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
        public static void runTaslParallel()
        {
            Console.WriteLine("== Parallel ==");
            var watch = Stopwatch.StartNew();
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

        // 
        public static double SumRootN(int root)
        {
            double result = 0;
            for (int i = 1; i < 10000000; i++)
                result += Math.Exp(Math.Log(i) / root);
            return result;
        }
    }
}
