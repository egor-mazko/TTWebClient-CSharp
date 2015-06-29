using System;
using System.Collections.Generic;
using System.Linq;

namespace TTWebClientRobot.Statistic
{
    public class Aggregator
    {
        private readonly Dictionary<string, Benchmark> _benchmarks = new Dictionary<string, Benchmark>();

        public BenchmarkScope Benchmark(string name)
        {
            Benchmark benchmark;
            if (!_benchmarks.TryGetValue(name, out benchmark))
            {
                benchmark = new Benchmark(name);
                _benchmarks.Add(name, benchmark);
            }

            return new BenchmarkScope(benchmark);
        }

        public void PrintStatistic()
        {
            Console.WriteLine("--- Statistic ---");
            foreach (var benchmark in _benchmarks.Values.OrderBy(v => v.Name))
            {
                benchmark.PrintStatistic();
                Console.WriteLine("------");
            }
        }
    }
}
